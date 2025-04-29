namespace omech.Models
{
    public class IuPipeShiftLocation_Model : ComParaModel
    {
        public char IU_FLAG { get; set; }
        public string ACTION_FLAG { get; set; }

        public int? PR_SRNO { get; set; }

        public int? WORK_SHIFT_SRNO { get; set; }
        public int? FROM_LOCATION { get; set; }
        public int? TO_LOCATION{ get; set; }
        public string PIPE_NOS { get; set; }
        // for sell
        public string? CUSTOMER_NAME { get; set; }
        public string? INVOICE_NUMBER { get; set; }
        // sell end
        // CUTTING
        public string? LEASRE_MACHINE_NUMBER { get; set; }

        //CUTTING CUTTING END
        public DateTime? TRN_DATE { get; set; }
        public int? TRN_BY { get; set; }
        public string? TRN_REMARK { get; set; }
        public int? PR_INV_SRNO { get; set; }

    }
}
