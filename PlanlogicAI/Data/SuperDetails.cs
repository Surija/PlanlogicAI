using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class SuperDetails
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
}
