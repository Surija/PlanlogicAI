using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{

    public partial class ProposedInsuranceViewModel
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }
        public string Owner { get; set; }
        public int ReplacementId { get; set; }
        public List<ProposedFeeDetailsViewModel> FeeDetails { get; set; }
        public List<ProposedLifeCoverViewModel> LifeCover { get; set; }
        public List<ProposedTpdCoverViewModel> TpdCover { get; set; }
        public List<ProposedTraumaCoverViewModel> TraumaCover { get; set; }
        public List<ProposedIncomeCoverViewModel> IncomeCover { get; set; }
        public List<InsuranceReplacementViewModel> Replacement { get; set; }      
        public CostOfAdviceViewModel Implementation { get; set; }
        public CostOfAdviceViewModel Ongoing { get; set; }
    }

    public partial class ProposedInsuranceProductsViewModel
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }
        public string Owner { get; set; }
        public int ReplacementId { get; set; }
    }
}
