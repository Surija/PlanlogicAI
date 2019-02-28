using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class AssetTypes
    {
        public int RecId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Growth { get; set; }
        public decimal Income { get; set; }
        public decimal Franking { get; set; }
        public decimal ProductFees { get; set; }
    }
}
