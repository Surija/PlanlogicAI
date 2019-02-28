using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class Liability
    {
        public int LiabilityId { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Deductibility { get; set; }
        public string Owner { get; set; }
        public int Principal { get; set; }
        public string RepaymentType { get; set; }
        public int Repayment { get; set; }
        public decimal InterestRate { get; set; }
        public int Term { get; set; }
        public string CommenceOnDateType { get; set; }
        public int? CommenceOnDate { get; set; }
        public string RepaymentDateType { get; set; }
        public int? RepaymentDate { get; set; }
        public string AssociatedAsset { get; set; }
        public int CreditLimit { get; set; }
    }
}
