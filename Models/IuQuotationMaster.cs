using System.Collections.Generic;

namespace omech.Models
{
    public class IuQuotationMaster : ComParaModel
    {
        public string IU_FLAG { get; set; }
        public string QUOTATION_NO { get; set; }
        public System.DateTime QUOTATION_DATE { get; set; }
        public int? PARTY_SRNO { get; set; }
        public string? PARTY_NAME { get; set; }
        public string? PARTY_ADDRESS { get; set; }
        public string? PARTY_GST_NO { get; set; }
        public string? ENQUIRY_NO { get; set; }
        public System.DateTime? ENQUIRY_DATE { get; set; }
        public string RATE_BASIS_MODE { get; set; } = "K"; // 'K' | 'M' | 'BOTH'
        public decimal? GST_PERCENT { get; set; }
        public string? PAYMENT_TERMS { get; set; }
        public string? RATE_TERMS { get; set; }
        public string? DELIVERY_TERMS { get; set; }
        public string? VALIDITY { get; set; }
        public string? STATUS { get; set; }
        public List<IuQuotationDt> DETAIL_JSON { get; set; }
        public int? QUOTATION_SRNO { get; set; }
    }

    public class IuQuotationDt
    {
        public int? QUOTATION_DT_SRNO { get; set; }
        public int? SR_NO { get; set; }
        public bool? IS_NOTE { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? OD_SRNO { get; set; }
        public int? GRADE_SRNO { get; set; }
        public int? THICKNESS_SRNO { get; set; }
        public decimal? LENGTH { get; set; }
        public decimal? QTY { get; set; }
        public decimal? WEIGHT { get; set; }
        public decimal? RATE_PER_KG { get; set; }
        public decimal? RATE_PER_METER { get; set; }
    }
}
