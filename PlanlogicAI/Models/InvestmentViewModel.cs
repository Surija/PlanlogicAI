using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class InvestmentFundTest
    {

        public string Name { get; set; }
        public int Age { get; set; }
     

    }

    public partial class InvestmentViewModel
    {

        public int InvestmentId { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int Value { get; set; }
        public decimal Growth { get; set; }
        public decimal Income { get; set; }
        public decimal Franked { get; set; }
        public int CostBase { get; set; }
        public string Reinvest { get; set; }
        public string Centrelink { get; set; }
        public string StartDateType { get; set; }
        public int? StartDate { get; set; }
        public string EndDateType { get; set; }
        public int? EndDate { get; set; }
        public decimal ProductFees { get; set; }


    }

    public class InvestmentCW
    {
        public InvestmentViewModel investmentDetails { get; set; }
        public InvestmentDetailsViewModel[] cw { get; set; }
    }
}
