using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
   

    public partial class CurrentClientProductsViewModel
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Owner { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Value { get; set; }
        public decimal UnutilizedValue { get; set; }
        public decimal Percentage { get; set; }
        public int Status { get; set; }
        public int isEqual { get; set; }
        public int CurrentId { get; set; }
        public string PlatformName { get; set; }
        public int PlatformId { get; set; }
        public int RopCurrentId { get; set; }
        
    }

    public partial class CurrentClientFundsViewModel
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string Apircode { get; set; }
        public string FundName { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
    }

    public partial class ProposedClientFundsViewModel
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string Apircode { get; set; }
        public string FundName { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public string FeeLabel1 { get; set; }
        public string FeeLabel2 { get; set; }
        public string FeeLabel3 { get; set; }
        public string FeeLabel4 { get; set; }
    }

    public partial class ProposedClientProductsViewModel
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Owner { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public int Status { get; set; }
        public int isEqual { get; set; }
        public int CurrentId { get; set; }
        public int RopCurrentId { get; set; }
        public string PlatformName { get; set; }
        public int PlatformId { get; set; }
    }

    public partial class ProductReplacementViewModel
    {
        public int RecId { get; set; }
        public int CurrentId { get; set; }
        public int ProposedId { get; set; }
    }

}
