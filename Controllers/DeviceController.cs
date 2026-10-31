using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using omech.Helpers;
using omech.Models;
using omech.Services;

namespace omech.Controllers
{
    /// <summary>
    /// Device registration. "status" and "request" are reachable from any IP
    /// (see DeviceAccess.PublicPaths); "list" and "action" need a logged-in
    /// admin and are only reachable from an allowed IP or an approved device.
    /// </summary>
    [ApiController]
    [Route("api/device")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>Is this browser on an allowed IP, and what is its device's state?</summary>
        [HttpGet("status")]
        public IActionResult Status()
        {
            if (!DeviceAccess.TryConsumeRequestQuota(HttpContext, "status", 60, TimeSpan.FromMinutes(5)))
            {
                return Ok(CommonHelper.CreateApiResponse(429, "Too many requests. Please wait a few minutes.", null));
            }

            var ipAllowed = HttpContext.Items[DeviceAccess.IpAllowedItem] as bool? ?? false;
            var response = _deviceService.GetStatus(DeviceAccess.GetKey(HttpContext), ipAllowed);
            return Ok(response);
        }

        /// <summary>Register this phone (needs a valid username + password). Creates a PENDING request.</summary>
        [HttpPost("request")]
        public IActionResult RequestAccess([FromBody] DeviceRequestModel model)
        {
            // limits password guessing through this endpoint
            if (!DeviceAccess.TryConsumeRequestQuota(HttpContext, "request", 10, TimeSpan.FromMinutes(15)))
            {
                return Ok(CommonHelper.CreateApiResponse(429, "Too many attempts. Please wait 15 minutes and try again.", null));
            }

            var ip = HttpContext.Items[DeviceAccess.ClientIpItem] as string;
            var userAgent = Request.Headers.UserAgent.FirstOrDefault();
            var response = _deviceService.RequestAccess(model, ip, userAgent);
            return Ok(response);
        }

        /// <summary>Admin: list devices. DEVICE_STATUS = PENDING / APPROVED / EXPIRED / REJECTED / REVOKED, or empty for all.</summary>
        [Authorize]
        [HttpGet("list")]
        public IActionResult List([FromQuery] string? DEVICE_STATUS)
        {
            var userSrno = CurrentUserSrno();
            if (userSrno == null) return Unauthorized();
            return Ok(_deviceService.ListDevices(userSrno.Value, DEVICE_STATUS));
        }

        /// <summary>Admin: APPROVE / RENEW / REJECT / REVOKE a device.</summary>
        [Authorize]
        [HttpPost("action")]
        public IActionResult DeviceAction([FromBody] DeviceActionModel model)
        {
            var userSrno = CurrentUserSrno();
            if (userSrno == null) return Unauthorized();
            return Ok(_deviceService.DeviceAction(userSrno.Value, model));
        }

        // The acting admin always comes from the signed JWT, never from the request body/query.
        private int? CurrentUserSrno()
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(sub, out var id) ? id : null;
        }
    }
}
