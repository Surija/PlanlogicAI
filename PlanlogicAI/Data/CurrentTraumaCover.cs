using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class CurrentTraumaCover
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string PolicyOwner { get; set; }
        public decimal BenefitAmount { get; set; }
        public string PremiumType { get; set; }
        public string StandaloneOrLinked { get; set; }
        public string WithinSuper { get; set; }
        public int Reinstatement { get; set; }
        public int DoubleTrauma { get; set; }
        public int ChildTrauma { get; set; }
        public int WaiverOfPremium { get; set; }
    }
}
