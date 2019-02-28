using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class Pension
    {
        public int PensionId { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public string PensionType { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int Value { get; set; }
        public int TaxFreeComponent { get; set; }
        public int TaxableComponent { get; set; }
        public decimal Growth { get; set; }
        public decimal Income { get; set; }
        public decimal Franked { get; set; }
        public decimal ProductFees { get; set; }
        public int TotalBalance { get; set; }
        public string PensionRebootFromType { get; set; }
        public int? PensionRebootFromDate { get; set; }
        public string EndDateType { get; set; }
        public int? EndDate { get; set; }
    }
}
