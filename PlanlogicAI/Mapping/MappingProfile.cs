using AutoMapper;
using PlanlogicAI.Data;
using PlanlogicAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanlogicAI.Mapping 
{
    public class MappingProfile : Profile
    {
        public MappingProfile() : this("MyProfile") { }
        protected MappingProfile(string profileName)
        : base(profileName)
        {
            //CreateMap<Advisor, AdvisorViewModel>();
            //CreateMap<AdvisorViewModel, Advisor>();
            CreateMap<Client, ClientViewModel>();
            CreateMap<ClientViewModel, Client>()
                .ForMember(c => c.ClientId, opt => opt.Ignore())
                .ForMember(c => c.AdvisorId, opt => opt.Ignore());

            CreateMap<BasicDetails, BasicDetailsViewModel>();
            CreateMap<BasicDetailsViewModel, BasicDetails>()
                .ForMember(c => c.ClientId, opt => opt.Ignore());


            CreateMap<CashFlow, CashFlowViewModel>();
            CreateMap<CashFlowViewModel, CashFlow>()
                .ForMember(c => c.CflowId, opt => opt.Ignore());

            CreateMap<LifestyleAsset, LifestyleAssetViewModel>();
            CreateMap<LifestyleAssetViewModel, LifestyleAsset>()
                .ForMember(c => c.LassetId, opt => opt.Ignore());

            CreateMap<Property, PropertyViewModel>();
            CreateMap<PropertyViewModel, Property>()
                .ForMember(c => c.PropertyId, opt => opt.Ignore());

            CreateMap<Investment, InvestmentViewModel>();
            CreateMap<InvestmentViewModel, Investment>()
                .ForMember(c => c.InvestmentId, opt => opt.Ignore());

            CreateMap<InvestmentDetails, InvestmentDetailsViewModel>();
            CreateMap<InvestmentDetailsViewModel, InvestmentDetails>()
                .ForMember(c => c.RecId, opt => opt.Ignore());

            CreateMap<Super, SuperViewModel>();
            CreateMap<SuperViewModel, Super>()
                .ForMember(c => c.SuperId, opt => opt.Ignore());

            CreateMap<SuperDetails, SuperDetailsViewModel>();
            CreateMap<SuperDetailsViewModel, SuperDetails>()
                .ForMember(s => s.RecId, opt => opt.Ignore());


            CreateMap<Pension, PensionViewModel>();
            CreateMap<PensionViewModel, Pension>()
                .ForMember(c => c.PensionId, opt => opt.Ignore());

            CreateMap<PensionDetails, PensionDetailsViewModel>();
            CreateMap<PensionDetailsViewModel, PensionDetails>()
                .ForMember(s => s.RecId, opt => opt.Ignore());


            CreateMap<Liability, LiabilityViewModel>();
            CreateMap<LiabilityViewModel, Liability>()
                .ForMember(c => c.LiabilityId, opt => opt.Ignore());

            CreateMap<LiabilityDetails, LiabilityDetailsViewModel>();
            CreateMap<LiabilityDetailsViewModel, LiabilityDetails>()
                .ForMember(c => c.RecId, opt => opt.Ignore());


            CreateMap<General, GeneralViewModel>();
            CreateMap<GeneralViewModel, General>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<AssetTypes, AssetTypesViewModel>();
            CreateMap<AssetTypesViewModel, AssetTypes>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<MarginalTaxRates, MarginalTaxRatesViewModel>();
            CreateMap<MarginalTaxRatesViewModel, MarginalTaxRates>();

            CreateMap<PensionDrawdown, PensionDrawdownViewModel>();
            CreateMap<PensionDrawdownViewModel, PensionDrawdown>();

            CreateMap<SuperAssumptions, SuperAssumptionsViewModel>();
            CreateMap<SuperAssumptionsViewModel, SuperAssumptions>();

            CreateMap<Sgcrate, SgcrateViewModel>();
            CreateMap<SgcrateViewModel, Sgcrate>();

            CreateMap<PreservationAge, PreservationAgeViewModel>();
            CreateMap<PreservationAgeViewModel, PreservationAge>();

            CreateMap<MinimumPensionDrawdown, MinimumPensionDrawdownViewModel>();
            CreateMap<MinimumPensionDrawdownViewModel, MinimumPensionDrawdown>();

            CreateMap<QualifyingAge, QualifyingAgeViewModel>();
            CreateMap<QualifyingAgeViewModel, QualifyingAge>();

            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>()
                .ForMember(g => g.ProductId, opt => opt.Ignore());

            CreateMap<ProductFees, ProductFeesViewModel>();
            CreateMap<ProductFeesViewModel, ProductFees>()
                .ForMember(g => g.FeeId, opt => opt.Ignore());

            CreateMap<ProductFund, ProductFundViewModel>();
            CreateMap<ProductFundViewModel, ProductFund>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<InvestmentFund, InvestmentFundViewModel>();
            CreateMap<InvestmentFundViewModel, InvestmentFund>();

            CreateMap<CurrentClientProducts, CurrentClientProductsViewModel>();
            CreateMap<CurrentClientProductsViewModel, CurrentClientProducts>()
                .ForMember(g => g.RecId, opt => opt.Ignore());
            

            CreateMap<CurrentClientFunds, CurrentClientFundsViewModel>();
            CreateMap<CurrentClientFundsViewModel, CurrentClientFunds>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<Platform, PlatformViewModel>();
            CreateMap<PlatformViewModel, Platform>()
                .ForMember(g => g.PlatformId, opt => opt.Ignore());


            CreateMap<ProposedClientProducts, ProposedClientProductsViewModel>()
                  .ForMember(c => c.RopCurrentId, opt => opt.Ignore());
            CreateMap<ProposedClientProductsViewModel, ProposedClientProducts>();
               

            CreateMap<RopcurrentProducts, ProposedClientProductsViewModel>()
           .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProposedClientFunds, ProposedClientFundsViewModel>();
            CreateMap<ProposedClientFundsViewModel, ProposedClientFunds>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProductReplacement, ProductReplacementViewModel>();
            CreateMap<ProductReplacementViewModel, ProductReplacement>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<AlternativeClientProducts, AlternativeClientProductsViewModel>();
            CreateMap<AlternativeClientProductsViewModel, AlternativeClientProducts>();

            CreateMap<AlternativeClientFunds, AlternativeClientFundsViewModel>();
            CreateMap<AlternativeClientFundsViewModel, AlternativeClientFunds>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<RopcurrentFunds, RopcurrentFundsViewModel>();
            CreateMap<RopcurrentFundsViewModel, RopcurrentFunds>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<RopcurrentProducts, RopcurrentProductsViewModel>();
            CreateMap<RopcurrentProductsViewModel, RopcurrentProducts>();

            CreateMap<RiskProfile, RiskProfileViewModel>();
            CreateMap<RiskProfileViewModel, RiskProfile>();

            CreateMap<NeedsAnalysis, NeedsAnalysisViewModel>();
            CreateMap<NeedsAnalysisViewModel, NeedsAnalysis>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<CurrentInsuranceProducts, CurrentInsuranceProductsViewModel>();
            CreateMap<CurrentInsuranceProductsViewModel, CurrentInsuranceProducts>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<CurrentFeeDetails, CurrentFeeDetailsViewModel>();
            CreateMap<CurrentFeeDetailsViewModel, CurrentFeeDetails>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<CurrentLifeCover, CurrentLifeCoverViewModel>();
            CreateMap<CurrentLifeCoverViewModel, CurrentLifeCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<CurrentTpdCover, CurrentTpdCoverViewModel>();
            CreateMap<CurrentTpdCoverViewModel, CurrentTpdCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<CurrentTraumaCover, CurrentTraumaCoverViewModel>();
            CreateMap<CurrentTraumaCoverViewModel, CurrentTraumaCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<CurrentIncomeCover, CurrentIncomeCoverViewModel>();
            CreateMap<CurrentIncomeCoverViewModel, CurrentIncomeCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProposedInsuranceProducts, ProposedInsuranceProductsViewModel>();
            CreateMap<ProposedInsuranceProductsViewModel, ProposedInsuranceProducts>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProposedFeeDetails, ProposedFeeDetailsViewModel>();
            CreateMap<ProposedFeeDetailsViewModel, ProposedFeeDetails>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProposedLifeCover, ProposedLifeCoverViewModel>();
            CreateMap<ProposedLifeCoverViewModel, ProposedLifeCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProposedTpdCover, ProposedTpdCoverViewModel>();
            CreateMap<ProposedTpdCoverViewModel, ProposedTpdCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProposedTraumaCover, ProposedTraumaCoverViewModel>();
            CreateMap<ProposedTraumaCoverViewModel, ProposedTraumaCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<ProposedIncomeCover, ProposedIncomeCoverViewModel>();
            CreateMap<ProposedIncomeCoverViewModel, ProposedIncomeCover>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<InsuranceReplacement, InsuranceReplacementViewModel>();
            CreateMap<InsuranceReplacementViewModel, InsuranceReplacement>()
                .ForMember(g => g.RecId, opt => opt.Ignore());

            CreateMap<CostOfAdvice, CostOfAdviceViewModel>();
            CreateMap<CostOfAdviceViewModel, CostOfAdvice>()
                .ForMember(g => g.RecId, opt => opt.Ignore());


            CreateMap<CurrentFeeDetailsViewModel, ProposedFeeDetailsViewModel>()
              .ForMember(g => g.RecId, opt => opt.Ignore());
          
        }
    }
}
