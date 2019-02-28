using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class LifestyleAssetViewModel
    {
        public int LassetId { get; set; }
        public int ClientId { get; set; }
        public string LassetType { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int Value { get; set; }
        public decimal Growth { get; set; }
        public int CostBase { get; set; }
        public string StartDateType { get; set; }
        public int? StartDate { get; set; }
        public string EndDateType { get; set; }
        public int? EndDate { get; set; }
    }
}
