using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace omech.Helpers
{
    /// <summary>
    /// Device registration support for access from outside the allowed IPs
    /// (e.g. phones on mobile data). The IP allowlist itself is unchanged:
    /// a request from an allowed IP never reaches any of this.
    ///
    /// A registered phone sends its secret key in the X-Device-Token header.
    /// Only the SHA-256 hash of that key is stored (M_USER_DEVICE).
    /// </summary>
    public static class DeviceAccess
    {
        public const string HeaderName = "X-Device-Token";

        // HttpContext.Items keys set by the IP middleware
        public const string IpAllowedItem = "OMECH_IP_ALLOWED";
        public const string ClientIpItem = "OMECH_CLIENT_IP";

        // Endpoints a not-yet-approved phone must be able to reach
        // (they do their own checks: credentials, rate limit).
        private static readonly string[] PublicPaths =
        {
            "/api/device/status",
            "/api/device/request",
        };

        // How long a lookup result is cached, so the DB is hit at most about
        // once a minute per device. A revoke therefore takes effect within ~1 min.
        private static readonly TimeSpan ValidCacheTime = TimeSpan.FromSeconds(60);
        private static readonly TimeSpan InvalidCacheTime = TimeSpan.FromSeconds(15);

        public static bool IsPublicPath(PathString path)
        {
            foreach (var p in PublicPaths)
            {
                if (path.Equals(p, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>New random device key: 32 bytes, base64url (43 chars).</summary>
        public static string NewKey()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        /// <summary>Cheap format check so junk headers never hit the database.</summary>
        public static bool IsWellFormedKey(string? key)
        {
            if (string.IsNullOrEmpty(key) || key.Length != 43) return false;
            foreach (var c in key)
            {
                if (!(char.IsAsciiLetterOrDigit(c) || c == '-' || c == '_')) return false;
            }
            return true;
        }

        /// <summary>SHA-256 of the key as 64 uppercase hex chars (what the DB stores).</summary>
        public static string HashKey(string key)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
        }

        public static string? GetKey(HttpContext context)
        {
            var key = context.Request.Headers[HeaderName].FirstOrDefault()?.Trim();
            return IsWellFormedKey(key) ? key : null;
        }

        /// <summary>
        /// True when the request carries the key of an APPROVED, not-expired device
        /// whose owner is still active. Fails closed on any error.
        /// </summary>
        public static bool IsApprovedDevice(HttpContext context, string? ip)
        {
            var key = GetKey(context);
            if (key == null) return false;

            var hash = HashKey(key);
            var cache = context.RequestServices.GetRequiredService<IMemoryCache>();
            var cacheKey = "omech-device:" + hash;

            if (cache.TryGetValue(cacheKey, out bool cached)) return cached;

            try
            {
                var db = context.RequestServices.GetRequiredService<DatabaseHelper>();
                var ds = db.ExecuteStoredProcedureAsDataSet("DT_DEVICE_VALIDATE", new Dictionary<string, object>
                {
                    { "@DEVICE_KEY_HASH", hash },
                    { "@IP", Truncate(ip, 45) },
                    { "@TOUCH", true },
                });

                var valid = ds.Tables.Count > 0
                            && ds.Tables[0].Rows.Count > 0
                            && Convert.ToInt32(ds.Tables[0].Rows[0]["IS_VALID"]) == 1;

                cache.Set(cacheKey, valid, valid ? ValidCacheTime : InvalidCacheTime);
                return valid;
            }
            catch
            {
                return false; // DB problem: deny, and don't cache the failure
            }
        }

        /// <summary>
        /// Simple fixed-window limit per IP for the public device endpoints.
        /// Returns false when the caller has exceeded the limit.
        /// </summary>
        public static bool TryConsumeRequestQuota(HttpContext context, string bucket, int maxPerWindow, TimeSpan window)
        {
            var ip = context.Items[ClientIpItem] as string ?? "unknown";
            var cache = context.RequestServices.GetRequiredService<IMemoryCache>();
            var cacheKey = $"omech-rate:{bucket}:{ip}";

            var counter = cache.GetOrCreate(cacheKey, e =>
            {
                e.AbsoluteExpirationRelativeToNow = window;
                return new RateCounter();
            })!;

            return Interlocked.Increment(ref counter.Count) <= maxPerWindow;
        }

        public static string? Truncate(string? value, int max)
        {
            if (value == null) return null;
            return value.Length <= max ? value : value.Substring(0, max);
        }

        private sealed class RateCounter
        {
            public int Count;
        }
    }
}
