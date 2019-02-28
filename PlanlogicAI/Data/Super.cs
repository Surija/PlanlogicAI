using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class Super
    {
        public int SuperId { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int Value { get; set; }
        public int TaxFreeComponent { get; set; }
        public int TaxableComponent { get; set; }
        public decimal Growth { get; set; }
        public decimal Income { get; set; }
        public int InsuranceCost { get; set; }
        public string StartDateType { get; set; }
        public int? StartDate { get; set; }
        public string EndDateType { get; set; }
        public int? EndDate { get; set; }
        public int? SuperSalary { get; set; }
        public string IncreaseToLimit { get; set; }
        public string Sgrate { get; set; }
        public decimal ProductFees { get; set; }
        public decimal Franked { get; set; }
    }
}
