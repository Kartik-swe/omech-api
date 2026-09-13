namespace omech.Models
{
    public class IuThresholdMasterModel : ComParaModel
    {
        public int? THRESHOLD_SRNO { get; set; }
        public int GRADE_SRNO { get; set; }
        public int THICKNESS_SRNO { get; set; }
        public int OD_SRNO { get; set; }
        public decimal MIN_THRESHOLD { get; set; }
    }
}
