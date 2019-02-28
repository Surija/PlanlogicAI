using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{

    public partial class CurrentInsuranceViewModel
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }
        public string Owner { get; set; }
        public List<CurrentFeeDetailsViewModel> FeeDetails { get; set; }
        public List<CurrentLifeCoverViewModel> LifeCover { get; set; }
        public List<CurrentTpdCoverViewModel> TpdCover { get; set; }
        public List<CurrentTraumaCoverViewModel> TraumaCover { get; set; }
        public List<CurrentIncomeCoverViewModel> IncomeCover { get; set; }
    }



        public partial class CurrentInsuranceProductsViewModel
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Provider { get; set; }
        public string Owner { get; set; }

    }
}
