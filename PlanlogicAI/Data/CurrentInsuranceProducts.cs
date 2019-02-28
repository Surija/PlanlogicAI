using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class CurrentInsuranceProducts
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }
        public string Owner { get; set; }
    }
}
