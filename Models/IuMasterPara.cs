namespace omech.Models
{
    public class IuMasterPara : ComParaModel
    {
        public string IU_FLAG { get; set; }

        public string M_NAME { get; set; } // MASTER FIELLD NAME
        public string? UOM { get; set; }
        public string? ADDRESS { get; set; }
        public string? GST_NO { get; set; }
        public string? STATE { get; set; }
        public string? EMAIL { get; set; }
        public string? PHONE { get; set; }
        public string? CONTACT_PERSON { get; set; }

        public int? PK_SRNO { get; set; }
    }

    public class DelMasterPara : ComParaModel
    {
        public int PK_SRNO { get; set; }
    }
}
