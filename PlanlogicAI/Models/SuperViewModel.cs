using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class SuperViewModel
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
        public decimal Franked { get; set; }
        public int InsuranceCost { get; set; }
        public string StartDateType { get; set; }
        public int? StartDate { get; set; }
        public string EndDateType { get; set; }
        public int? EndDate { get; set; }
        public int? SuperSalary { get; set; }
        public string IncreaseToLimit { get; set; }
        public string Sgrate { get; set; }
        public decimal ProductFees { get; set; }
    }

    public partial class SuperDetailsViewModel
    {
        public int RecId { get; set; }
        public int SuperId { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public string IncreaseToLimit { get; set; }
        public int Amount { get; set; }
        public string FromDateType { get; set; }
        public int? FromDate { get; set; }
        public string ToDateType { get; set; }
        public int? ToDate { get; set; }

    }

    public class SuperAsset
    {
        public SuperViewModel super { get; set; }
        public SuperDetailsViewModel[] superDetails { get; set; }
    }
}
