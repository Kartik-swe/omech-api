﻿namespace omech.Models
{
    public class IuRawMaterialModel : ComParaModel
    {
        public char IU_FLAG { get; set; }
        public int MATERIAL_C_LOCATION { get; set; }
        public string CHALLAN_NO { get; set; }
        public int MATERIAL_GRADE { get; set; }
        public decimal MATERIAL_THICKNESS { get; set; }
        public Char MATERIAL_TYPE { get; set; }
        public decimal MATERIAL_WIDTH { get; set; }
        public decimal MATERIAL_WEIGHT { get; set; }
        public DateTime RECEIVED_DATE { get; set; }
        public int? MATERIAL_STATUS_SRNO { get; set; }

        public int? MATERIAL_SCRAP {  get; set; }
        public decimal? MATERIAL_SCRAP_WEIGHT {  get; set; }
        public string? SUPPLIER {  get; set; }
        public int? NOS{  get; set; }
        public int? MATERIAL_SRNO { get; set; }
    }
}
