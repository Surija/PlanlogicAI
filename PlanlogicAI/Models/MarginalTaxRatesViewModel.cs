using System;
using System.Collections.Generic;

namespace PlanlogicAI.Models
{
    public partial class MarginalTaxRatesViewModel
    {
        public int Index { get; set; }
        public int Threshold { get; set; }
        public decimal Rate { get; set; }
    }
}
