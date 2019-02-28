using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class ProposedTpdCoverViewModel
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
      
        public string PolicyOwner { get; set; }
        public decimal BenefitAmount { get; set; }
        public string PremiumType { get; set; }
        public string StandaloneOrLinked { get; set; }
        public string Definition { get; set; }
        public string WithinSuper { get; set; }
        public string DisabilityTerm { get; set; }
        public int DoubleTpd { get; set; }
        public int WaiverOfPremium { get; set; }
        public int FutureInsurability { get; set; }
    }
}
