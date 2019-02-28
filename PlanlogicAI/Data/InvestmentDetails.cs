using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class InvestmentDetails
    {
        public int RecId { get; set; }
        public int InvestmentId { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public string FromDateType { get; set; }
        public int? FromDate { get; set; }
        public string ToDateType { get; set; }
        public int? ToDate { get; set; }
    }
}
