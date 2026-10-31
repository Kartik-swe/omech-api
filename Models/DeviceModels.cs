namespace omech.Models
{
    /// <summary>Body of POST api/device/request (from a phone that is not yet registered).</summary>
    public class DeviceRequestModel
    {
        public string USERNAME { get; set; } = "";
        public string PASSWORD { get; set; } = "";
        public string DEVICE_NAME { get; set; } = "";
    }

    /// <summary>Body of POST api/device/action (admin).</summary>
    public class DeviceActionModel
    {
        public int DEVICE_SRNO { get; set; }

        /// <summary>APPROVE, RENEW, REJECT or REVOKE.</summary>
        public string ACTION { get; set; } = "";
    }
}
