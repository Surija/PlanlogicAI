using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class ProposedInsuranceProducts
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }
        public string Owner { get; set; }
        public int ReplacementId { get; set; }
    }
}
