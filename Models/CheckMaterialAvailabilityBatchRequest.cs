using System.Collections.Generic;

namespace omech.Models
{
    public class MaterialAvailabilityCombo
    {
        public int GRADE_SRNO { get; set; }
        public int THICKNESS_SRNO { get; set; }
        public int OD_SRNO { get; set; }
    }

    public class CheckMaterialAvailabilityBatchRequest : ComParaModel
    {
        public List<MaterialAvailabilityCombo> COMBOS { get; set; }
    }
}
