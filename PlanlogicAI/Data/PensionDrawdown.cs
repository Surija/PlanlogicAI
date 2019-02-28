using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class PensionDrawdown
    {
        public int Index { get; set; }
        public int Age { get; set; }
        public decimal MinRate { get; set; }
    }
}
