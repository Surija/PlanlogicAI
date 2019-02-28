using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class MarginalTaxRates
    {
        public int Index { get; set; }
        public int Threshold { get; set; }
        public decimal Rate { get; set; }
    }
}
