using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class PensionViewModel
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

    public partial class PensionDetailsViewModel
    {

        public int RecId { get; set; }
        public int PensionId { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public string FromDateType { get; set; }
        public int? FromDate { get; set; }
        public string ToDateType { get; set; }
        public int? ToDate { get; set; }

    }

    public class PensionAsset
    {
        public PensionViewModel pension { get; set; }
        public PensionDetailsViewModel[] pensionDrawdown { get; set; }
    }
}
