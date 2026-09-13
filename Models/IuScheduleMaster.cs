namespace omech.Models
{
    public class IuScheduleMaster : ComParaModel
    {
        public char IU_FLAG { get; set; }
        public int? SCHEDULE_SRNO { get; set; }
        public string ITEM_TYPE { get; set; } // PIPE, COIL, SHEET

        public string? PARTY_NAME { get; set; }
        public int? PARTY_SRNO { get; set; }
        public string? PO_NUMBER { get; set; }
        public DateTime? SCHEDULE_DATE { get; set; }
        public DateTime? ESTIMATED_DELIVERY_DATE { get; set; }
        public int? STATUS_SRNO { get; set; }
        public string? REMARKS { get; set; }

        public List<IuScheduleDt> DETAIL_JSON { get; set; }
    }
}
