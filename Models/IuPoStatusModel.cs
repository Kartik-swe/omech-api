namespace omech.Models
{
    public class IuPoStatusModel : ComParaModel
    {
        public Char IU_FLAG { get; set; }
        public Char STATUS_FLAG { get; set; }
        public int SCHEDULE_SRNO { get; set; }
        public int SCHEDULE_DT_SRNO { get; set; }
    }
}
