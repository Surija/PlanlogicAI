using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class ProductFund
    {
        public int RecId { get; set; }
        public int ProductId { get; set; }
        public string Apircode { get; set; }
        public string FeeLabel1 { get; set; }
        public string FeeLabel2 { get; set; }
        public string FeeLabel3 { get; set; }
    }
}
