using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class RopcurrentProducts
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Owner { get; set; }
        public int ProductId { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public int ProposedProduct { get; set; }
    }
}
