using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class InvestmentFund
    {
        public string Apircode { get; set; }
        public string Mid { get; set; }
        public string FundName { get; set; }
        public decimal Amount { get; set; }
        public decimal BuySpread { get; set; }
        public decimal Icr { get; set; }
        public decimal DomesticEquity { get; set; }
        public decimal InternationalEquity { get; set; }
        public decimal DomesticProperty { get; set; }
        public decimal InternationalProperty { get; set; }
        public decimal GrowthAlternatives { get; set; }
        public decimal DefensiveAlternatives { get; set; }
        public decimal DomesticFixedInterest { get; set; }
        public decimal InternationalFixedInterest { get; set; }
        public decimal DomesticCash { get; set; }
        public decimal InternationalCash { get; set; }
        public decimal OtherGrowth { get; set; }
        public string IsSingle { get; set; }
        public string InvestorProfile { get; set; }
        public string SubType { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
