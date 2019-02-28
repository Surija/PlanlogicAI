using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class PlatformViewModel
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string SubType { get; set; }
        public string PlatformType { get; set; }
    }

    public partial class ProductViewModel
    {

        public int ProductId { get; set; }
        public int PlatformId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string SubType { get; set; }
    }

    public partial class ProductFeesViewModel
    {
        public int FeeId { get; set; }
        public string HeaderType { get; set; }
        public int HeaderId { get; set; }
        public int ProductId { get; set; }
        public string CostType { get; set; }
        public string FeeName { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
    }

    public partial class PlatformDetailsViewModel
    {
        public int ProductId { get; set; }
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public string ProductType { get; set; }
        public string SubType { get; set; }
        public string PlatformType { get; set; }
    }

    public class ProductFundViewModel
    {
        public int RecId { get; set; }
        public int ProductId { get; set; }
        public string Apircode { get; set; }
        public string FeeLabel1 { get; set; }
        public string FeeLabel2 { get; set; }
        public string FeeLabel3 { get; set; }
    }

    public partial class InvestmentFundViewModel
    {
        public string Apircode { get; set; }
        public string FundName { get; set; }
        public string MId { get; set; }
        public decimal BuySpread { get; set; }
        public decimal Icr { get; set; }
        public decimal DomesticEquity { get; set; }
        public decimal InternationalEquity { get; set; }
        public decimal DomesticProperty { get; set; }
        public decimal InternationalProperty { get; set; }
        public decimal GrowthAlternatives { get; set; }
        public decimal DefensiveAlternatives { get; set; }
        public decimal DomesticFixedInterest { get; set; }
        public decimal InternationalFixedInterest { get; set; }
        public decimal DomesticCash { get; set; }
        public decimal InternationalCash { get; set; }
        public decimal OtherGrowth { get; set; }
        public string IsSingle { get; set; }
        public string InvestorProfile { get; set; }
        public string SubType { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public partial class ProductInvestmentFundViewModel
    {
        public InvestmentFundViewModel fund { get; set; }
        public ProductFundViewModel productFund { get; set; }
    }

        public class NewProduct
    {
        public ProductLinks[] currentProducts { get; set; }
        public ProposedClientProductsViewModel proposedProduct { get; set; }
    }


    public class DocumentDetails
    {
        public string clientRiskProfile { get; set; }
        public string partnerRiskProfile { get; set; }
        public string jointRiskProfile { get; set; }
        public AssetDetails[] clientWeights { get; set; }
        public AssetDetails[] partnerWeights { get; set; }
        public AssetDetails[] jointWeights { get; set; }
        public BasicDetails clientDetails { get; set; }
        public CashFlow[] income { get; set; }
        public CashFlow[] expenses { get; set; }
        public LifestyleAsset[] lifestyleAssets { get; set; }
        public CF[] currentAssests { get; set; }
        public OriginalCF[] currentOriginalAssests { get; set; }
        public PF[] proposedAssets { get; set; }
        public AlternativeFund[] alternativeAssets { get; set; }
        public Liability[] liabilities { get; set; }

        //public AssetDetails[] partnerProposedWeights { get; set; }
        //public AssetDetails[] jointProposedWeights { get; set; }
    }

    public class InsuranceDocumentDetails
    {
        
        public ProposedInsuranceViewModel[] proposedInsurance { get; set; }
        public CurrentInsuranceViewModel[] currentInsurance { get; set; }
        public NeedsAnalysisViewModel[] needsAnalysis { get; set; }
        public BasicDetails clientDetails { get; set; }
    }

    public class AssetDetails
     {
        public string name { get; set; }
        public decimal current { get; set; }
        public decimal proposed { get; set; }
        public decimal target { get; set; }
        public decimal targetmin { get; set; }
        public decimal targetmax { get; set; }
     
    }

    public class ProductLinks
    {
        public int product { get; set; }
        public decimal value { get; set; }
    }

    public class CF
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int proposedId { get; set; }
        public string product { get; set; }
        public string owner { get; set; }
        public decimal value { get; set; }
        public CurrentClientFundsViewModel[] data { get; set; }
        public ProductFees[] feeDetails { get; set; }
        public ProductFeesDisplay[] feeDisplay { get; set; }
    }

    public class OriginalCF
    {
        public int RecId { get; set; }
        public int ClientId { get; set; }
        public string Owner { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Value { get; set; }
        public decimal UnutilizedValue { get; set; }
        public decimal Percentage { get; set; }
        public int Status { get; set; }
        public int isEqual { get; set; }
        public CurrentClientFundsViewModel[] data { get; set; }
      
    }
    public class PF
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int platformId { get; set; }
        public string product { get; set; }
        public string owner { get; set; }
        public decimal value { get; set; }
        public int status { get; set; }
        public ProposedClientFundsViewModel[] data { get; set; }
        public ProductFees[] feeDetails { get; set; }
        public ProductFeesDisplay[] feeDisplay { get; set; }
    }

    public class AlternativeFund
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int proposedId { get; set; }
        public string product { get; set; }
        public string owner { get; set; }
        public decimal value { get; set; }
        public int status { get; set; }
        public AlternativeClientFundsViewModel[] data { get; set; }
        public ProductFees[] feeDetails { get; set; }
        public ProductFeesDisplay[] feeDisplay { get; set; }
    }

    public class AF
    {
               public AlternativeClientFundsViewModel[] data { get; set; }
        public int alternativeProduct { get; set; }
        public AlternativeClientProductsViewModel alternativeProductDetails { get; set; }
    }
    public class ProductFeesDisplay
    {
        public string name { get; set; }
        public string feeType { get; set; }
        public decimal val { get; set; }
        public decimal percentage { get; set; }
       

    }
}
