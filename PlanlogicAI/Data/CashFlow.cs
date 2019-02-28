using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class CashFlow
    {
        public int CflowId { get; set; }
        public int ClientId { get; set; }
        public string Cftype { get; set; }
        public string Cfname { get; set; }
        public string Owner { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public decimal? Indexation { get; set; }
        public string StartDateType { get; set; }
        public int? StartDate { get; set; }
        public string EndDateType { get; set; }
        public int? EndDate { get; set; }
    }
}
