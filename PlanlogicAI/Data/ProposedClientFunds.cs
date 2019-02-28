using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class ProposedClientFunds
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string Apircode { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
    }
}
