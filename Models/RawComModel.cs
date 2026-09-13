namespace omech.Models
{
    public class RawComModel
    {
    }
    public class IuShiftRawMaterial : ComParaModel
    {
        public char IU_FLAG { get; set; } // Insert/Update Flag
        public int? MATERIAL_SRNO { get; set; } // Material Serial Number
        public int? SLITTING_SRNO { get; set; } // Slitting Serial Number
        public int FROM_LOCATION { get; set; } // From Locaction
        public int TO_LOCATION { get; set; } // To Location
        public DateTime SHIFT_DATE { get; set; } // Shift Date
        public string? SHIFT_REASON { get; set; } // SHIFT_REASON
        public int? SHIFTING_SRNO { get; set; } // Pk of record for edit
    }
}
