namespace omech.Models
{
    public class IuRawSlitModel : ComParaModel
    {
        public char IU_FLAG { get; set; } // Insert/Update Flag
        public int? MATERIAL_SRNO { get; set; } // Material Serial Number
        public int? SLITTING_SRNO_FK { get; set; } // Foreign Key for Slitting Serial Number
        public int C_LOCATION { get; set; } // Vendor Serial Number
        public int? SLITTING_LEVEL { get; set; } // Slitting Level (e.g., 1, 2, etc.)
        public DateTime SLITTING_DATE { get; set; } // Date of Slitting
        public int? SLITTING_GRADE_SRNO { get; set; } // Material Grade
        public int? SLITTING_THICKNESS_SRNO { get; set; } // Material Thickness
        public decimal SLITTING_WIDTH { get; set; } // Slitted Material Width
        public decimal SLITTING_WEIGHT { get; set; } // Slitted Material Weight
        public string DC_NO { get; set; } // Delivery Challan Number
        public decimal? SLITTING_SCRAP { get; set; } // Delivery Challan Number
        public int? STATUS_SRNO { get; set; } // Delivery Challan Number
        public int? IS_SLITTED { get; set; } // Delivery Challan Number
        public int SLITTING_SRNO { get; set; } // Slitting Serial Number
    }
}
