namespace omech.Models
{
    public class Sp_Iu_Raw_Slit_Model
    {
        public Char IU_FLAG { get; set; }
        public int MATERIAL_SRNO { get; set; }
        public int VENDOR_SRNO { get; set; }
        public int? SLITTING_SRNO_FK { get; set; }
        public int SLITTING_LEVEL { get; set; }
        public DateTime SLITTING_DATE { get; set; }
        public string DC_NO { get; set; }
        public int SHIFT_TO { get; set; }
        public Decimal SCRAP { get; set; }
        public int USER_SRNO { get; set; }
        public List<SlitDetail> SlitDetails { get; set; }
    }

    public class SlitDetail
    {
        public decimal SLITTING_WIDTH { get; set; }
        public int NOS { get; set; }
        public decimal SLITTING_WEIGHT { get; set; }
    }
}
