using System.Data;
using omech.Helpers;
using omech.Models;

namespace omech.Services
{
    public interface IDeviceService
    {
        object GetStatus(string? deviceKey, bool ipAllowed);
        object RequestAccess(DeviceRequestModel model, string? ip, string? userAgent);
        object ListDevices(int adminUserSrno, string? deviceStatus);
        object DeviceAction(int adminUserSrno, DeviceActionModel model);
    }

    /// <summary>
    /// Device registration: phones outside the allowed IPs request access,
    /// admins approve / reject / renew / revoke them (see M_USER_DEVICE).
    /// </summary>
    public class DeviceService : IDeviceService
    {
        private readonly DatabaseHelper _databaseHelper;

        public DeviceService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public object GetStatus(string? deviceKey, bool ipAllowed)
        {
            try
            {
                string deviceStatus = "NONE";
                object? deviceName = null, expiresAt = null;

                if (deviceKey != null)
                {
                    var ds = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_DEVICE_VALIDATE", new Dictionary<string, object>
                    {
                        { "@DEVICE_KEY_HASH", DeviceAccess.HashKey(deviceKey) },
                        { "@TOUCH", false },
                    });

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var row = ds.Tables[0].Rows[0];
                        deviceStatus = Convert.ToInt32(row["IS_VALID"]) == 1
                            ? "APPROVED"
                            // APPROVED but owner deactivated -> treat as revoked for the phone
                            : (row["DEVICE_STATUS"]?.ToString() == "APPROVED" ? "REVOKED" : row["DEVICE_STATUS"]?.ToString() ?? "NONE");
                        deviceName = row["DEVICE_NAME"] == DBNull.Value ? null : row["DEVICE_NAME"];
                        expiresAt = row["EXPIRES_AT"] == DBNull.Value ? null : row["EXPIRES_AT"];
                    }
                }

                // Dictionary (not an anonymous object) so the JSON keys stay exactly
                // as written - ASP.NET camel-cases property names (IP_ALLOWED -> iP_ALLOWED).
                return CommonHelper.CreateApiResponse(200, "Success", new Dictionary<string, object?>
                {
                    ["IP_ALLOWED"] = ipAllowed,
                    ["DEVICE_STATUS"] = deviceStatus,   // NONE / PENDING / APPROVED / EXPIRED / REJECTED / REVOKED
                    ["DEVICE_NAME"] = deviceName,
                    ["EXPIRES_AT"] = expiresAt,
                });
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }

        public object RequestAccess(DeviceRequestModel model, string? ip, string? userAgent)
        {
            try
            {
                var username = model.USERNAME?.Trim() ?? "";
                var deviceName = model.DEVICE_NAME?.Trim() ?? "";

                if (username.Length == 0 || string.IsNullOrEmpty(model.PASSWORD) || deviceName.Length == 0)
                {
                    return CommonHelper.CreateApiResponse(400, "Username, password and device name are required.", null);
                }
                if (deviceName.Length > 100)
                {
                    return CommonHelper.CreateApiResponse(400, "Device name must be 100 characters or less.", null);
                }

                var key = DeviceAccess.NewKey();

                var ds = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_DEVICE_REQUEST", new Dictionary<string, object>
                {
                    { "@USERNAME", username },
                    { "@PASSWORD", model.PASSWORD },
                    { "@DEVICE_NAME", deviceName },
                    { "@DEVICE_KEY_HASH", DeviceAccess.HashKey(key) },
                    { "@REQUEST_IP", DeviceAccess.Truncate(ip, 45) },
                    { "@USER_AGENT", DeviceAccess.Truncate(userAgent, 500) },
                });

                var row = ds.Tables[0].Rows[0];
                var code = Convert.ToInt32(row["RESULT_CODE"]);
                var msg = row["RESULT_MSG"]?.ToString() ?? "";

                if (code != 1)
                {
                    return CommonHelper.CreateApiResponse(400, msg, null);
                }

                // The key is returned exactly once; only its hash is stored.
                return CommonHelper.CreateApiResponse(200, msg, new Dictionary<string, object> { ["DEVICE_KEY"] = key });
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }

        public object ListDevices(int adminUserSrno, string? deviceStatus)
        {
            try
            {
                var ds = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_USER_DEVICES", new Dictionary<string, object>
                {
                    { "@USER_SRNO", adminUserSrno },
                    { "@DEVICE_STATUS", string.IsNullOrWhiteSpace(deviceStatus) ? null! : deviceStatus.Trim().ToUpperInvariant() },
                });

                var code = Convert.ToInt32(ds.Tables[0].Rows[0]["RESULT_CODE"]);
                if (code != 1)
                {
                    return CommonHelper.CreateApiResponse(403, ds.Tables[0].Rows[0]["RESULT_MSG"]?.ToString() ?? "Not allowed.", null);
                }

                var result = CommonHelper.SerializeDataSet(ds);
                return CommonHelper.CreateApiResponse(200, "Success", new Dictionary<string, object>
                {
                    ["Table"] = ds.Tables.Count > 1 ? result["Table1"] : new List<Dictionary<string, object>>()
                });
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }

        public object DeviceAction(int adminUserSrno, DeviceActionModel model)
        {
            try
            {
                var action = model.ACTION?.Trim().ToUpperInvariant() ?? "";
                if (action is not ("APPROVE" or "RENEW" or "REJECT" or "REVOKE"))
                {
                    return CommonHelper.CreateApiResponse(400, "Unknown action.", null);
                }

                var ds = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_DEVICE_ACTION", new Dictionary<string, object>
                {
                    { "@DEVICE_SRNO", model.DEVICE_SRNO },
                    { "@ACTION", action },
                    { "@USER_SRNO", adminUserSrno },
                });

                var row = ds.Tables[0].Rows[0];
                var code = Convert.ToInt32(row["RESULT_CODE"]);
                var msg = row["RESULT_MSG"]?.ToString() ?? "";

                return code switch
                {
                    1 => CommonHelper.CreateApiResponse(200, msg, null),
                    -1 => CommonHelper.CreateApiResponse(403, msg, null),
                    _ => CommonHelper.CreateApiResponse(400, msg, null),
                };
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }
    }
}
