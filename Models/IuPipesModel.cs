namespace omech.Models
{
    public class IuPipesModel: ComParaModel
    {
        public char IU_FLAG { get; set; }
        public int? MATERIAL_SRNO { get; set; }
        public int? SLITTING_SRNO { get; set; }
        public int MACHINE_SRNO { get; set; }
        public int OD_SRNO { get; set; }
        public int GRADE_SRNO { get; set; }
        public int THICKNESS_SRNO { get; set; }
        public int? WORK_SHIFT_SRNO { get; set; }
        public int C_LOCATION { get; set; }
        public bool? IS_COIL_COMPLETED { get; set; }
        public string P_LENGTH { get; set; }
        public string PIPE_NOS { get; set; }
        public string? PG_SCRAP_WT { get; set; }
        public decimal? P_WEIGHT { get; set; }
        public string? REMARKS { get; set; }
        public DateTime? TRN_DATE { get; set; }
        public int? TRN_BY { get; set; }
        public string? TRN_REMARK { get; set; }
        public int? PG_SRNO { get; set; }

    }
}
