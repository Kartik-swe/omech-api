namespace omech.Models
{
    public class IuScheduleDt
    {

        public int? SCHEDULE_DT_SRNO { get; set; }
        //public int? SCHEDULE_SRNO { get; set; }
        // Common
        public int? OD_SRNO { get; set; }
        public int? THICKNESS_SRNO { get; set; }
        public int? GRADE_SRNO { get; set; }

        // PIPE
        public decimal? LENGTH { get; set; }
        public int? QUANTITY { get; set; }

        // COIL
        public decimal? WIDTH { get; set; }

        // SHEET
        public decimal? BREADTH { get; set; }

        // COIL & SHEET
        public decimal? WEIGHT_KG { get; set; }

        public int? STATUS_SRNO { get; set; }

    }
}
