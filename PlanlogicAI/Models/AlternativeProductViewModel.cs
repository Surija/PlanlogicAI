using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
   
       public partial class AlternativeClientFundsViewModel
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string Apircode { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public string FundName { get; set; }
        public string FeeLabel1 { get; set; }
        public string FeeLabel2 { get; set; }
        public string FeeLabel3 { get; set; }
    }

    public partial class AlternativeClientProductsViewModel
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Owner { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public int ProposedProduct { get; set; }
    }


}
