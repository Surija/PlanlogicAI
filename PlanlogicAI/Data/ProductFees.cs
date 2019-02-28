using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class ProductFees
    {
        public int FeeId { get; set; }
        public string HeaderType { get; set; }
        public int HeaderId { get; set; }
        public int ProductId { get; set; }
        public string CostType { get; set; }
        public string FeeName { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
    }
}
