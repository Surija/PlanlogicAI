using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanlogicAI.Models
{
    public class AssetTypesViewModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Growth { get; set; }
        public decimal Income { get; set; }
        public decimal Franking { get; set; }
        public decimal ProductFees { get; set; }
    }
}
