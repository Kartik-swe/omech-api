namespace omech.Models
{
    public class IuPipesInvPrModel : ComParaModel
    {
            public char IU_FLAG { get; set; }

            public string FLAG { get; set; } // MASTER FIELLD NAME
            public int PR_INV_SRNO { get; set; }
            public int PIPE_NOS { get; set; }
    }
}
