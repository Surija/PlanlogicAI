using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class CostOfAdviceViewModel
    {
        public int RecId { get; set; }
        public int HeaderId { get; set; }
        public string CoaType { get; set; }
        public decimal Commission { get; set; }
        public decimal Adviser { get; set; }
        public decimal Practice { get; set; }
        public decimal Riadvice { get; set; }
    }
}
