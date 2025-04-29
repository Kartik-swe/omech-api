namespace omech.Models
{
    public class IuStatusLogModel : ComParaModel
    {
        // Numeric fields for Material, Slitting, Pre Log Status, etc.
        public decimal MATERIAL_SRNO { get; set; }
        public decimal? SLITTING_SRNO { get; set; }
        public decimal? PRE_LOG_STATUS_SRNO { get; set; }

        // Description and Remarks fields as NVARCHAR
        public string? DESCRIPTION { get; set; }
        public string? REMARKS { get; set; }

        // Status Change Date, this is nullable
        public DateTime? STATUS_CHANGE_DATE { get; set; }

        
        // Output parameter for Log Status Serial Number
        public int? LOG_STATUS_SRNO { get; set; }
    }
}
