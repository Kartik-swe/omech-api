namespace omech.Models
{
    public class IuCompanyMasterModel : ComParaModel
    {
        public string COMPANY_NAME { get; set; }
        public string? ADDRESS_LINE1 { get; set; }
        public string? ADDRESS_LINE2 { get; set; }
        public string? CITY { get; set; }
        public string? STATE { get; set; }
        public string? PINCODE { get; set; }
        public string? PHONE { get; set; }
        public string? MOBILE { get; set; }
        public string? EMAIL { get; set; }
        public string? GST_NO { get; set; }
        public string? LOGO_PATH { get; set; }
        public string? BANK_NAME { get; set; }
        public string? BANK_ACCOUNT_NO { get; set; }
        public string? BANK_IFSC { get; set; }
        public string? BANK_BRANCH { get; set; }
    }
}
