namespace omech.Models
{
    public class IuMasterPara : ComParaModel
    {
        public string IU_FLAG { get; set; }

        public string M_NAME { get; set; } // MASTER FIELLD NAME
        public string? UOM { get; set; }

        public int? PK_SRNO { get; set; }
    }

    public class DelMasterPara : ComParaModel
    {
        public int PK_SRNO { get; set; }
    }
}
