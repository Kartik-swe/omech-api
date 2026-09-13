namespace omech.Models
{
    public class IuDispatchModal : ComParaModel
    {
        public char IU_FLAG { get; set; }       
        public int? DISPATCH_SRNO { get; set; }
        public int SCHEDULE_SRNO { get; set; }
        public int SCHEDULE_DT_SRNO { get; set; }
        public int DISPATCH_QTY { get; set; }
        public DateTime? DISPATCH_DATE { get; set; }
        public string? DC_NO { get; set; }  // Nullable string, optional but recommended to check in controller
    }
}
