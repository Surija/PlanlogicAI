using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class ProposedIncomeCover
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string PolicyOwner { get; set; }
        public decimal MonthlyBenefitAmount { get; set; }
        public string PremiumType { get; set; }
        public int WithinSuper { get; set; }
        public string Definition { get; set; }
        public string WaitingPeriod { get; set; }
        public string BenefitPeriod { get; set; }
        public int ClaimsIndexation { get; set; }
        public int CriticalConditionsCover { get; set; }
        public int AccidentBenefit { get; set; }
    }
}
