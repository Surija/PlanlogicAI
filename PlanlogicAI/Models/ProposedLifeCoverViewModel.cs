using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class ProposedLifeCoverViewModel
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
      
        public string PolicyOwner { get; set; }
        public decimal BenefitAmount { get; set; }
        public string PremiumType { get; set; }
        public int WithinSuper { get; set; }
        public int FutureInsurability { get; set; }
        public int TerminalIllness { get; set; }
    }
}
