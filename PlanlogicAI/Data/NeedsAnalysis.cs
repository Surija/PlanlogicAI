using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class NeedsAnalysis
    {
        public int RecId { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public string Owner { get; set; }
        public string Life { get; set; }
        public string Tpd { get; set; }
        public string Trauma { get; set; }
        public string IncomeProtection { get; set; }
        public int IsDefault { get; set; }
    }
}
