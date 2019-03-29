using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanlogicAI.Models
{

    [Serializable]
    public class Projection
    {
 
        public DateTime Date { get; set; }
        public List<Lifestyle> Lifestyles { get; set; }
        public List<InvestmentValue> Investments { get; set; }
        public List<PropertyValue> Properties { get; set; }
        public List<Common> LATotals { get; set; }
       
        public List<LiabilityValue> Liabilities { get; set; }
        public List<Income> Inflow { get; set; }
        public List<Income> Outflow { get; set; }
        public List<Income> Income { get; set; }
        public List<NetCashflow> NetCashflows { get; set; }
        public List<Common> FaTotals { get; set; }
        public List<Common> PensionTotal { get; set; }
        public List<Common> PropertiesTotal { get; set; }
        public List<Common> LBTotal { get; set; }
        public List<PensionValue> Pensions { get; set; }
        public List<SuperValue> Supers { get; set; }
        public List<Common> SuperTotal { get; set; }


        public List<Income> ClientDeductions { get; set; }
        public List<Income> PartnerDeductions { get; set; }
        public List<Common> ClientTaxableIncome { get; set; }
        public List<Common> PartnerTaxableIncome { get; set; }
        public List<Common> ClientLossAdjustment { get; set; }
        public List<Common> PartnerLossAdjustment { get; set; }
        public List<Common> GrossTax { get; set; }
        public List<Common> ClientNrTaxOffset { get; set; }
        public List<Common> PartnerNrTaxOffsets { get; set; }
        public List<Common> ClientRTaxOffset { get; set; }
        public List<Common> PartnerRTaxOffset { get; set; }
        public List<Common> ClientMedicareLevy { get; set; }
        public List<Common> PartnerMedicareLevy { get; set; }
        public List<Common> NetPayable { get; set; }
        public List<Common> TotalPayable { get; set; }
        public List<Common> NetAsset { get; set; }
    }

    [Serializable]
    public partial class Lifestyle
    {
        public string Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public double Value { get; set; }
        public int Increment { get; set; }
        public double PurchaseOfAssetValue { get; set; }
        public double SaleOfAssetValue { get; set; }
    }

    [Serializable]
    public partial class InvestmentValue
    {
        public int Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double BegValues { get; set; }
        public double GrowthValues { get; set; }
        public double IncomeValues { get; set; }
        public double IncomePaidOutValues { get; set; }
        public double FrankingCreditsValues { get; set; }
        public double EarningsValues { get; set; }
        public double PurchaseOfAssetValues { get; set; }
        public double RegularContributionsValues { get; set; }
        public double ContributionsValues { get; set; }
        public double SaleOfAssetValues { get; set; }
        public double RegularWithdrawalsValues { get; set; }
        public double WithdrawalsValues { get; set; }
        public double EndingValues { get; set; }
        public double EndingValuesPv { get; set; }
        public double RealCgValues { get; set; }
        public double UnrealCgValues { get; set; }
        public double CashFlowValues { get; set; }
        public double UnfundedValues { get; set; }
    }

    [Serializable]
    public partial class PropertyValue
    {
        public int Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string StartDateType { get; set; }
        public double BegValues { get; set; }
        public double PropertyPurchaseValues { get; set; }
        public double PropertySaleValues { get; set; }
        public double CapitalGrowthValues { get; set; }
        public double EndingValues { get; set; }
        public double EndingValuesPv { get; set; }
        public double RentValues { get; set; }
        public double ExpensesValues { get; set; }
        public double RealCgValues { get; set; }
        public double UnrealCgValues { get; set; }
        public double PropPurchase { get; set; }
    }

    [Serializable]
    public partial class NetCashflow
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public double Values { get; set; }
        public double AssetAllocationValues { get; set; }
        public double UnfundedNetCfValues { get; set; }
    }

    [Serializable]
    public partial class Common
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
     
    }

    [Serializable]
    public partial class Income
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        public int Increment { get; set; }
    }

    [Serializable]
    public partial class Inflow
    {
        public string Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }
        public double Values { get; set; }
        public long Increment { get; set; }
        public long PurchaseOfAssetValues { get; set; }
        public long SaleOfAssetValues { get; set; }
    }

    [Serializable]
    public partial class LiabilityValue
    {
        public int Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double Deductibility { get; set; }
        public double BegValues { get; set; }
        public double AccruedInterestValues { get; set; }
        public double RepmtValues { get; set; }
        public double EndingValues { get; set; }
    }


    //public partial class NetCashflow
    //{
    //    [JsonProperty("owner")]
    //    public string Owner { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("values")]
    //    public string Values { get; set; }

    //    [JsonProperty("assetAllocationValues")]
    //    public string AssetAllocationValues { get; set; }

    //    [JsonProperty("unfundedNetCFValues")]
    //    public string UnfundedNetCfValues { get; set; }
    //}

    [Serializable]
    public partial class PensionValue
    {
        public int Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double BegValues { get; set; }
        public double TaxableBegValues { get; set; }
        public double TaxFreeBegValues { get; set; }
        public double GrowthValues { get; set; }
        public double IncomeValues { get; set; }
        public double FrankingCreditsValues { get; set; }
        public double PensionIncomeValues { get; set; }
        public double PITaxAssessableValues { get; set; }
        public double PITaxExemptValues { get; set; }
        public double EndingValues { get; set; }
        public double EndingValuesPv { get; set; }
        public double TaxableEndingValues { get; set; }
        public double TaxFreeEndingValues { get; set; }
        public int TaxableProp { get; set; }
        public int TaxfreeProp { get; set; }
    }

    //public partial class PropertyValue
    //{
    //    [JsonProperty("owner")]
    //    public long Owner { get; set; }

    //    [JsonProperty("type")]
    //    public string Type { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("startDateType")]
    //    public string StartDateType { get; set; }

    //    [JsonProperty("BegValues")]
    //    public string BegValues { get; set; }

    //    [JsonProperty("PropertyPurchaseValues")]
    //    public string PropertyPurchaseValues { get; set; }

    //    [JsonProperty("PropertySaleValues")]
    //    public string PropertySaleValues { get; set; }

    //    [JsonProperty("capitalGrowthValues")]
    //    public string CapitalGrowthValues { get; set; }

    //    [JsonProperty("endingValues")]
    //    public string EndingValues { get; set; }

    //    [JsonProperty("endingValuesPV")]
    //    public string EndingValuesPv { get; set; }

    //    [JsonProperty("rentValues")]
    //    public string RentValues { get; set; }

    //    [JsonProperty("expensesValues")]
    //    public string ExpensesValues { get; set; }

    //    [JsonProperty("realCGValues")]
    //    public string RealCgValues { get; set; }

    //    [JsonProperty("unrealCGValues")]
    //    public string UnrealCgValues { get; set; }

    //    [JsonProperty("propPurchase")]
    //    public long PropPurchase { get; set; }
    //}

    [Serializable]
    public partial class SuperValue
    {
        public int Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double BegValues { get; set; }
        public double TaxableBegValues { get; set; }
        public double TaxFreeBegValues { get; set; }
        public double GrowthValues { get; set; }
        public double IncomeValues { get; set; }
        public double FrankingCreditsValues { get; set; }
        public double InsuranceValues { get; set; }
        public double SgContrValues { get; set; }
        public double SsContrValues { get; set; }
        public double PncContrValues { get; set; }
        public double SpouseContrValues { get; set; }
        public double LumpSumValues { get; set; }
        public double LumpSumTaxableValues { get; set; }
        public double TaxPayableValues { get; set; }
        public double EndingValues { get; set; }
        public double EndingValuesPv { get; set; }
        public double TaxableEndingValues { get; set; }
        public double TaxFreeEndingValues { get; set; }
    }

    //public partial struct Id
    //{
    //    public long? Integer;
    //    public string String;

    //    public static implicit operator Id(long Integer) => new Id { Integer = Integer };
    //    public static implicit operator Id(string String) => new Id { String = String };
    //}

}
