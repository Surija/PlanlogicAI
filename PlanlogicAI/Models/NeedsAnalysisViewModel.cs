using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class NeedsAnalysisViewModel
    {
        public int RecId { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public string Owner { get; set; }
        public string Life { get; set; }
        public string Tpd { get; set; }
        public string Trauma { get; set; } 
        public string IncomeProtection { get; set; }
        public int IsDefault { get; set; }
    }

    public class TotalNeedsAnalysisData
    {
        public int Life { get; set; }
        public int Tpd { get; set; }
        public int Trauma { get; set; }
        public int IncomeProtection { get; set; }
    }


    public class NeedsAnalysisData
    {
        public List<NeedsAnalysisViewModel> clientNeedsAnalysis { get; set; }
        public List<NeedsAnalysisViewModel> partnerNeedsAnalysis { get; set; }
        public int clientId { get; set; }
        public int isMarried { get; set; }
    }

}
