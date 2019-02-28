using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class CurrentFeeDetails
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
        public string SpecialNotes { get; set; }
    }
}
