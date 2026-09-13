namespace omech.Models
{
    public class IuPoItemsRejectModal : ComParaModel
    {
        public char IU_FLAG { get; set; }
        public int SCHEDULE_SRNO { get; set; }
        public int SCHEDULE_DT_SRNO { get; set; }
        public int REJECTED_QTY { get; set; }
        public string? REjECTED_REMARK { get; set; }  // Nullable string, optional but recommended to check in controller
        public int? REJECT_SRNO { get; set; }
    }
}
