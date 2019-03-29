using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PlanlogicAI.Data;
using PlanlogicAI.Models;

namespace PlanlogicAI.Controllers
{
    [Route("api/projections")]
    public class ProjectionsController : Controller
    {

        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        private IEnumerable<MarginalTaxRatesViewModel> marginalTaxRates;

        public ProjectionsController(StrategyOptimizerPrototypeContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public JsonResult GetCashFlows(int id)
        {

            try
            {

                var clientId = id;
                IEnumerable<CashFlowViewModel> cashFlowIncome;
                IEnumerable<CashFlowViewModel> cashFlowExpenditure;
              
                IEnumerable<InvestmentViewModel> investments;
                IEnumerable<GeneralViewModel> generalAssumptions;
                IEnumerable<InvestmentDetailsViewModel> investmentCW;
                IEnumerable<PropertyViewModel> properties;
                IEnumerable<SuperViewModel> supers;
                IEnumerable<SuperDetailsViewModel> superDetails;
                IEnumerable<SgcrateViewModel> sgcRates;
                IEnumerable<SuperAssumptionsViewModel> superAssumptions;
                IEnumerable<LiabilityViewModel> liabilities;
                IEnumerable<LiabilityDetailsViewModel> liabilityDD;
                IEnumerable<PensionViewModel> pensions;
                IEnumerable<PensionDetailsViewModel> pensionDD;
                IEnumerable<PreservationAgeViewModel> preservationAge;
                IEnumerable<MinimumPensionDrawdownViewModel> minimumPensionDrawdown;
                IEnumerable<LifestyleAssetViewModel> lifestyleAssets;
                IEnumerable<AssetTypesViewModel> assetAssumptions;
                IEnumerable<QualifyingAgeViewModel> qualifyingAge;

                //Filtered
                IEnumerable<CashFlowViewModel> cfiClient;
                IEnumerable<CashFlowViewModel> cfiPartner;
                IEnumerable<CashFlowViewModel> cfeClient;
                IEnumerable<CashFlowViewModel> cfePartner;
                IEnumerable<CashFlowViewModel> cfeJoint;

                IEnumerable<CashFlowViewModel> EPRTClient;
                IEnumerable<CashFlowViewModel> EPRTPartner;
                IEnumerable<CashFlowViewModel> EPRTJoint;

                IEnumerable<InvestmentViewModel> investmentClient;
                IEnumerable<InvestmentViewModel> investmentClientOptimized;
                IEnumerable<InvestmentViewModel> investmentPartner;
                IEnumerable<InvestmentViewModel> investmentPartnerOptimized;
                IEnumerable<InvestmentViewModel> investmentJoint;
                IEnumerable<InvestmentViewModel> investmentJointOptimized;

                IEnumerable<PropertyViewModel> propertiesClient;
                IEnumerable<PropertyViewModel> propertiesPartner;
                IEnumerable<PropertyViewModel> propertiesJoint;

                IEnumerable<SuperViewModel> superClient;
                IEnumerable<SuperViewModel> superPartner;

                IEnumerable<LiabilityViewModel> liabilityClient;
                IEnumerable<LiabilityViewModel> liabilityPartner;
                IEnumerable<LiabilityViewModel> liabilityJoint;

                IEnumerable<PensionViewModel> pensionClient;
                IEnumerable<PensionViewModel> pensionPartner;

                IEnumerable<LifestyleAssetViewModel> lifestyleClient;
                IEnumerable<LifestyleAssetViewModel> lifestylePartner;
                IEnumerable<LifestyleAssetViewModel> lifestyleJoint;

                IEnumerable<InvestmentDetailsViewModel> investmentContribution;
                IEnumerable<InvestmentDetailsViewModel> investmentWithdrawal;

                IEnumerable<LiabilityDetailsViewModel> liabilityDrawDown;
                IEnumerable<PensionDetailsViewModel> pensionDrawDown;

                IEnumerable<SuperDetailsViewModel> superSS;
                IEnumerable<SuperDetailsViewModel> superPNC;
                IEnumerable<SuperDetailsViewModel> superSpouse;
                IEnumerable<SuperDetailsViewModel> superLumpSum;

          

                BasicDetails clientDetails;

                JsonResult finalResult = new JsonResult(null);

                if (clientId != 0)
                {

                    clientDetails = context.BasicDetails.Where(x => x.ClientId == clientId).FirstOrDefault();

                    cashFlowIncome = this.mapper.Map<IEnumerable<CashFlowViewModel>>(this.context.CashFlow.AsEnumerable().Where(x => (x.ClientId == clientId) && (x.Cftype == "I")));
                    cashFlowExpenditure = this.mapper.Map<IEnumerable<CashFlowViewModel>>(this.context.CashFlow.AsEnumerable().Where(x => (x.ClientId == clientId) && (x.Cftype == "E")));
                    marginalTaxRates = this.mapper.Map<IEnumerable<MarginalTaxRatesViewModel>>(this.context.MarginalTaxRates.AsEnumerable());
                    investments = this.mapper.Map<IEnumerable<InvestmentViewModel>>(this.context.Investment.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    generalAssumptions = this.mapper.Map<IEnumerable<GeneralViewModel>>(this.context.General.AsEnumerable());
                    investmentCW = this.mapper.Map<IEnumerable<InvestmentDetailsViewModel>>(this.context.InvestmentDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    properties = this.mapper.Map<IEnumerable<PropertyViewModel>>(this.context.Property.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    supers = this.mapper.Map<IEnumerable<SuperViewModel>>(this.context.Super.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    superDetails = this.mapper.Map<IEnumerable<SuperDetailsViewModel>>(this.context.SuperDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    sgcRates = this.mapper.Map<IEnumerable<SgcrateViewModel>>(this.context.Sgcrate.AsEnumerable());
                    superAssumptions = this.mapper.Map<IEnumerable<SuperAssumptionsViewModel>>(this.context.SuperAssumptions.AsEnumerable());
                    liabilities = this.mapper.Map<IEnumerable<LiabilityViewModel>>(this.context.Liability.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    liabilityDD = this.mapper.Map<IEnumerable<LiabilityDetailsViewModel>>(this.context.LiabilityDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    pensions = this.mapper.Map<IEnumerable<PensionViewModel>>(this.context.Pension.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    pensionDD = this.mapper.Map<IEnumerable<PensionDetailsViewModel>>(this.context.PensionDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    preservationAge = this.mapper.Map<IEnumerable<PreservationAgeViewModel>>(this.context.PreservationAge.AsEnumerable());
                    minimumPensionDrawdown = this.mapper.Map<IEnumerable<MinimumPensionDrawdownViewModel>>(this.context.MinimumPensionDrawdown.AsEnumerable());
                    lifestyleAssets = this.mapper.Map<IEnumerable<LifestyleAssetViewModel>>(this.context.LifestyleAsset.AsEnumerable().Where(x => (x.ClientId == clientId)));
                    assetAssumptions = this.mapper.Map<IEnumerable<AssetTypesViewModel>>(this.context.AssetTypes.AsEnumerable());
                    qualifyingAge = this.mapper.Map<IEnumerable<QualifyingAgeViewModel>>(this.context.QualifyingAge.AsEnumerable());


                    marginalTaxRates = marginalTaxRates.OrderByDescending(i => i.Index);

                    sgcRates = sgcRates.OrderByDescending(i => i.Sgcrate1);

                    cfiClient = cashFlowIncome.Where(c => c.Owner == "Client");
                    cfiPartner = cashFlowIncome.Where(c => c.Owner == "Partner");

                    cfeClient = cashFlowExpenditure.Where(c => c.Owner == "Client");
                    cfePartner = cashFlowExpenditure.Where(c => c.Owner == "Partner");
                    cfeJoint = cashFlowExpenditure.Where(c => c.Owner == "Joint");

                    //PreTaxExpenditures for tax
                    EPRTClient = cashFlowExpenditure.Where(c => c.Owner == "Client").Where(r => r.Type == "Pre-tax");
                    EPRTJoint = cashFlowExpenditure.Where(c => c.Owner == "Joint").Where(r => r.Type == "Pre-tax");
                    EPRTPartner = cashFlowExpenditure.Where(c => c.Owner == "Partner").Where(r => r.Type == "Pre-tax");

                    //Investments
                    investmentClient = investments.Where(c => c.Owner == "Client");
                    investmentClientOptimized = investments.Where(c => c.Owner == "Client");
                    investmentPartner = investments.Where(c => c.Owner == "Partner");
                    investmentPartnerOptimized = investments.Where(c => c.Owner == "Partner");
                    investmentJoint = investments.Where(c => c.Owner == "Joint");
                    investmentJointOptimized = investments.Where(c => c.Owner == "Joint");

                    //Properties
                    propertiesClient = properties.Where(c => c.Owner == "Client");
                    propertiesPartner = properties.Where(c => c.Owner == "Partner");
                    propertiesJoint = properties.Where(c => c.Owner == "Joint");

                    //Supers
                    superClient = supers.Where(c => c.Owner == "Client");
                    superPartner = supers.Where(c => c.Owner == "Partner");

                    //Liabilities
                    liabilityClient = liabilities.Where(c => c.Owner == "Client");
                    liabilityPartner = liabilities.Where(c => c.Owner == "Partner");
                    liabilityJoint = liabilities.Where(c => c.Owner == "Joint");


                    //Pensions
                    pensionClient = pensions.Where(c => c.Owner == "Client");
                    pensionPartner = pensions.Where(c => c.Owner == "Partner");

                    lifestyleClient = lifestyleAssets.Where(c => c.Owner == "Client");
                    lifestylePartner = lifestyleAssets.Where(c => c.Owner == "Partner");
                    lifestyleJoint = lifestyleAssets.Where(c => c.Owner == "Joint");

                    var period = clientDetails.Period + 1;

                    var clientRetirementYear = clientDetails.ClientRetirementYear;
                    var partnerRetirementYear = clientDetails.PartnerRetirementYear;

                    var m = 1;

                    var masterJson = new List<Projection>();
                    for (var i = 0; i < clientDetails.Period; i++)
                    {
                        var json = new Projection();
                        if (i == 0)
                        {
                           
                            json.Date = new DateTime(clientDetails.StartDate + i, 7, 1);
                            json.Lifestyles = new List<Lifestyle>();
                            json.LATotals = new List<Common>();
                            json.Income = new List<Income>();
                            json.Inflow = new List<Income>();
                            json.Outflow = new List<Income>();
                            json.NetCashflows = new List<NetCashflow>();
                            json.Investments = new List<InvestmentValue>();
                            json.FaTotals = new List<Common>();
                            json.Properties = new List<PropertyValue>();
                            json.PropertiesTotal = new List<Common>();
                            json.Liabilities = new List<LiabilityValue>();
                            json.LBTotal = new List<Common>();
                            json.Pensions = new List<PensionValue>();
                            json.PensionTotal = new List<Common>();
                            json.Supers = new List<SuperValue>();
                            json.SuperTotal = new List<Common>();

                            json.ClientDeductions = new List<Income>();
                            json.PartnerDeductions = new List<Income>();
                            json.ClientTaxableIncome = new List<Common>();
                            json.PartnerTaxableIncome = new List<Common>();
                            json.ClientLossAdjustment = new List<Common>();
                            json.PartnerLossAdjustment = new List<Common>();
                            json.GrossTax = new List<Common>();
                            json.ClientNrTaxOffset = new List<Common>();
                            json.PartnerNrTaxOffsets = new List<Common>();
                            json.ClientRTaxOffset = new List<Common>();
                            json.PartnerRTaxOffset = new List<Common>();
                            json.ClientMedicareLevy = new List<Common>();
                            json.PartnerMedicareLevy = new List<Common>();

                            json.NetPayable = new List<Common>();
                            json.TotalPayable = new List<Common>();
                            json.NetAsset = new List<Common>();
                        }
                        else
                        {
                            var projection = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault();
                            json = DeepCopy<Projection>(projection);
                            json.Date = new DateTime(clientDetails.StartDate + i, 7, 1);
                        }


                        var indexRangeInflow = new List<Income>();
                        var indexRangeOutflow = new List<Income>();

                        var clientEmploymentIncome = new List<Income>();
                        var partnerEmploymentIncome = new List<Income>();

                        for (var q = 0; q <= 1; q++)
                        {

                            //TO DO : Get Highest.
                            var highestVal = 0;
                            var highestValObject = new List<InvestmentViewModel>();
                            //    highestVal = this.investmentClient.find Math.max(this.investmentClient.map((o: any) => o.value));
                            if (clientDetails.MaritalStatus == "S")
                            {
                                highestValObject = investmentClient.Where(c => c.Type == "Domestic Cash").ToList();
                                if (highestValObject != null && highestValObject.Count() != 0)
                                {
                                    highestVal = highestValObject[0].InvestmentId;
                                }
                            }
                            else
                            {
                                highestValObject = investmentJoint.Where(c => c.Type == "Domestic Cash").ToList();
                                if (highestValObject != null && highestValObject.Count() != 0)
                                {
                                    highestVal = highestValObject[0].InvestmentId;
                                }
                                else
                                {
                                    highestValObject = investmentClient.Where(c => c.Type == "Domestic Cash").ToList();
                                    if (highestValObject != null && highestValObject.Count() != 0)
                                    {
                                        highestVal = highestValObject[0].InvestmentId;
                                    }
                                    else
                                    {
                                        highestValObject = investmentPartner.Where(c => c.Type == "Domestic Cash").ToList();
                                        if (highestValObject != null && highestValObject.Count() != 0)
                                        {
                                            highestVal = highestValObject[0].InvestmentId;
                                        }
                                    }
                                }
                            }


                            foreach (LifestyleAssetViewModel x in lifestyleClient)
                            {
                                var obj = json.Lifestyles.Where(y => y.Id == x.LassetId).FirstOrDefault();

                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Lifestyle();
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                if (q == 0)
                                {
                                    obj.Owner = x.Owner;
                                    obj.Name = x.Name;

                                    obj.Id = x.LassetId;
                                    obj.Type = x.LassetType;

                                    if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                    {
                                        x.StartDate = clientDetails.StartDate;
                                    }
                                    else if (x.StartDateType == "Client Retirement")
                                    {
                                        x.StartDate = clientDetails.ClientRetirementYear - 1;
                                    }


                                    if (x.EndDateType == "End")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }
                                    else if (x.EndDateType == "Retain")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                    }
                                    else if (x.EndDateType == "Client Retirement")
                                    {
                                        x.EndDate = clientDetails.ClientRetirementYear - 1;
                                    }

                                    if ((clientDetails.StartDate + i) >= x.EndDate)
                                    {
                                        obj.Value = 0;
                                    }
                                    else if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {

                                        if (x.LassetType == "PrimaryResidence")
                                        {
                                            if (j == 0)
                                            {
                                                obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                            }
                                            else
                                            {
                                                obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j)));
                                            }
                                            j++;
                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                    }
                                    else
                                    {
                                        obj.Value = 0;
                                    }

                                    //Purchase of assets
                                    if (x.StartDateType != "Existing")
                                    {

                                        if (x.StartDate == clientDetails.StartDate + i)
                                        {
                                            obj.PurchaseOfAssetValue = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else
                                        {
                                            obj.PurchaseOfAssetValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        obj.PurchaseOfAssetValue = 0;
                                    }

                                    //Sale of assets
                                    if (x.EndDateType != "Retain")
                                    {

                                        if (x.EndDate == clientDetails.StartDate + i)
                                        {
                                            if (x.LassetType == "PrimaryResidence")
                                            {
                                                if (j == 0)
                                                {
                                                    obj.SaleOfAssetValue = Math.Round(Convert.ToDouble(x.Value));

                                                }
                                                else
                                                {
                                                    obj.SaleOfAssetValue = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j)));
                                                }
                                                j++;
                                            }
                                            else
                                            {
                                                obj.SaleOfAssetValue = Math.Round(Convert.ToDouble(x.Value));
                                            }

                                        }
                                        else
                                        {
                                            obj.SaleOfAssetValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        obj.SaleOfAssetValue = 0;
                                    }

                                   
                                }

                                obj.Increment = j;


                                if (json.Lifestyles.Where(y => y.Id == x.LassetId).FirstOrDefault() != null)
                                {
                                    json.Lifestyles[json.Lifestyles.FindIndex(y => y.Id == x.LassetId)] = obj;
                                }
                                else
                                {
                                    json.Lifestyles.Add(obj);

                                }
                            }
                            foreach (LifestyleAssetViewModel x in lifestylePartner)
                            {
                                var obj = json.Lifestyles.Where(y => y.Id == x.LassetId).FirstOrDefault();
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Lifestyle();
                                    j = 0;
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                if (q == 0)
                                {
                                    obj.Owner = x.Owner;
                                    obj.Name = x.Name;

                                    obj.Id = x.LassetId;
                                    obj.Type = x.LassetType;

                                    if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                    {
                                        x.StartDate = clientDetails.StartDate;
                                    }
                                    else if (x.StartDateType == "Partner Retirement")
                                    {
                                        x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                    }


                                    if (x.EndDateType == "End")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }
                                    else if (x.EndDateType == "Retain")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                    }
                                    else if (x.EndDateType == "Partner Retirement")
                                    {
                                        x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                    }

                                    if ((clientDetails.StartDate + i) >= x.EndDate)
                                    {
                                        obj.Value = 0;
                                    }


                                    else if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {

                                        if (x.LassetType == "PrimaryResidence")
                                        {
                                            if (j == 0)
                                            {
                                                obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                            }
                                            else
                                            {
                                                obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j)));
                                            }
                                            j++;
                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                    }
                                    else
                                    {
                                        obj.Value = 0;
                                    }

                                    //Purchase of assets
                                    if (x.StartDateType != "Existing")
                                    {

                                        if (x.StartDate == clientDetails.StartDate + i)
                                        {
                                            obj.PurchaseOfAssetValue = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else
                                        {
                                            obj.PurchaseOfAssetValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        obj.PurchaseOfAssetValue = 0;
                                    }

                                    //Sale of assets
                                    if (x.EndDateType != "Retain")
                                    {

                                        if (x.EndDate == clientDetails.StartDate + i)
                                        {
                                            if (x.LassetType == "PrimaryResidence")
                                            {
                                                if (j == 0)
                                                {
                                                    obj.SaleOfAssetValue = Math.Round(Convert.ToDouble(x.Value));

                                                }
                                                else
                                                {
                                                    obj.SaleOfAssetValue = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j)));
                                                }
                                                j++;
                                            }
                                            else
                                            {
                                                obj.SaleOfAssetValue = Math.Round(Convert.ToDouble(x.Value));
                                            }

                                        }
                                        else
                                        {
                                            obj.SaleOfAssetValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        obj.SaleOfAssetValue = 0;
                                    }

                                   
                                }

                                obj.Increment = j;


                                if (json.Lifestyles.Where(y => y.Id == x.LassetId).FirstOrDefault() != null)
                                {
                                    json.Lifestyles[json.Lifestyles.FindIndex(y => y.Id == x.LassetId)] = obj;
                                }
                                else
                                {
                                    json.Lifestyles.Add(obj);

                                }
                            }
                            foreach (LifestyleAssetViewModel x in lifestyleJoint)
                            {
                                var obj = json.Lifestyles.Where(y => y.Id == x.LassetId).FirstOrDefault();
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Lifestyle();
                                    j = 0;
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                if (q == 0)
                                {
                                    obj.Owner = x.Owner;
                                    obj.Name = x.Name;

                                    obj.Id = x.LassetId;
                                    obj.Type = x.LassetType;

                                    if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                    {
                                        x.StartDate = clientDetails.StartDate;
                                    }
                                    else if (x.StartDateType == "Client Retirement")
                                    {
                                        x.StartDate = clientDetails.ClientRetirementYear - 1;
                                    }
                                    else if (x.StartDateType == "Partner Retirement")
                                    {
                                        x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                    }


                                    if (x.EndDateType == "End")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }
                                    else if (x.EndDateType == "Retain")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                    }
                                    else if (x.EndDateType == "Client Retirement")
                                    {
                                        x.EndDate = clientDetails.ClientRetirementYear - 1;
                                    }
                                    else if (x.EndDateType == "Partner Retirement")
                                    {
                                        x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                    }



                                    if ((clientDetails.StartDate + i) >= x.EndDate)
                                    {
                                        obj.Value = 0;
                                    }


                                    else if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {

                                        if (x.LassetType == "PrimaryResidence")
                                        {
                                            if (j == 0)
                                            {
                                                obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                            }
                                            else
                                            {
                                                obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j)));
                                            }
                                            j++;
                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                    }
                                    else
                                    {
                                        obj.Value = 0;
                                    }

                                    //Purchase of assets
                                    if (x.StartDateType != "Existing")
                                    {

                                        if (x.StartDate == clientDetails.StartDate + i)
                                        {
                                            obj.PurchaseOfAssetValue = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else
                                        {
                                            obj.PurchaseOfAssetValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        obj.PurchaseOfAssetValue = 0;
                                    }

                                    //Sale of assets
                                    if (x.EndDateType != "Retain")
                                    {

                                        if (x.EndDate == clientDetails.StartDate + i)
                                        {
                                            if (x.LassetType == "PrimaryResidence")
                                            {
                                                if (j == 0)
                                                {
                                                    obj.SaleOfAssetValue = Math.Round(Convert.ToDouble(x.Value));

                                                }
                                                else
                                                {
                                                    obj.SaleOfAssetValue = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j)));
                                                }
                                                j++;
                                            }
                                            else
                                            {
                                                obj.SaleOfAssetValue = Math.Round(Convert.ToDouble(x.Value));
                                            }

                                        }
                                        else
                                        {
                                            obj.SaleOfAssetValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        obj.SaleOfAssetValue = 0;
                                    }

                                   
                                }

                                obj.Increment = j;


                                if (json.Lifestyles.Where(y => y.Id == x.LassetId).FirstOrDefault() != null)
                                {
                                    json.Lifestyles[json.Lifestyles.FindIndex(y => y.Id == x.LassetId)] = obj;
                                }
                                else
                                {
                                    json.Lifestyles.Add(obj);

                                }
                            }

                            calculateTotalLASaleProceeds("TotalLASales", json);
                            calculateTotalLAPropertyExpenses("TotalLAPurchase", json);


                            foreach (CashFlowViewModel x in cfiClient)
                            {
                                var obj = indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type != "Tax")).FirstOrDefault();
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                obj.Owner = x.Owner;
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;
                                obj.Type = x.Cftype;
                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }

                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));

                                        }

                                        j++;

                                    }

                                    else
                                    {
                                        obj.Value = 0;
                                    }


                                   
                                }

                                obj.Increment = j;

                                if (indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type != "Tax")).FirstOrDefault() != null)
                                {
                                    indexRangeInflow[indexRangeInflow.FindIndex(y => y.Id == x.CflowId && y.Type != "Tax")] = obj;
                                    clientEmploymentIncome[clientEmploymentIncome.FindIndex(y => y.Id == x.CflowId && y.Type != "Tax")] = obj;
                                    json.Income[json.Income.FindIndex(y => y.Id == x.CflowId && y.Type != "Tax")] = obj;

                                }
                                else
                                {
                                    indexRangeInflow.Add(obj);
                                    json.Income.Add(obj);
                                    clientEmploymentIncome.Add(obj);
                                }
                            }

                            foreach (CashFlowViewModel x in cfiPartner)
                            {
                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                var obj = indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type != "Tax")).FirstOrDefault();
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                if (q == 0)
                                {
                                    obj.Owner = x.Owner;
                                    obj.Name = x.Cfname;
                                    obj.Id = x.CflowId;
                                    obj.Type = x.Cftype;

                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));

                                        }

                                        j++;

                                    }

                                    else
                                    {
                                        obj.Value = 0;
                                    }


                                    
                                }

                                obj.Increment = j;

                                if (indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type != "Tax")).FirstOrDefault() != null)
                                {
                                    indexRangeInflow[indexRangeInflow.FindIndex(y => y.Id == x.CflowId && y.Type != "Tax")] = obj;
                                    partnerEmploymentIncome[partnerEmploymentIncome.FindIndex(y => y.Id == x.CflowId && y.Type != "Tax")] = obj;
                                    json.Income[json.Income.FindIndex(y => y.Id == x.CflowId && y.Type != "Tax")] = obj;
                                }
                                else
                                {
                                    indexRangeInflow.Add(obj);
                                    json.Income.Add(obj);
                                    partnerEmploymentIncome.Add(obj);
                                }
                            }

                            foreach (CashFlowViewModel x in cfiClient) // client
                            {
                                if (x.Type != "Non-Taxable")
                                {
                                    if (x.StartDateType == "Start")
                                    {
                                        x.StartDate = clientDetails.StartDate;
                                    }
                                    else if (x.StartDateType == "Client Retirement")
                                    {
                                        x.StartDate = clientDetails.ClientRetirementYear - 1;
                                    }
                                    //TODO : Reconfirm end date
                                    if (x.EndDateType == "End")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }
                                    else if (x.EndDateType == "Client Retirement")
                                    {
                                        x.EndDate = clientDetails.ClientRetirementYear - 1;
                                    }

                                    var obj = indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type == "Tax")).FirstOrDefault();
                                    var j = 0;
                                    if (obj == null)
                                    {
                                        obj = new Income();
                                        j = 0;
                                    }
                                    else
                                    {
                                        j = obj.Increment;
                                    }
                                    obj.Owner = "ClientIncome-tax";
                                    obj.Name = x.Cfname;
                                    obj.Id = x.CflowId;
                                    obj.Type = "Tax";

                                    if (q == 0)
                                    {
                                        if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                        {
                                            if (j == 0)
                                            {
                                                obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                            }
                                            else
                                            {
                                                obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));
                                            }
                                            j++;
                                        }
                                        else
                                        {
                                            obj.Value = 0;
                                        }

                                        
                                    }

                                    obj.Increment = j;


                                    if (indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type == "Tax")).FirstOrDefault() != null)
                                    {
                                        indexRangeInflow[indexRangeInflow.FindIndex(y => (y.Id == x.CflowId && y.Type == "Tax"))] = obj;
                                        json.Income[json.Income.FindIndex(y => (y.Id == x.CflowId && y.Type == "Tax"))] = obj;
                                    }
                                    else
                                    {
                                        indexRangeInflow.Add(obj);
                                        json.Income.Add(obj);
                                    }


                                }

                            }

                            foreach (CashFlowViewModel x in cfiPartner) // client
                            {
                                if (x.Type != "Non-Taxable")
                                {
                                    if (x.StartDateType == "Start")
                                    {
                                        x.StartDate = clientDetails.StartDate;
                                    }
                                    else if (x.StartDateType == "Partner Retirement")
                                    {
                                        x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                    }
                                    //TODO : Reconfirm end date
                                    if (x.EndDateType == "End")
                                    {
                                        x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }
                                    else if (x.EndDateType == "Partner Retirement")
                                    {
                                        x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                    }

                                    var obj = indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type == "Tax")).FirstOrDefault();
                                    var j = 0;
                                    if (obj == null)
                                    {
                                        obj = new Income();
                                        j = 0;
                                    }
                                    else
                                    {
                                        j = obj.Increment;
                                    }
                                    obj.Owner = "Partner" +
                                        "Income-tax";
                                    obj.Name = x.Cfname;
                                    obj.Id = x.CflowId;
                                    obj.Type = "Tax";

                                    if (q == 0)
                                    {
                                        if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                        {
                                            if (j == 0)
                                            {
                                                obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                            }
                                            else
                                            {
                                                obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));
                                            }
                                            j++;
                                        }
                                        else
                                        {
                                            obj.Value = 0;
                                        }

                                        
                                    }
                                    obj.Increment = j;


                                    if (indexRangeInflow.Where(y => (y.Id == x.CflowId && y.Type == "Tax")).FirstOrDefault() != null)
                                    {
                                        indexRangeInflow[indexRangeInflow.FindIndex(y => (y.Id == x.CflowId && y.Type == "Tax"))] = obj;
                                        json.Income[json.Income.FindIndex(y => (y.Id == x.CflowId && y.Type == "Tax"))] = obj;
                                    }
                                    else
                                    {
                                        indexRangeInflow.Add(obj);
                                        json.Income.Add(obj);
                                    }


                                }

                            }

                            json.Inflow = indexRangeInflow;

                            foreach (CashFlowViewModel x in cfeClient)
                            {
                                var obj = indexRangeOutflow.Where(y => y.Id == x.CflowId).FirstOrDefault();
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                obj.Owner = x.Owner;
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;

                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }

                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));

                                        }

                                        j++;

                                    }

                                    else
                                    {
                                        obj.Value = 0;
                                    }
                             
                                }
                                obj.Increment = j;

                                if (indexRangeOutflow.Where(y => y.Id == x.CflowId).FirstOrDefault() != null)
                                {
                                    indexRangeOutflow[indexRangeOutflow.FindIndex(y => y.Id == x.CflowId)] = obj;
                                }
                                else
                                {
                                    indexRangeOutflow.Add(obj);

                                }
                            }

                            foreach (CashFlowViewModel x in cfePartner)
                            {
                                var obj = indexRangeOutflow.Where(y => y.Id == x.CflowId).FirstOrDefault();
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                obj.Owner = x.Owner;
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;

                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));

                                        }

                                        j++;

                                    }

                                    else
                                    {
                                        obj.Value = 0;
                                    }


                                   
                                }
                                obj.Increment = j;

                                if (indexRangeOutflow.Where(y => y.Id == x.CflowId).FirstOrDefault() != null)
                                {
                                    indexRangeOutflow[indexRangeOutflow.FindIndex(y => y.Id == x.CflowId)] = obj;
                                }
                                else
                                {
                                    indexRangeOutflow.Add(obj);

                                }
                            }

                            foreach (CashFlowViewModel x in cfeJoint)
                            {
                                var obj = indexRangeOutflow.Where(y => y.Id == x.CflowId).FirstOrDefault();
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {
                                    j = obj.Increment;
                                }

                                obj.Owner = x.Owner;
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;

                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));

                                        }

                                        j++;

                                    }

                                    else
                                    {
                                        obj.Value = 0;
                                    }


                                  
                                }
                                obj.Increment = j;

                                if (indexRangeOutflow.Where(y => y.Id == x.CflowId).FirstOrDefault() != null)
                                {
                                    indexRangeOutflow[indexRangeOutflow.FindIndex(y => y.Id == x.CflowId)] = obj;
                                }
                                else
                                {
                                    indexRangeOutflow.Add(obj);

                                }
                            }

                            json.Outflow = indexRangeOutflow;

                            foreach (InvestmentViewModel x in investmentClient)
                            {

                                investmentContribution = investmentCW.Where(c => c.InvestmentId == x.InvestmentId).Where(r => r.Type == "C");
                                investmentWithdrawal = investmentCW.Where(c => c.InvestmentId == x.InvestmentId).Where(r => r.Type == "W");


                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }


                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }


                                var obj = json.Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault();

                                if (obj == null)
                                {
                                    obj = new InvestmentValue();
                                }
                                else
                                {

                                }



                                obj.Owner = x.InvestmentId;
                                obj.Type = "Client";
                                obj.Name = "Opening Value";
                                var previousYearVal = new InvestmentValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault();
                                }

                                //BeginningValue
                                if (m == 1)
                                {
                                    if (x.StartDateType == "Existing")
                                    {
                                        obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else
                                    {
                                        obj.BegValues = 0;
                                    }

                                }
                                else
                                {


                                    if ((clientDetails.StartDate + i) == x.StartDate)
                                    {
                                        obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else if ((clientDetails.StartDate + i) < x.StartDate)
                                    {

                                        obj.BegValues = 0;
                                    }
                                    else
                                    {

                                        obj.BegValues = previousYearVal.EndingValues;
                                    }
                                }


                                if (highestVal == x.InvestmentId)
                                {
                                    if (q == 1)
                                    {

                                        var netValue = new NetCashflow();
                                        netValue = json.NetCashflows.Where(c => c.Owner == "NetCashflow").FirstOrDefault();
                                        obj.CashFlowValues = Convert.ToInt32(netValue.Values);



                                    }
                                    else
                                    {
                                        obj.CashFlowValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.CashFlowValues = 0;
                                }


                                //growth & income
                                var growthUnAdj = Convert.ToDouble(x.Growth / 100) * (obj.BegValues);
                                var incomeUnAdj = Convert.ToDouble(x.Income / 100) * (obj.BegValues);

                                var ICR = Convert.ToDouble(x.ProductFees / 100) * (obj.BegValues);


                                var growthVal = growthUnAdj - (ICR * (growthUnAdj / (growthUnAdj + incomeUnAdj)));
                                var incomeVal = incomeUnAdj - (ICR * (incomeUnAdj / (growthUnAdj + incomeUnAdj)));


                                obj.GrowthValues = Math.Round(growthVal);
                                obj.IncomeValues = Math.Round(incomeVal);



                                //incomePaidOut

                                if (x.Reinvest == "N")
                                {
                                    obj.IncomePaidOutValues = obj.IncomeValues;
                                }
                                else
                                {
                                    obj.IncomePaidOutValues = 0;
                                }

                                //FrankingCredits

                                var corporateTaxRate = generalAssumptions.Where(a => a.Type == "CorporateTaxRate").FirstOrDefault();
                                var ctr = Convert.ToDouble(corporateTaxRate.Percentage) / 100;

                                obj.FrankingCreditsValues = Math.Round(obj.IncomeValues * (ctr / (1 - ctr)) * Convert.ToDouble(x.Franked / 100));

                                //Earnings
                                obj.EarningsValues = Math.Round(obj.GrowthValues + obj.IncomeValues - obj.IncomePaidOutValues);

                                //List of Contributions
                                double ContributionSum = 0;
                                foreach (InvestmentDetailsViewModel y in investmentContribution)
                                { // client

                                    if (y.FromDateType == "Start")
                                    {
                                        y.FromDate = clientDetails.StartDate;
                                    }
                                    if (y.ToDateType == "End")
                                    {
                                        y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }

                                    if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                    {
                                        var t = Convert.ToDouble(y.Value);
                                        ContributionSum = Math.Round(ContributionSum + t);
                                    }

                                }

                                obj.RegularContributionsValues = ContributionSum;



                                //Purchase of assets
                                if (x.StartDateType != "Existing")
                                {

                                    if (x.StartDate == clientDetails.StartDate + i)
                                    {
                                        obj.PurchaseOfAssetValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else
                                    {
                                        obj.PurchaseOfAssetValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.PurchaseOfAssetValues = 0;
                                }

                                //Contributions
                                obj.ContributionsValues = obj.RegularContributionsValues + obj.PurchaseOfAssetValues;

                                //List of Withdrawals
                                //TODO: only if main date started


                                double WithdrawalSum = 0;
                                foreach (InvestmentDetailsViewModel y in investmentWithdrawal)
                                { // client

                                    if (y.FromDateType == "Start")
                                    {
                                        y.FromDate = clientDetails.StartDate;
                                    }
                                    if (y.ToDateType == "End")
                                    {
                                        y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }

                                    if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                    {
                                        var t = Convert.ToDouble(y.Value);
                                        WithdrawalSum = Math.Round(WithdrawalSum + t);
                                    }

                                }

                                obj.RegularWithdrawalsValues = WithdrawalSum;


                                //Sale of assets

                                if (x.EndDateType != "Retain")
                                {

                                    if (x.EndDate == clientDetails.StartDate + i)
                                    {
                                        obj.SaleOfAssetValues = obj.BegValues + obj.EarningsValues + obj.ContributionsValues - obj.RegularWithdrawalsValues;
                                    }
                                    else
                                    {
                                        obj.SaleOfAssetValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.SaleOfAssetValues = 0;
                                }

                                //Withdrawals
                                obj.WithdrawalsValues = obj.RegularWithdrawalsValues + obj.SaleOfAssetValues;

                                if (highestVal == x.InvestmentId)
                                {
                                    if (q == 1)
                                    {

                                        if (obj.CashFlowValues < 0)
                                        {
                                            if (Math.Abs(obj.CashFlowValues) > (obj.BegValues + obj.EarningsValues + obj.ContributionsValues - obj.WithdrawalsValues))
                                            {
                                                obj.UnfundedValues = Math.Abs(obj.CashFlowValues) - (obj.BegValues + obj.EarningsValues + obj.ContributionsValues - obj.WithdrawalsValues);
                                                obj.CashFlowValues = (obj.BegValues + obj.EarningsValues + obj.ContributionsValues - obj.WithdrawalsValues) * -1;

                                            }
                                            else
                                            {
                                                obj.UnfundedValues = 0;
                                            }

                                        }
                                        else
                                        {
                                            obj.UnfundedValues = 0;
                                        }

                                    }
                                    else
                                    {
                                        obj.UnfundedValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.UnfundedValues = 0;
                                }



                                obj.EndingValues = Math.Round(obj.BegValues + obj.CashFlowValues + obj.EarningsValues + obj.ContributionsValues - obj.WithdrawalsValues);

                                //Ending Value PV
                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = Convert.ToDouble(inflation.Percentage) / 100;
                                var k = Convert.ToInt32(clientDetails.StartDate + i) - DateTime.Now.Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / (Math.Pow(Convert.ToDouble(1 + inf), k)));


                                if (x.Type == "Domestic Cash" || x.Type == "Domestic Fixed Interest")
                                {
                                    obj.RealCgValues = 0;
                                    obj.UnrealCgValues = 0;
                                }
                                else
                                {
                                    if (obj.BegValues != 0)
                                    {
                                        //Real and Unreal CG
                                        double TotalCG = 0;
                                        var RateCG = 0;


                                        if (clientDetails.StartDate + i == x.StartDate)
                                        {
                                            TotalCG = Math.Max(0, x.Value - x.CostBase + Convert.ToInt32(obj.GrowthValues));
                                        }
                                        else if (clientDetails.StartDate + i > x.StartDate)
                                        {
                                            TotalCG = previousYearVal.UnrealCgValues + obj.GrowthValues;
                                        }

                                        if (obj.EndingValues == 0)
                                        {
                                            obj.RealCgValues = Math.Round(Convert.ToDouble(TotalCG));
                                        }
                                        else
                                        {
                                            if (obj.WithdrawalsValues != 0)
                                            {
                                                obj.RealCgValues = Math.Round((obj.WithdrawalsValues / obj.EndingValues) * TotalCG);
                                            }
                                            else
                                            {
                                                obj.RealCgValues = 0;
                                            }
                                        }

                                        obj.UnrealCgValues = Math.Round(TotalCG - obj.RealCgValues);

                                    }
                                    else
                                    {
                                        obj.RealCgValues = 0;
                                        obj.UnrealCgValues = 0;
                                    }
                                }



                                if (json.Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault() != null)
                                {
                                    json.Investments[json.Investments.FindIndex(c => c.Owner == x.InvestmentId)] = obj;
                                }
                                else
                                {
                                    json.Investments.Add(obj);
                                }
                            }

                            foreach (InvestmentViewModel x in investmentPartner)
                            {

                                investmentContribution = investmentCW.Where(c => c.InvestmentId == x.InvestmentId).Where(r => r.Type == "C");
                                investmentWithdrawal = investmentCW.Where(c => c.InvestmentId == x.InvestmentId).Where(r => r.Type == "W");


                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }


                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }


                                var obj = json.Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault();

                                if (obj == null)
                                {
                                    obj = new InvestmentValue();
                                }
                                else
                                {

                                }



                                obj.Owner = x.InvestmentId;
                                obj.Type = "Partner";
                                obj.Name = "Opening Value";
                                var previousYearVal = new InvestmentValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault();
                                }

                                //BeginningValue
                                if (m == 1)
                                {
                                    if (x.StartDateType == "Existing")
                                    {
                                        obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else
                                    {
                                        obj.BegValues = 0;
                                    }

                                }
                                else
                                {


                                    if ((clientDetails.StartDate + i) == x.StartDate)
                                    {
                                        obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else if ((clientDetails.StartDate + i) < x.StartDate)
                                    {

                                        obj.BegValues = 0;
                                    }
                                    else
                                    {

                                        obj.BegValues = previousYearVal.EndingValues;
                                    }
                                }


                                


                                //growth & income
                                var growthUnAdj = Convert.ToDouble(x.Growth / 100) * (obj.BegValues);
                                var incomeUnAdj = Convert.ToDouble(x.Income / 100) * (obj.BegValues);

                                var ICR = Convert.ToDouble(x.ProductFees / 100) * (obj.BegValues);


                                var growthVal = growthUnAdj - (ICR * (growthUnAdj / (growthUnAdj + incomeUnAdj)));
                                var incomeVal = incomeUnAdj - (ICR * (incomeUnAdj / (growthUnAdj + incomeUnAdj)));


                                obj.GrowthValues = Math.Round(growthVal);
                                obj.IncomeValues = Math.Round(incomeVal);



                                //incomePaidOut

                                if (x.Reinvest == "N")
                                {
                                    obj.IncomePaidOutValues = obj.IncomeValues;
                                }
                                else
                                {
                                    obj.IncomePaidOutValues = 0;
                                }

                                //FrankingCredits

                                var corporateTaxRate = generalAssumptions.Where(a => a.Type == "CorporateTaxRate").FirstOrDefault();
                                var ctr = Convert.ToDouble(corporateTaxRate.Percentage) / 100;

                                obj.FrankingCreditsValues = Math.Round(obj.IncomeValues * (ctr / (1 - ctr)) * Convert.ToDouble(x.Franked / 100));

                                //Earnings
                                obj.EarningsValues = Math.Round(obj.GrowthValues + obj.IncomeValues - obj.IncomePaidOutValues);

                                //List of Contributions
                                double ContributionSum = 0;
                                foreach (InvestmentDetailsViewModel y in investmentContribution)
                                { // client

                                    if (y.FromDateType == "Start")
                                    {
                                        y.FromDate = clientDetails.StartDate;
                                    }
                                    if (y.ToDateType == "End")
                                    {
                                        y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }

                                    if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                    {
                                        var t = Convert.ToDouble(y.Value);
                                        ContributionSum = Math.Round(ContributionSum + t);
                                    }

                                }

                                obj.RegularContributionsValues = ContributionSum;



                                //Purchase of assets
                                if (x.StartDateType != "Existing")
                                {

                                    if (x.StartDate == clientDetails.StartDate + i)
                                    {
                                        obj.PurchaseOfAssetValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else
                                    {
                                        obj.PurchaseOfAssetValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.PurchaseOfAssetValues = 0;
                                }

                                //Contributions
                                obj.ContributionsValues = obj.RegularContributionsValues + obj.PurchaseOfAssetValues;

                                //List of Withdrawals
                                //TODO: only if main date started


                                double WithdrawalSum = 0;
                                foreach (InvestmentDetailsViewModel y in investmentWithdrawal)
                                { // client

                                    if (y.FromDateType == "Start")
                                    {
                                        y.FromDate = clientDetails.StartDate;
                                    }
                                    if (y.ToDateType == "End")
                                    {
                                        y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }

                                    if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                    {
                                        var t = Convert.ToDouble(y.Value);
                                        WithdrawalSum = Math.Round(WithdrawalSum + t);
                                    }

                                }

                                obj.RegularWithdrawalsValues = WithdrawalSum;


                                //Sale of assets

                                if (x.EndDateType != "Retain")
                                {

                                    if (x.EndDate == clientDetails.StartDate + i)
                                    {
                                        obj.SaleOfAssetValues = obj.BegValues + obj.EarningsValues + obj.ContributionsValues - obj.RegularWithdrawalsValues;
                                    }
                                    else
                                    {
                                        obj.SaleOfAssetValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.SaleOfAssetValues = 0;
                                }

                                //Withdrawals
                                obj.WithdrawalsValues = obj.RegularWithdrawalsValues + obj.SaleOfAssetValues;

                              

                                obj.EndingValues = Math.Round(obj.BegValues  + obj.EarningsValues + obj.ContributionsValues - obj.WithdrawalsValues);

                                //Ending Value PV
                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = Convert.ToDouble(inflation.Percentage) / 100;
                                var k = Convert.ToInt32(clientDetails.StartDate + i) - DateTime.Now.Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / (Math.Pow(Convert.ToDouble(1 + inf), k)));


                                if (x.Type == "Domestic Cash" || x.Type == "Domestic Fixed Interest")
                                {
                                    obj.RealCgValues = 0;
                                    obj.UnrealCgValues = 0;
                                }
                                else
                                {
                                    if (obj.BegValues != 0)
                                    {
                                        //Real and Unreal CG
                                        double TotalCG = 0;
                                        var RateCG = 0;


                                        if (clientDetails.StartDate + i == x.StartDate)
                                        {
                                            TotalCG = Math.Max(0, x.Value - x.CostBase + Convert.ToInt32(obj.GrowthValues));
                                        }
                                        else if (clientDetails.StartDate + i > x.StartDate)
                                        {
                                            TotalCG = previousYearVal.UnrealCgValues + obj.GrowthValues;
                                        }

                                        if (obj.EndingValues == 0)
                                        {
                                            obj.RealCgValues = Math.Round(Convert.ToDouble(TotalCG));
                                        }
                                        else
                                        {
                                            if (obj.WithdrawalsValues != 0)
                                            {
                                                obj.RealCgValues = Math.Round((obj.WithdrawalsValues / obj.EndingValues) * TotalCG);
                                            }
                                            else
                                            {
                                                obj.RealCgValues = 0;
                                            }
                                        }

                                        obj.UnrealCgValues = Math.Round(TotalCG - obj.RealCgValues);

                                    }
                                    else
                                    {
                                        obj.RealCgValues = 0;
                                        obj.UnrealCgValues = 0;
                                    }
                                }



                                if (json.Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault() != null)
                                {
                                    json.Investments[json.Investments.FindIndex(c => c.Owner == x.InvestmentId)] = obj;
                                }
                                else
                                {
                                    json.Investments.Add(obj);
                                }
                            }

                            foreach (InvestmentViewModel x in investmentJoint)
                            {

                                investmentContribution = investmentCW.Where(c => c.InvestmentId == x.InvestmentId).Where(r => r.Type == "C");
                                investmentWithdrawal = investmentCW.Where(c => c.InvestmentId == x.InvestmentId).Where(r => r.Type == "W");


                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }


                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }


                                var obj = json.Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault();

                                if (obj == null)
                                {
                                    obj = new InvestmentValue();
                                }
                                else
                                {

                                }



                                obj.Owner = x.InvestmentId;
                                obj.Type = "Joint";
                                obj.Name = "Opening Value";
                                var previousYearVal = new InvestmentValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault();
                                }

                                //BeginningValue
                                if (m == 1)
                                {
                                    if (x.StartDateType == "Existing")
                                    {
                                        obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else
                                    {
                                        obj.BegValues = 0;
                                    }

                                }
                                else
                                {


                                    if ((clientDetails.StartDate + i) == x.StartDate)
                                    {
                                        obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else if ((clientDetails.StartDate + i) < x.StartDate)
                                    {

                                        obj.BegValues = 0;
                                    }
                                    else
                                    {

                                        obj.BegValues = previousYearVal.EndingValues;
                                    }
                                }





                                //growth & income
                                var growthUnAdj = Convert.ToDouble(x.Growth / 100) * (obj.BegValues);
                                var incomeUnAdj = Convert.ToDouble(x.Income / 100) * (obj.BegValues);

                                var ICR = Convert.ToDouble(x.ProductFees / 100) * (obj.BegValues);


                                var growthVal = growthUnAdj - (ICR * (growthUnAdj / (growthUnAdj + incomeUnAdj)));
                                var incomeVal = incomeUnAdj - (ICR * (incomeUnAdj / (growthUnAdj + incomeUnAdj)));


                                obj.GrowthValues = Math.Round(growthVal);
                                obj.IncomeValues = Math.Round(incomeVal);



                                //incomePaidOut

                                if (x.Reinvest == "N")
                                {
                                    obj.IncomePaidOutValues = obj.IncomeValues;
                                }
                                else
                                {
                                    obj.IncomePaidOutValues = 0;
                                }

                                //FrankingCredits

                                var corporateTaxRate = generalAssumptions.Where(a => a.Type == "CorporateTaxRate").FirstOrDefault();
                                var ctr = Convert.ToDouble(corporateTaxRate.Percentage) / 100;

                                obj.FrankingCreditsValues = Math.Round(obj.IncomeValues * (ctr / (1 - ctr)) * Convert.ToDouble(x.Franked / 100));

                                //Earnings
                                obj.EarningsValues = Math.Round(obj.GrowthValues + obj.IncomeValues - obj.IncomePaidOutValues);

                                //List of Contributions
                                double ContributionSum = 0;
                                foreach (InvestmentDetailsViewModel y in investmentContribution)
                                { // client

                                    if (y.FromDateType == "Start")
                                    {
                                        y.FromDate = clientDetails.StartDate;
                                    }
                                    if (y.ToDateType == "End")
                                    {
                                        y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }

                                    if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                    {
                                        var t = Convert.ToDouble(y.Value);
                                        ContributionSum = Math.Round(ContributionSum + t);
                                    }

                                }

                                obj.RegularContributionsValues = ContributionSum;



                                //Purchase of assets
                                if (x.StartDateType != "Existing")
                                {

                                    if (x.StartDate == clientDetails.StartDate + i)
                                    {
                                        obj.PurchaseOfAssetValues = Math.Round(Convert.ToDouble(x.Value));
                                    }
                                    else
                                    {
                                        obj.PurchaseOfAssetValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.PurchaseOfAssetValues = 0;
                                }

                                //Contributions
                                obj.ContributionsValues = obj.RegularContributionsValues + obj.PurchaseOfAssetValues;

                                //List of Withdrawals
                                //TODO: only if main date started


                                double WithdrawalSum = 0;
                                foreach (InvestmentDetailsViewModel y in investmentWithdrawal)
                                { // client

                                    if (y.FromDateType == "Start")
                                    {
                                        y.FromDate = clientDetails.StartDate;
                                    }
                                    if (y.ToDateType == "End")
                                    {
                                        y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                    }

                                    if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                    {
                                        var t = Convert.ToDouble(y.Value);
                                        WithdrawalSum = Math.Round(WithdrawalSum + t);
                                    }

                                }

                                obj.RegularWithdrawalsValues = WithdrawalSum;


                                //Sale of assets

                                if (x.EndDateType != "Retain")
                                {

                                    if (x.EndDate == clientDetails.StartDate + i)
                                    {
                                        obj.SaleOfAssetValues = obj.BegValues + obj.EarningsValues + obj.ContributionsValues - obj.RegularWithdrawalsValues;
                                    }
                                    else
                                    {
                                        obj.SaleOfAssetValues = 0;
                                    }
                                }
                                else
                                {
                                    obj.SaleOfAssetValues = 0;
                                }

                                //Withdrawals
                                obj.WithdrawalsValues = obj.RegularWithdrawalsValues + obj.SaleOfAssetValues;



                                obj.EndingValues = Math.Round(obj.BegValues + obj.EarningsValues + obj.ContributionsValues - obj.WithdrawalsValues);

                                //Ending Value PV
                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = Convert.ToDouble(inflation.Percentage) / 100;
                                var k = Convert.ToInt32(clientDetails.StartDate + i) - DateTime.Now.Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / (Math.Pow(Convert.ToDouble(1 + inf), k)));


                                if (x.Type == "Domestic Cash" || x.Type == "Domestic Fixed Interest")
                                {
                                    obj.RealCgValues = 0;
                                    obj.UnrealCgValues = 0;
                                }
                                else
                                {
                                    if (obj.BegValues != 0)
                                    {
                                        //Real and Unreal CG
                                        double TotalCG = 0;
                                        var RateCG = 0;


                                        if (clientDetails.StartDate + i == x.StartDate)
                                        {
                                            TotalCG = Math.Max(0, x.Value - x.CostBase + Convert.ToInt32(obj.GrowthValues));
                                        }
                                        else if (clientDetails.StartDate + i > x.StartDate)
                                        {
                                            TotalCG = previousYearVal.UnrealCgValues + obj.GrowthValues;
                                        }

                                        if (obj.EndingValues == 0)
                                        {
                                            obj.RealCgValues = Math.Round(Convert.ToDouble(TotalCG));
                                        }
                                        else
                                        {
                                            if (obj.WithdrawalsValues != 0)
                                            {
                                                obj.RealCgValues = Math.Round((obj.WithdrawalsValues / obj.EndingValues) * TotalCG);
                                            }
                                            else
                                            {
                                                obj.RealCgValues = 0;
                                            }
                                        }

                                        obj.UnrealCgValues = Math.Round(TotalCG - obj.RealCgValues);

                                    }
                                    else
                                    {
                                        obj.RealCgValues = 0;
                                        obj.UnrealCgValues = 0;
                                    }
                                }



                                if (json.Investments.Where(y => y.Owner == x.InvestmentId).FirstOrDefault() != null)
                                {
                                    json.Investments[json.Investments.FindIndex(c => c.Owner == x.InvestmentId)] = obj;
                                }
                                else
                                {
                                    json.Investments.Add(obj);
                                }
                            }

                            calculateTotalInvestmentPaidOut("TotalIPO", json);
                            calculateTotalInvestmentWithdrawals("TotalIW", json);
                            calculateTotalInvestmentContributions("TotalIC", json);
                            calculateTotalInvestmentEarnings("TotalTaxIE-client", "Client", json);
                            calculateTotalInvestmentEarnings("TotalTaxIE-partner", "Partner", json);
                            calculateRealizedCGFA("RCGFA-client", "Client", json);
                            calculateRealizedCGFA("RCGFA-partner", "Partner", json);
                            calculateFrankingCredits("FrankingCredits-client", "Client", json);
                            calculateFrankingCredits("FrankingCredits-partner", "Partner", json);

                            foreach (PropertyViewModel x in propertiesClient) { // client

                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }


                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Retain")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }

                                var obj = json.Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault();
                                if (obj == null)
                                {
                                    obj = new PropertyValue();
                                }
                                else
                                {
                                }

                                var propPurchase = 0;
                                var propSale = 0;

                                obj.Owner= x.PropertyId;
                                obj.Type= "Client";
                                obj.Name = x.Name;
                                obj.StartDateType = x.StartDateType;

                                var previousYearVal = new PropertyValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault();
                                }

                    

                                if (m == 1 && x.StartDateType == "Existing")
                                {
                                        obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                }
                                else if ((clientDetails.StartDate + i) <= x.StartDate)
                                {

                                      obj.BegValues = 0;
                                }
                                else
                                {
                                        obj.BegValues = previousYearVal.EndingValues;
                                }



                                //capital growth
                                obj.CapitalGrowthValues = Math.Round(Convert.ToDouble(x.Growth / 100) * obj.BegValues);

                                ////Ending Value


                                if ((clientDetails.StartDate + i) == x.StartDate)
                                {
                                    obj.EndingValues = Math.Round(Convert.ToDouble(x.Value));
                                    obj.PropertyPurchaseValues = Math.Round(Convert.ToDouble(x.Value));
                                    propPurchase = clientDetails.StartDate + i;
                                   obj.PropertySaleValues = 0;

                                }
                                else if ((clientDetails.StartDate + i) < x.StartDate)
                                {
                                    obj.EndingValues = 0;
                                    obj.PropertyPurchaseValues = 0;


                                }


                                if ((clientDetails.StartDate + i) == x.EndDate)
                                {
                                    obj.PropertySaleValues = Math.Round(obj.BegValues + obj.CapitalGrowthValues);
                                    obj.EndingValues = 0;
                                }
                                else if ((clientDetails.StartDate + i) > x.StartDate && (clientDetails.StartDate + i) < x.EndDate)
                                {
                                    propSale = clientDetails.StartDate + i;
                                    obj.PropertySaleValues = 0;
                                    obj.EndingValues = Math.Round(obj.BegValues + obj.CapitalGrowthValues);

                                }
                                else if ((clientDetails.StartDate + i) > x.EndDate)
                                {
                                    obj.EndingValues = 0;
                                    obj.PropertyPurchaseValues = 0;
                                    obj.PropertySaleValues = 0;
                                }




                                ////Ending Value PV
                               
                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = Convert.ToDouble(inflation.Percentage) / 100;
                                var k = Convert.ToInt32(clientDetails.StartDate + i) - DateTime.Now.Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / (Math.Pow(Convert.ToDouble(1 + inf), k)));


                                //rent
                                if ((clientDetails.StartDate + i) >= x.EndDate)
                                {
                                    obj.RentValues = 0;
                                }
                                else
                                {
                                    double rentVal = 0;

                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        rentVal = Convert.ToDouble(x.Rent);
                                    }

                                   obj.RentValues = rentVal;
                                }


                                //expenses
                                if ((clientDetails.StartDate + i) >= x.EndDate)
                                {
                                    obj.ExpensesValues = 0;
                                }
                                else
                                {
                                    double expensesVal = 0;

                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        expensesVal = Convert.ToDouble(x.Expenses);
                                    }

                                    obj.ExpensesValues = expensesVal;
                                }

                                if (obj.BegValues != 0)
                                {
                                    //Real and Unreal CG
                                    double TotalCG = 0;
                                    var RateCG = 0;


                                    if (clientDetails.StartDate + i == x.StartDate)
                                    {
                                        TotalCG = Math.Max(0, x.Value - x.CostBase + Convert.ToInt32(obj.CapitalGrowthValues));
                                    }
                                    else if (clientDetails.StartDate + i > x.StartDate)
                                    {
                                        TotalCG = previousYearVal.UnrealCgValues + obj.CapitalGrowthValues;
                                    }

                                    if (obj.EndingValues == 0)
                                    {
                                        obj.RealCgValues = Math.Round(Convert.ToDouble(TotalCG));
                                    }
                                    else
                                    {
                                        if (obj.PropertySaleValues != 0)
                                        {
                                            obj.RealCgValues = Math.Round((obj.PropertySaleValues / obj.EndingValues) * TotalCG);
                                        }
                                        else
                                        {
                                            obj.RealCgValues = 0;
                                        }
                                    }

                                    obj.UnrealCgValues = Math.Round(TotalCG - obj.RealCgValues);

                                }
                                else
                                {
                                    obj.RealCgValues = 0;
                                    obj.UnrealCgValues = 0;
                                }


                                if (json.Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault() != null)
                                {
                                    json.Properties[json.Properties.FindIndex(c => c.Owner == x.PropertyId)] = obj;
                                }
                                else
                                {
                                    json.Properties.Add(obj);
                                }

                            }
                            foreach (PropertyViewModel x in propertiesPartner)
                            { // client

                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }


                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Retain")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }

                                var obj = json.Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault();
                                if (obj == null)
                                {
                                    obj = new PropertyValue();
                                }
                                else
                                {
                                }

                                var propPurchase = 0;
                                var propSale = 0;

                                obj.Owner = x.PropertyId;
                                obj.Type = "Partner";
                                obj.Name = x.Name;
                                obj.StartDateType = x.StartDateType;

                                var previousYearVal = new PropertyValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault();
                                }



                                if (m == 1 && x.StartDateType == "Existing")
                                {
                                    obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                }
                                else if ((clientDetails.StartDate + i) <= x.StartDate)
                                {

                                    obj.BegValues = 0;
                                }
                                else
                                {
                                    obj.BegValues = previousYearVal.EndingValues;
                                }



                                //capital growth
                                obj.CapitalGrowthValues = Math.Round(Convert.ToDouble(x.Growth / 100) * obj.BegValues);

                                ////Ending Value


                                if ((clientDetails.StartDate + i) == x.StartDate)
                                {
                                    obj.EndingValues = Math.Round(Convert.ToDouble(x.Value));
                                    obj.PropertyPurchaseValues = Math.Round(Convert.ToDouble(x.Value));
                                    propPurchase = clientDetails.StartDate + i;
                                    obj.PropertySaleValues = 0;

                                }
                                else if ((clientDetails.StartDate + i) < x.StartDate)
                                {
                                    obj.EndingValues = 0;
                                    obj.PropertyPurchaseValues = 0;


                                }


                                if ((clientDetails.StartDate + i) == x.EndDate)
                                {
                                    obj.PropertySaleValues = Math.Round(obj.BegValues + obj.CapitalGrowthValues);
                                    obj.EndingValues = 0;
                                }
                                else if ((clientDetails.StartDate + i) > x.StartDate && (clientDetails.StartDate + i) < x.EndDate)
                                {
                                    propSale = clientDetails.StartDate + i;
                                    obj.PropertySaleValues = 0;
                                    obj.EndingValues = Math.Round(obj.BegValues + obj.CapitalGrowthValues);

                                }
                                else if ((clientDetails.StartDate + i) > x.EndDate)
                                {
                                    obj.EndingValues = 0;
                                    obj.PropertyPurchaseValues = 0;
                                    obj.PropertySaleValues = 0;
                                }




                                ////Ending Value PV

                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = Convert.ToDouble(inflation.Percentage) / 100;
                                var k = Convert.ToInt32(clientDetails.StartDate + i) - DateTime.Now.Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / (Math.Pow(Convert.ToDouble(1 + inf), k)));


                                //rent
                                if ((clientDetails.StartDate + i) >= x.EndDate)
                                {
                                    obj.RentValues = 0;
                                }
                                else
                                {
                                    double rentVal = 0;

                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        rentVal = Convert.ToDouble(x.Rent);
                                    }

                                    obj.RentValues = rentVal;
                                }


                                //expenses
                                if ((clientDetails.StartDate + i) >= x.EndDate)
                                {
                                    obj.ExpensesValues = 0;
                                }
                                else
                                {
                                    double expensesVal = 0;

                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        expensesVal = Convert.ToDouble(x.Expenses);
                                    }

                                    obj.ExpensesValues = expensesVal;
                                }

                                if (obj.BegValues != 0)
                                {
                                    //Real and Unreal CG
                                    double TotalCG = 0;
                                    var RateCG = 0;


                                    if (clientDetails.StartDate + i == x.StartDate)
                                    {
                                        TotalCG = Math.Max(0, x.Value - x.CostBase + Convert.ToInt32(obj.CapitalGrowthValues));
                                    }
                                    else if (clientDetails.StartDate + i > x.StartDate)
                                    {
                                        TotalCG = previousYearVal.UnrealCgValues + obj.CapitalGrowthValues;
                                    }

                                    if (obj.EndingValues == 0)
                                    {
                                        obj.RealCgValues = Math.Round(Convert.ToDouble(TotalCG));
                                    }
                                    else
                                    {
                                        if (obj.PropertySaleValues != 0)
                                        {
                                            obj.RealCgValues = Math.Round((obj.PropertySaleValues / obj.EndingValues) * TotalCG);
                                        }
                                        else
                                        {
                                            obj.RealCgValues = 0;
                                        }
                                    }

                                    obj.UnrealCgValues = Math.Round(TotalCG - obj.RealCgValues);

                                }
                                else
                                {
                                    obj.RealCgValues = 0;
                                    obj.UnrealCgValues = 0;
                                }


                                if (json.Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault() != null)
                                {
                                    json.Properties[json.Properties.FindIndex(c => c.Owner == x.PropertyId)] = obj;
                                }
                                else
                                {
                                    json.Properties.Add(obj);
                                }

                            }
                            foreach (PropertyViewModel x in propertiesJoint)
                            { // client

                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }


                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Retain")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }

                                var obj = json.Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault();
                                if (obj == null)
                                {
                                    obj = new PropertyValue();
                                }
                                else
                                {
                                }

                                var propPurchase = 0;
                                var propSale = 0;

                                obj.Owner = x.PropertyId;
                                obj.Type = "Joint";
                                obj.Name = x.Name;
                                obj.StartDateType = x.StartDateType;

                                var previousYearVal = new PropertyValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault();
                                }



                                if (m == 1 && x.StartDateType == "Existing")
                                {
                                    obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                }
                                else if ((clientDetails.StartDate + i) <= x.StartDate)
                                {

                                    obj.BegValues = 0;
                                }
                                else
                                {
                                    obj.BegValues = previousYearVal.EndingValues;
                                }



                                //capital growth
                                obj.CapitalGrowthValues = Math.Round(Convert.ToDouble(x.Growth / 100) * obj.BegValues);

                                ////Ending Value


                                if ((clientDetails.StartDate + i) == x.StartDate)
                                {
                                    obj.EndingValues = Math.Round(Convert.ToDouble(x.Value));
                                    obj.PropertyPurchaseValues = Math.Round(Convert.ToDouble(x.Value));
                                    propPurchase = clientDetails.StartDate + i;
                                    obj.PropertySaleValues = 0;

                                }
                                else if ((clientDetails.StartDate + i) < x.StartDate)
                                {
                                    obj.EndingValues = 0;
                                    obj.PropertyPurchaseValues = 0;


                                }


                                if ((clientDetails.StartDate + i) == x.EndDate)
                                {
                                    obj.PropertySaleValues = Math.Round(obj.BegValues + obj.CapitalGrowthValues);
                                    obj.EndingValues = 0;
                                }
                                else if ((clientDetails.StartDate + i) > x.StartDate && (clientDetails.StartDate + i) < x.EndDate)
                                {
                                    propSale = clientDetails.StartDate + i;
                                    obj.PropertySaleValues = 0;
                                    obj.EndingValues = Math.Round(obj.BegValues + obj.CapitalGrowthValues);

                                }
                                else if ((clientDetails.StartDate + i) > x.EndDate)
                                {
                                    obj.EndingValues = 0;
                                    obj.PropertyPurchaseValues = 0;
                                    obj.PropertySaleValues = 0;
                                }




                                ////Ending Value PV

                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = Convert.ToDouble(inflation.Percentage) / 100;
                                var k = Convert.ToInt32(clientDetails.StartDate + i) - DateTime.Now.Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / (Math.Pow(Convert.ToDouble(1 + inf), k)));


                                //rent
                                if ((clientDetails.StartDate + i) >= x.EndDate)
                                {
                                    obj.RentValues = 0;
                                }
                                else
                                {
                                    double rentVal = 0;

                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        rentVal = Convert.ToDouble(x.Rent);
                                    }

                                    obj.RentValues = rentVal;
                                }


                                //expenses
                                if ((clientDetails.StartDate + i) >= x.EndDate)
                                {
                                    obj.ExpensesValues = 0;
                                }
                                else
                                {
                                    double expensesVal = 0;

                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        expensesVal = Convert.ToDouble(x.Expenses);
                                    }

                                    obj.ExpensesValues = expensesVal;
                                }

                                if (obj.BegValues != 0)
                                {
                                    //Real and Unreal CG
                                    double TotalCG = 0;
                                    var RateCG = 0;


                                    if (clientDetails.StartDate + i == x.StartDate)
                                    {
                                        TotalCG = Math.Max(0, x.Value - x.CostBase + Convert.ToInt32(obj.CapitalGrowthValues));
                                    }
                                    else if (clientDetails.StartDate + i > x.StartDate)
                                    {
                                        TotalCG = previousYearVal.UnrealCgValues + obj.CapitalGrowthValues;
                                    }

                                    if (obj.EndingValues == 0)
                                    {
                                        obj.RealCgValues = Math.Round(Convert.ToDouble(TotalCG));
                                    }
                                    else
                                    {
                                        if (obj.PropertySaleValues != 0)
                                        {
                                            obj.RealCgValues = Math.Round((obj.PropertySaleValues / obj.EndingValues) * TotalCG);
                                        }
                                        else
                                        {
                                            obj.RealCgValues = 0;
                                        }
                                    }

                                    obj.UnrealCgValues = Math.Round(TotalCG - obj.RealCgValues);

                                }
                                else
                                {
                                    obj.RealCgValues = 0;
                                    obj.UnrealCgValues = 0;
                                }


                                if (json.Properties.Where(y => y.Owner == x.PropertyId).FirstOrDefault() != null)
                                {
                                    json.Properties[json.Properties.FindIndex(c => c.Owner == x.PropertyId)] = obj;
                                }
                                else
                                {
                                    json.Properties.Add(obj);
                                }

                            }

                            calculateTotalRent("TotalRent", json);
                            calculateTotalSaleProceeds("TotalSaleProceeds", json);
                            calculateTotalPropertyExpenses("TotalPropertyExpenses", json);
                            calculateInvestmentPropertyExpenses("InvestmentPropertyExpenses-client", "Client", json);
                            calculateInvestmentPropertyExpenses("InvestmentPropertyExpenses-partner", "Partner", json);
                            calculateRealizedCGP("RCGP-client", "Client", json, clientDetails.StartDate + i);
                            calculateRealizedCGP("RCGP-partner", "Partner", json, clientDetails.StartDate + i);

                            foreach (LiabilityViewModel x in liabilityClient)
                            { // client

                                liabilityDrawDown = liabilityDD.Where(c => c.LiabilityId == x.LiabilityId);


                                if (x.CommenceOnDateType == "Start" || x.CommenceOnDateType == "Existing")
                                {
                                    x.CommenceOnDate = clientDetails.StartDate;
                                }
                                if (x.RepaymentDateType == "End")
                                {
                                    x.Repayment = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.RepaymentDateType == "Retain")
                                {
                                    x.Repayment = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }


                                var obj = json.Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault();
                               

                                if (obj == null)
                                {
                                    obj = new LiabilityValue();
                                }
                                else
                                {
                                   
                                }

                                obj.Owner = x.LiabilityId;
                                obj.Type = "Client";
                                obj.Name = "Opening Value";
                                obj.Deductibility= x.Deductibility;


                                var previousYearVal = new LiabilityValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault();
                                }



                                if (x.CommenceOnDate <= clientDetails.StartDate + i && x.RepaymentDate >= clientDetails.StartDate + i)
                                {

                                    //BeginningValue
                                    if (m == 1)
                                    {
                                        if (x.CommenceOnDateType == "Existing")
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Principal));
                                        }
                                        else
                                        {
                                            obj.BegValues = 0;
                                        }

                                    }
                                    else
                                    {


                                        if ((clientDetails.StartDate + i) == x.CommenceOnDate)
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Principal));
                                        }
                                        else if ((clientDetails.StartDate + i) < x.CommenceOnDate)
                                        {

                                            obj.BegValues = 0;
                                        }
                                        else
                                        {

                                            obj.BegValues = previousYearVal.EndingValues;
                                        }
                                    }
                                   
                                      
                                }
                                else
                                {
                                    obj.BegValues = 0;
                                }

                                //accrued Interest
                                obj.AccruedInterestValues = Math.Round(obj.BegValues * Convert.ToDouble(x.InterestRate / 100));

                                //Minimum repayment

                                double PMT = 0;
                                if (x.RepaymentType == "IO")
                                {
                                    PMT = obj.AccruedInterestValues;
                                }
                                else
                                {
                                    PMT = x.Principal * Convert.ToDouble(x.InterestRate / 100) * (Math.Pow(Convert.ToDouble(1 + (x.InterestRate / 100)), x.Term) / (Math.Pow(Convert.ToDouble(1 + (x.InterestRate / 100)), x.Term) - 1));
                                }
                                if (x.CommenceOnDate <= clientDetails.StartDate + i && x.RepaymentDate >= clientDetails.StartDate + i)
                                {

                                    //Repayment Value
                                    double Repmt= 0;
                                    if (x.RepaymentDateType == "Retain")
                                    {
                                        Repmt = Math.Max(x.Repayment, PMT);
                                    }
                                    else if ((clientDetails.StartDate + i) == x.RepaymentDate)
                                    {
                                        Repmt =obj.BegValues + obj.AccruedInterestValues;
                                    }
                                    else
                                    {
                                        Repmt = Math.Max(x.Repayment, PMT);
                                    }
                                    var repayment = Math.Min((obj.BegValues + obj.AccruedInterestValues), Repmt);
                                    obj.RepmtValues = Math.Round(repayment);
                                }
                                else
                                {
                                   obj.RepmtValues = 0;
                                }
                                ////Ending Value
                                if (obj.RepmtValues > (obj.BegValues + obj.AccruedInterestValues))
                                {
                                    obj.EndingValues = 0;
                                }
                                else
                                {
                                    obj.EndingValues = Math.Round(obj.BegValues + obj.AccruedInterestValues - obj.RepmtValues);
                                }

                             

                                if (json.Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault() != null)
                                {
                                    json.Liabilities[json.Liabilities.FindIndex(c => c.Owner == x.LiabilityId)] = obj;
                                }
                                else
                                {
                                    json.Liabilities.Add(obj);
                                }
                            }
                            foreach (LiabilityViewModel x in liabilityPartner)
                            { // client

                                liabilityDrawDown = liabilityDD.Where(c => c.LiabilityId == x.LiabilityId);


                                if (x.CommenceOnDateType == "Start" || x.CommenceOnDateType == "Existing")
                                {
                                    x.CommenceOnDate = clientDetails.StartDate;
                                }
                                if (x.RepaymentDateType == "End")
                                {
                                    x.Repayment = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.RepaymentDateType == "Retain")
                                {
                                    x.Repayment = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }


                                var obj = json.Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault();


                                if (obj == null)
                                {
                                    obj = new LiabilityValue();
                                }
                                else
                                {

                                }

                                obj.Owner = x.LiabilityId;
                                obj.Type = "Partner";
                                obj.Name = "Opening Value";
                                obj.Deductibility = x.Deductibility;


                                var previousYearVal = new LiabilityValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault();
                                }



                                if (x.CommenceOnDate <= clientDetails.StartDate + i && x.RepaymentDate >= clientDetails.StartDate + i)
                                {

                                    //BeginningValue
                                    if (m == 1)
                                    {
                                        if (x.CommenceOnDateType == "Existing")
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Principal));
                                        }
                                        else
                                        {
                                            obj.BegValues = 0;
                                        }

                                    }
                                    else
                                    {


                                        if ((clientDetails.StartDate + i) == x.CommenceOnDate)
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Principal));
                                        }
                                        else if ((clientDetails.StartDate + i) < x.CommenceOnDate)
                                        {

                                            obj.BegValues = 0;
                                        }
                                        else
                                        {

                                            obj.BegValues = previousYearVal.EndingValues;
                                        }
                                    }


                                }
                                else
                                {
                                    obj.BegValues = 0;
                                }

                                //accrued Interest
                                obj.AccruedInterestValues = Math.Round(obj.BegValues * Convert.ToDouble(x.InterestRate / 100));

                                //Minimum repayment

                                double PMT = 0;
                                if (x.RepaymentType == "IO")
                                {
                                    PMT = obj.AccruedInterestValues;
                                }
                                else
                                {
                                    PMT = x.Principal * Convert.ToDouble(x.InterestRate / 100) * (Math.Pow(Convert.ToDouble(1 + (x.InterestRate / 100)), x.Term) / (Math.Pow(Convert.ToDouble(1 + (x.InterestRate / 100)), x.Term) - 1));
                                }
                                if (x.CommenceOnDate <= clientDetails.StartDate + i && x.RepaymentDate >= clientDetails.StartDate + i)
                                {

                                    //Repayment Value
                                    double Repmt = 0;
                                    if (x.RepaymentDateType == "Retain")
                                    {
                                        Repmt = Math.Max(x.Repayment, PMT);
                                    }
                                    else if ((clientDetails.StartDate + i) == x.RepaymentDate)
                                    {
                                        Repmt = obj.BegValues + obj.AccruedInterestValues;
                                    }
                                    else
                                    {
                                        Repmt = Math.Max(x.Repayment, PMT);
                                    }
                                    var repayment = Math.Min((obj.BegValues + obj.AccruedInterestValues), Repmt);
                                    obj.RepmtValues = Math.Round(repayment);
                                }
                                else
                                {
                                    obj.RepmtValues = 0;
                                }
                                ////Ending Value
                                if (obj.RepmtValues > (obj.BegValues + obj.AccruedInterestValues))
                                {
                                    obj.EndingValues = 0;
                                }
                                else
                                {
                                    obj.EndingValues = Math.Round(obj.BegValues + obj.AccruedInterestValues - obj.RepmtValues);
                                }



                                if (json.Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault() != null)
                                {
                                    json.Liabilities[json.Liabilities.FindIndex(c => c.Owner == x.LiabilityId)] = obj;
                                }
                                else
                                {
                                    json.Liabilities.Add(obj);
                                }
                            }
                            foreach (LiabilityViewModel x in liabilityJoint)
                            { // client

                                liabilityDrawDown = liabilityDD.Where(c => c.LiabilityId == x.LiabilityId);


                                if (x.CommenceOnDateType == "Start" || x.CommenceOnDateType == "Existing")
                                {
                                    x.CommenceOnDate = clientDetails.StartDate;
                                }
                                if (x.RepaymentDateType == "End")
                                {
                                    x.Repayment = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.RepaymentDateType == "Retain")
                                {
                                    x.Repayment = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }


                                var obj = json.Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault();


                                if (obj == null)
                                {
                                    obj = new LiabilityValue();
                                }
                                else
                                {

                                }

                                obj.Owner = x.LiabilityId;
                                obj.Type = "Joint";
                                obj.Name = "Opening Value";
                                obj.Deductibility = x.Deductibility;


                                var previousYearVal = new LiabilityValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault();
                                }



                                if (x.CommenceOnDate <= clientDetails.StartDate + i && x.RepaymentDate >= clientDetails.StartDate + i)
                                {

                                    //BeginningValue
                                    if (m == 1)
                                    {
                                        if (x.CommenceOnDateType == "Existing")
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Principal));
                                        }
                                        else
                                        {
                                            obj.BegValues = 0;
                                        }

                                    }
                                    else
                                    {


                                        if ((clientDetails.StartDate + i) == x.CommenceOnDate)
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Principal));
                                        }
                                        else if ((clientDetails.StartDate + i) < x.CommenceOnDate)
                                        {

                                            obj.BegValues = 0;
                                        }
                                        else
                                        {

                                            obj.BegValues = previousYearVal.EndingValues;
                                        }
                                    }


                                }
                                else
                                {
                                    obj.BegValues = 0;
                                }

                                //accrued Interest
                                obj.AccruedInterestValues = Math.Round(obj.BegValues * Convert.ToDouble(x.InterestRate / 100));

                                //Minimum repayment

                                double PMT = 0;
                                if (x.RepaymentType == "IO")
                                {
                                    PMT = obj.AccruedInterestValues;
                                }
                                else
                                {
                                    PMT = x.Principal * Convert.ToDouble(x.InterestRate / 100) * (Math.Pow(Convert.ToDouble(1 + (x.InterestRate / 100)), x.Term) / (Math.Pow(Convert.ToDouble(1 + (x.InterestRate / 100)), x.Term) - 1));
                                }
                                if (x.CommenceOnDate <= clientDetails.StartDate + i && x.RepaymentDate >= clientDetails.StartDate + i)
                                {

                                    //Repayment Value
                                    double Repmt = 0;
                                    if (x.RepaymentDateType == "Retain")
                                    {
                                        Repmt = Math.Max(x.Repayment, PMT);
                                    }
                                    else if ((clientDetails.StartDate + i) == x.RepaymentDate)
                                    {
                                        Repmt = obj.BegValues + obj.AccruedInterestValues;
                                    }
                                    else
                                    {
                                        Repmt = Math.Max(x.Repayment, PMT);
                                    }
                                    var repayment = Math.Min((obj.BegValues + obj.AccruedInterestValues), Repmt);
                                    obj.RepmtValues = Math.Round(repayment);
                                }
                                else
                                {
                                    obj.RepmtValues = 0;
                                }
                                ////Ending Value
                                if (obj.RepmtValues > (obj.BegValues + obj.AccruedInterestValues))
                                {
                                    obj.EndingValues = 0;
                                }
                                else
                                {
                                    obj.EndingValues = Math.Round(obj.BegValues + obj.AccruedInterestValues - obj.RepmtValues);
                                }



                                if (json.Liabilities.Where(y => y.Owner == x.LiabilityId).FirstOrDefault() != null)
                                {
                                    json.Liabilities[json.Liabilities.FindIndex(c => c.Owner == x.LiabilityId)] = obj;
                                }
                                else
                                {
                                    json.Liabilities.Add(obj);
                                }
                            }

                            this.calculateTotalDebtRepayment("TotalDebtRepayment", json);
                            this.calculateAccruedLiabilities("Accruedliability-client", "Client", json);
                            this.calculateAccruedLiabilities("Accruedliability-partner", "Partner", json);

                            foreach (PensionViewModel x in pensionClient)
                            {

                                pensionDrawDown = pensionDD.Where(c => c.PensionId == x.PensionId);
                                if (x.PensionRebootFromType == "Start" || x.PensionRebootFromType == "Existing")
                                {
                                    x.PensionRebootFromDate = clientDetails.StartDate;
                                }
                                else if (x.PensionRebootFromType == "Client Retirement")
                                {
                                    x.PensionRebootFromDate = clientDetails.ClientRetirementYear - 1;
                                }
                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Retain")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }

                                var obj = json.Pensions.Where(y => y.Owner == x.PensionId).FirstOrDefault();
                                var TaxableProp = 0;
                                var TaxFreeProp = 0;

                                if (obj == null)
                                {
                                    obj = new PensionValue();
                                  
                                }
                                else
                                {
                                    

                                }

                                obj.Owner = x.PensionId;
                                obj.Type = "Client";
                                obj.Name = "Opening Value";

                                var previousYearVal = new PensionValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Pensions.Where(y => y.Owner == x.PensionId).FirstOrDefault();
                                }
                                if (x.PensionRebootFromDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {

                                    //BeginningValue
                                    if (m == 1)
                                    {
                                        if (x.PensionRebootFromType == "Existing")
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else
                                        {
                                            obj.BegValues = 0;
                                        }

                                    }
                                    else
                                    {


                                        if ((clientDetails.StartDate + i) == x.PensionRebootFromDate)
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else if ((clientDetails.StartDate + i) < x.PensionRebootFromDate)
                                        {

                                            obj.BegValues = 0;
                                        }
                                        else
                                        {

                                            obj.BegValues = previousYearVal.EndingValues;
                                        }
                                    }

                                    if ((clientDetails.StartDate + i) == x.PensionRebootFromDate || x.PensionRebootFromType == "Existing")
                                    {
                                        //TODO Confirm negative
                                        if (x.TaxableComponent != 0 && x.Value != 0)
                                        {
                                            TaxableProp = (x.TaxableComponent / x.Value);
                                        }
                                        else
                                        {
                                            TaxableProp = 0;
                                        }
                                        TaxFreeProp = 1 - TaxableProp;

                                        obj.TaxableBegValues = Math.Round(Convert.ToDouble(x.TaxableComponent));
                                        obj.TaxFreeBegValues = Math.Round(Convert.ToDouble(x.TaxFreeComponent));
                                    }
                                    else if ((clientDetails.StartDate + i) < x.PensionRebootFromDate)
                                    {

                                        obj.TaxableBegValues = 0;
                                        obj.TaxFreeBegValues = 0;
                                    }
                                    else
                                    {

                                        obj.TaxableBegValues = previousYearVal.TaxableEndingValues;
                                        obj.TaxFreeBegValues = previousYearVal.TaxFreeEndingValues;
                                    }
                                }
                                else
                                {
                                    obj.TaxableBegValues = 0;
                                    obj.TaxFreeBegValues = 0;
                                    obj.BegValues = 0;

                                }

                              


                                ////growth & income
                                var growthUnAdj = Convert.ToDouble(x.Growth / 100) * obj.BegValues;
                                var incomeUnAdj = Convert.ToDouble(x.Income / 100) * obj.BegValues;

                                var ICR = Convert.ToDouble(x.ProductFees / 100) * obj.BegValues;


                                var growthVal = growthUnAdj - (ICR * (growthUnAdj / (growthUnAdj + incomeUnAdj)));
                                var incomeVal = incomeUnAdj - (ICR * (incomeUnAdj / (growthUnAdj + incomeUnAdj)));


                                obj.GrowthValues = Math.Round(growthVal);
                                obj.IncomeValues = Math.Round(incomeVal);
                               

                                //FrankingCredits

                                var corporateTaxRate = generalAssumptions.Where(a => a.Type == "CorporateTaxRate").FirstOrDefault();
                                double ctr = Convert.ToDouble(corporateTaxRate.Percentage / 100);

                                obj.FrankingCreditsValues = (obj.IncomeValues * (ctr / (1 - ctr)) * Convert.ToDouble(x.Franked / 100));
                              
                                //pension Income
                                var preservation = preservationAge.OrderBy(a => a.Dob).ToList();
                                var minPendionDD = minimumPensionDrawdown.OrderBy(a => a.Age).ToList();

                                var currentYear = clientDetails.StartDate + i;
                                var pAge = 0;
                                var minRate  = 0;

                                for (var v = 0; v < preservation.Count(); v++)
                                {


                                    if ((v == (preservation.Count() - 1)) && clientDetails.ClientDob >= preservation[v].Dob)
                                    {
                                        pAge = preservation[v].Age;

                                    }
                                    else if (v == 0 && ((clientDetails.ClientDob) <= preservation[v].Dob))
                                    {
                                        pAge = preservation[v].Age;
                                        break;

                                    }
                                    else if ((clientDetails.ClientDob) > preservation[v].Dob && (clientDetails.ClientDob) <= preservation[v + 1].Dob)
                                    {
                                        pAge = preservation[v + 1].Age;
                                        break;

                                    }
                                }
                                //var date1 = new DateTime(clientDetails.StartDate + i, 7, 1);
                                //var date2 = new Date(this.clientDetails.clientDob);
                                //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                                //var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;

                                //var clientAge = Math.round(diffDays * 10) / 10;
                                var today = new DateTime(clientDetails.StartDate + i, 7, 1);
                                var clientAge = today.Year - clientDetails.ClientDob.Year;          
                                if (clientDetails.ClientDob > today.AddYears(-clientAge)) clientAge--;


                                for (var l = 0; l < minPendionDD.Count(); l++)
                                {


                                    if ((l == (minPendionDD.Count() - 1)) && (clientAge >= minPendionDD[l].Age))
                                    {
                                        minRate = minPendionDD[l].MinimumDrawdown;

                                    }
                                    else if (l == 0 && (clientAge <= minPendionDD[l].Age))
                                    {
                                        minRate = minPendionDD[l].MinimumDrawdown;
                                        break;

                                    }
                                    else if (clientAge > minPendionDD[l].Age && clientAge <= minPendionDD[l + 1].Age)
                                    {
                                        minRate = minPendionDD[l + 1].MinimumDrawdown;
                                        break;

                                    }
                                }


                                double minDrawdown = 0;
                                double maxDrawdown = 0;

                                if (clientAge < pAge)
                                {
                                    minDrawdown = 0;
                                    maxDrawdown = 0;
                                }
                                else if (clientAge >= pAge && clientAge < 65)
                                {
                                    if (clientRetirementYear != 0 && clientRetirementYear <= (clientDetails.StartDate + i))
                                    {
                                        minDrawdown = obj.BegValues * (minRate / 100);
                                        maxDrawdown = obj.BegValues + obj.GrowthValues + obj.IncomeValues;

                                    }
                                    else
                                    {
                                        minDrawdown = 0;
                                        maxDrawdown = obj.BegValues * (10 / 100);
                                    }

                                }
                                else if (clientAge >= 65)
                                {
                                    minDrawdown = obj.BegValues * (minRate / 100);
                                    maxDrawdown = obj.BegValues + obj.GrowthValues + obj.IncomeValues;
                                }


                                if (x.PensionRebootFromDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {

                                    if (pensionDrawDown.Count() > 0)
                                    {
                                        var pIncomeSum = 0;
                                        foreach(PensionDetailsViewModel y in pensionDrawDown) {

                                            if (y.FromDateType == "Start")
                                            {
                                                y.FromDate = clientDetails.StartDate;
                                            }
                                            if (y.ToDateType == "End")
                                            {
                                                y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                            }

                                            if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                            {
                                                if (y.ClientId == 0)
                                                {
                                                    if (y.Type == "Minimum")
                                                    {
                                                        obj.PensionIncomeValues = Math.Round(Convert.ToDouble(minDrawdown));
                                                        //pensionIncome[clientDetails.StartDate + i] = minDrawdown;
                                                    }
                                                    else if (y.Type == "Maximum")
                                                    {
                                                        obj.PensionIncomeValues = Math.Round(Convert.ToDouble(maxDrawdown));
                                                        //pensionIncome[clientDetails.StartDate + i] = maxDrawdown;
                                                    }
                                                }
                                                else
                                                {
                                                    obj.PensionIncomeValues = Math.Round(Math.Min(maxDrawdown, Math.Max(minDrawdown, y.Amount)));
                                                    //pensionIncome[clientDetails.StartDate + i] = (Math.min(maxDrawdown, Math.max(minDrawdown, y.amount)));
                                                }
                                              
                                            }

                                        }
                }
                else
                {
                    obj.PensionIncomeValues = 0;
                }


            }
                          else {
                                    obj.PensionIncomeValues = 0;
            }


            if (clientAge >= pAge && clientAge < 60)
            {
                                    obj.PITaxAssessableValues = Math.Round(obj.PensionIncomeValues * TaxableProp);
                                    obj.PITaxExemptValues = Math.Round(obj.PensionIncomeValues - obj.PITaxAssessableValues);
 
            }
            else if (clientAge >= 60)
            {
                                    obj.PITaxAssessableValues = 0;
                                    obj.PITaxExemptValues = Math.Round(obj.PensionIncomeValues);
            }

            //Ending Value
            obj.EndingValues = obj.BegValues + obj.GrowthValues + obj.IncomeValues - obj.PensionIncomeValues;

            ////Ending Value PV
            var inflation =generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
            var inf = inflation.Percentage / 100;
             var k = clientDetails.StartDate + i - new DateTime().Year;
            obj.EndingValuesPv = Math.Round(obj.EndingValues / Math.Pow(Convert.ToDouble(1 + inf), (k)));
                              
                                ////Ending Value - Taxable
                                obj.TaxableEndingValues = Math.Round(obj.EndingValues * TaxableProp);

                                obj.TaxFreeEndingValues = Math.Round(obj.EndingValues - TaxFreeProp);

            //TODO: Verify
            obj.TaxableProp = TaxableProp;
            obj.TaxfreeProp = TaxFreeProp;


         

                                if (json.Pensions.Where(y => y.Owner == x.PensionId).FirstOrDefault() != null)
                                {
                                    json.Pensions[json.Pensions.FindIndex(c => c.Owner == x.PensionId)] = obj;
                                }
                                else
                                {
                                    json.Pensions.Add(obj);
                                }


                            }
                            foreach (PensionViewModel x in pensionPartner)
                            {

                                pensionDrawDown = pensionDD.Where(c => c.PensionId == x.PensionId);
                                if (x.PensionRebootFromType == "Start" || x.PensionRebootFromType == "Existing")
                                {
                                    x.PensionRebootFromDate = clientDetails.StartDate;
                                }
                                else if (x.PensionRebootFromType == "Client Retirement")
                                {
                                    x.PensionRebootFromDate = clientDetails.PartnerRetirementYear - 1;
                                }
                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Retain")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                var obj = json.Pensions.Where(y => y.Owner == x.PensionId).FirstOrDefault();
                                var TaxableProp = 0;
                                var TaxFreeProp = 0;

                                if (obj == null)
                                {
                                    obj = new PensionValue();

                                }
                                else
                                {


                                }

                                obj.Owner = x.PensionId;
                                obj.Type = "Partner";
                                obj.Name = "Opening Value";

                                var previousYearVal = new PensionValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Pensions.Where(y => y.Owner == x.PensionId).FirstOrDefault();
                                }
                                if (x.PensionRebootFromDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {

                                    //BeginningValue
                                    if (m == 1)
                                    {
                                        if (x.PensionRebootFromType == "Existing")
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else
                                        {
                                            obj.BegValues = 0;
                                        }

                                    }
                                    else
                                    {


                                        if ((clientDetails.StartDate + i) == x.PensionRebootFromDate)
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else if ((clientDetails.StartDate + i) < x.PensionRebootFromDate)
                                        {

                                            obj.BegValues = 0;
                                        }
                                        else
                                        {

                                            obj.BegValues = previousYearVal.EndingValues;
                                        }
                                    }

                                    if ((clientDetails.StartDate + i) == x.PensionRebootFromDate || x.PensionRebootFromType == "Existing")
                                    {
                                        //TODO Confirm negative
                                        if (x.TaxableComponent != 0 && x.Value != 0)
                                        {
                                            TaxableProp = (x.TaxableComponent / x.Value);
                                        }
                                        else
                                        {
                                            TaxableProp = 0;
                                        }
                                        TaxFreeProp = 1 - TaxableProp;

                                        obj.TaxableBegValues = Math.Round(Convert.ToDouble(x.TaxableComponent));
                                        obj.TaxFreeBegValues = Math.Round(Convert.ToDouble(x.TaxFreeComponent));
                                    }
                                    else if ((clientDetails.StartDate + i) < x.PensionRebootFromDate)
                                    {

                                        obj.TaxableBegValues = 0;
                                        obj.TaxFreeBegValues = 0;
                                    }
                                    else
                                    {

                                        obj.TaxableBegValues = previousYearVal.TaxableEndingValues;
                                        obj.TaxFreeBegValues = previousYearVal.TaxFreeEndingValues;
                                    }
                                }
                                else
                                {
                                    obj.TaxableBegValues = 0;
                                    obj.TaxFreeBegValues = 0;
                                    obj.BegValues = 0;

                                }




                                ////growth & income
                                var growthUnAdj = Convert.ToDouble(x.Growth / 100) * obj.BegValues;
                                var incomeUnAdj = Convert.ToDouble(x.Income / 100) * obj.BegValues;

                                var ICR = Convert.ToDouble(x.ProductFees / 100) * obj.BegValues;


                                var growthVal = growthUnAdj - (ICR * (growthUnAdj / (growthUnAdj + incomeUnAdj)));
                                var incomeVal = incomeUnAdj - (ICR * (incomeUnAdj / (growthUnAdj + incomeUnAdj)));


                                obj.GrowthValues = Math.Round(growthVal);
                                obj.IncomeValues = Math.Round(incomeVal);


                                //FrankingCredits

                                var corporateTaxRate = generalAssumptions.Where(a => a.Type == "CorporateTaxRate").FirstOrDefault();
                                double ctr = Convert.ToDouble(corporateTaxRate.Percentage / 100);

                                obj.FrankingCreditsValues = (obj.IncomeValues * (ctr / (1 - ctr)) * Convert.ToDouble(x.Franked / 100));

                                //pension Income
                                var preservation = preservationAge.OrderBy(a => a.Dob).ToList();
                                var minPendionDD = minimumPensionDrawdown.OrderBy(a => a.Age).ToList();

                                var currentYear = clientDetails.StartDate + i;
                                var pAge = 0;
                                var minRate = 0;

                                for (var v = 0; v < preservation.Count(); v++)
                                {


                                    if ((v == (preservation.Count() - 1)) && clientDetails.PartnerDob >= preservation[v].Dob)
                                    {
                                        pAge = preservation[v].Age;

                                    }
                                    else if (v == 0 && ((clientDetails.PartnerDob) <= preservation[v].Dob))
                                    {
                                        pAge = preservation[v].Age;
                                        break;

                                    }
                                    else if ((clientDetails.PartnerDob) > preservation[v].Dob && (clientDetails.PartnerDob) <= preservation[v + 1].Dob)
                                    {
                                        pAge = preservation[v + 1].Age;
                                        break;

                                    }
                                }
                                //var date1 = new DateTime(clientDetails.StartDate + i, 7, 1);
                                //var date2 = new Date(this.clientDetails.clientDob);
                                //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                                //var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;

                                //var clientAge = Math.round(diffDays * 10) / 10;
                                var today = new DateTime(clientDetails.StartDate + i, 7, 1);
                                var partnerAge = today.Year - (clientDetails.PartnerDob.HasValue ? clientDetails.PartnerDob.Value.Year : 0);
                                if (clientDetails.PartnerDob > today.AddYears(-partnerAge)) partnerAge--;


                                for (var l = 0; l < minPendionDD.Count(); l++)
                                {


                                    if ((l == (minPendionDD.Count() - 1)) && (partnerAge >= minPendionDD[l].Age))
                                    {
                                        minRate = minPendionDD[l].MinimumDrawdown;

                                    }
                                    else if (l == 0 && (partnerAge <= minPendionDD[l].Age))
                                    {
                                        minRate = minPendionDD[l].MinimumDrawdown;
                                        break;

                                    }
                                    else if (partnerAge > minPendionDD[l].Age && partnerAge <= minPendionDD[l + 1].Age)
                                    {
                                        minRate = minPendionDD[l + 1].MinimumDrawdown;
                                        break;

                                    }
                                }


                                double minDrawdown = 0;
                                double maxDrawdown = 0;

                                if (partnerAge < pAge)
                                {
                                    minDrawdown = 0;
                                    maxDrawdown = 0;
                                }
                                else if (partnerAge >= pAge && partnerAge < 65)
                                {
                                    if (partnerRetirementYear != 0 && partnerRetirementYear <= (clientDetails.StartDate + i))
                                    {
                                        minDrawdown = obj.BegValues * (minRate / 100);
                                        maxDrawdown = obj.BegValues + obj.GrowthValues + obj.IncomeValues;

                                    }
                                    else
                                    {
                                        minDrawdown = 0;
                                        maxDrawdown = obj.BegValues * (10 / 100);
                                    }

                                }
                                else if (partnerAge >= 65)
                                {
                                    minDrawdown = obj.BegValues * (minRate / 100);
                                    maxDrawdown = obj.BegValues + obj.GrowthValues + obj.IncomeValues;
                                }


                                if (x.PensionRebootFromDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {

                                    if (pensionDrawDown.Count() > 0)
                                    {
                                        var pIncomeSum = 0;
                                        foreach (PensionDetailsViewModel y in pensionDrawDown)
                                        {

                                            if (y.FromDateType == "Start")
                                            {
                                                y.FromDate = clientDetails.StartDate;
                                            }
                                            if (y.ToDateType == "End")
                                            {
                                                y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                            }

                                            if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                            {
                                                if (y.ClientId == 0)
                                                {
                                                    if (y.Type == "Minimum")
                                                    {
                                                        obj.PensionIncomeValues = Math.Round(Convert.ToDouble(minDrawdown));
                                                        //pensionIncome[clientDetails.StartDate + i] = minDrawdown;
                                                    }
                                                    else if (y.Type == "Maximum")
                                                    {
                                                        obj.PensionIncomeValues = Math.Round(Convert.ToDouble(maxDrawdown));
                                                        //pensionIncome[clientDetails.StartDate + i] = maxDrawdown;
                                                    }
                                                }
                                                else
                                                {
                                                    obj.PensionIncomeValues = Math.Round(Math.Min(maxDrawdown, Math.Max(minDrawdown, y.Amount)));
                                                    //pensionIncome[clientDetails.StartDate + i] = (Math.min(maxDrawdown, Math.max(minDrawdown, y.amount)));
                                                }

                                            }

                                        }
                                    }
                                    else
                                    {
                                        obj.PensionIncomeValues = 0;
                                    }


                                }
                                else
                                {
                                    obj.PensionIncomeValues = 0;
                                }


                                if (partnerAge >= pAge && partnerAge < 60)
                                {
                                    obj.PITaxAssessableValues = Math.Round(obj.PensionIncomeValues * TaxableProp);
                                    obj.PITaxExemptValues = Math.Round(obj.PensionIncomeValues - obj.PITaxAssessableValues);

                                }
                                else if (partnerAge >= 60)
                                {
                                    obj.PITaxAssessableValues = 0;
                                    obj.PITaxExemptValues = Math.Round(obj.PensionIncomeValues);
                                }

                                //Ending Value
                                obj.EndingValues = obj.BegValues + obj.GrowthValues + obj.IncomeValues - obj.PensionIncomeValues;

                                ////Ending Value PV
                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = inflation.Percentage / 100;
                                var k = clientDetails.StartDate + i - new DateTime().Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / Math.Pow(Convert.ToDouble(1 + inf), (k)));

                                ////Ending Value - Taxable
                                obj.TaxableEndingValues = Math.Round(obj.EndingValues * TaxableProp);

                                obj.TaxFreeEndingValues = Math.Round(obj.EndingValues - TaxFreeProp);

                                //TODO: Verify
                                obj.TaxableProp = TaxableProp;
                                obj.TaxfreeProp = TaxFreeProp;




                                if (json.Pensions.Where(y => y.Owner == x.PensionId).FirstOrDefault() != null)
                                {
                                    json.Pensions[json.Pensions.FindIndex(c => c.Owner == x.PensionId)] = obj;
                                }
                                else
                                {
                                    json.Pensions.Add(obj);
                                }


                            }

                            calculateTotalPensionIncome("TotalPensionIncome", json);
                            calculatePensionIncomeTaxable("PensionIncome-client", "Client", json);
                            calculatePensionIncomeTaxable("PensionIncome-partner", "Partner", json);
                            calculateSuperIncomeTaxOffset("SIncomeTaxOffset-client", "Client", json);
                            calculateSuperIncomeTaxOffset("SIncomeTaxOffset-partner", "Partner", json);

                            foreach (SuperViewModel x in superClient)
                            { // client

                                superSS = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "SS");
                                superPNC = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "PNC");
                                superSpouse = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "Spouse");
                                superLumpSum = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "LumpSum");


                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Retain")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }


                                var obj = json.Supers.Where(y => y.Owner == x.SuperId).FirstOrDefault();
                               

                                if (obj == null)
                                {
                                    obj = new SuperValue();

                                }
                                else
                                {
                                   
                                }


                                obj.Owner = x.SuperId;
                                obj.Type = "Client";
                                obj.Name = "Opening Value";

                                var previousYearVal = new SuperValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Supers.Where(y => y.Owner == x.SuperId).FirstOrDefault();
                                }

                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {

                                    //BeginningValue
                                    if (m == 1)
                                    {
                                        if (x.StartDateType == "Existing")
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else
                                        {
                                            obj.BegValues = 0;
                                        }

                                    }
                                    else
                                    {
                                        if ((clientDetails.StartDate + i) == x.StartDate)
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else if ((clientDetails.StartDate + i) < x.StartDate)
                                        {

                                            obj.BegValues = 0;
                                        }
                                        else
                                        {

                                            obj.BegValues = previousYearVal.EndingValues;
                                        }
                                    }

                                    if ((clientDetails.StartDate + i) == x.StartDate)
                                    {  
                                        obj.TaxableBegValues = Math.Round(Convert.ToDouble(x.TaxableComponent));
                                        obj.TaxFreeBegValues = Math.Round(Convert.ToDouble(x.TaxFreeComponent));
                                    }
                                    else if ((clientDetails.StartDate + i) < x.StartDate)
                                    {

                                        obj.TaxableBegValues = 0;
                                        obj.TaxFreeBegValues = 0;
                                    }
                                    else
                                    {

                                        obj.TaxableBegValues = previousYearVal.TaxableEndingValues;
                                        obj.TaxFreeBegValues = previousYearVal.TaxFreeEndingValues;
                                    }
                                }
                                else
                                {
                                    obj.TaxableBegValues = 0;
                                    obj.TaxFreeBegValues = 0;
                                    obj.BegValues = 0;

                                }


                                ////growth & income
                                var growthUnAdj = Convert.ToDouble(x.Growth / 100) * obj.BegValues;
                                var incomeUnAdj = Convert.ToDouble(x.Income / 100) * obj.BegValues;

                                var ICR = Convert.ToDouble(x.ProductFees / 100) * obj.BegValues;


                                var growthVal = growthUnAdj - (ICR * (growthUnAdj / (growthUnAdj + incomeUnAdj)));
                                var incomeVal = incomeUnAdj - (ICR * (incomeUnAdj / (growthUnAdj + incomeUnAdj)));


                                obj.GrowthValues = Math.Round(growthVal);
                                obj.IncomeValues = Math.Round(incomeVal);


                                //FrankingCredits

                                var corporateTaxRate = generalAssumptions.Where(a => a.Type == "CorporateTaxRate").FirstOrDefault();
                                double ctr = Convert.ToDouble(corporateTaxRate.Percentage / 100);

                                obj.FrankingCreditsValues = (obj.IncomeValues * (ctr / (1 - ctr)) * Convert.ToDouble(x.Franked / 100));
                                

                                //insurance

                                double insuranceVal = 0;

                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    insuranceVal = Convert.ToDouble(x.InsuranceCost);
                                }

                                obj.InsuranceValues = insuranceVal;

                                //Super Guarantee

                                var cc_cap = superAssumptions.Where(a => a.Type == "CC_Cap").FirstOrDefault();
                                var cc_cap_val = cc_cap.Value;

                                var ncc_cap = superAssumptions.Where(a => a.Type == "NCC_Cap").FirstOrDefault();
                                var ncc_cap_val = ncc_cap.Value;

                                var mscb = superAssumptions.Where(a => a.Type == "MSCB").FirstOrDefault();
                                var mscb_val = mscb.Value;


                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    if (x.IncreaseToLimit == "Y")
                                    {

                                        double ContributionSum = 0;
                                        foreach(SuperDetailsViewModel y in superSS) {

                                            if (y.FromDateType == "Start")
                                            {
                                                y.FromDate = clientDetails.StartDate;
                                            }
                                            if (y.ToDateType == "End")
                                            {
                                                y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                            }

                                            if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                            {
                                                var t = Convert.ToDouble(y.Amount);
                                                ContributionSum = ContributionSum + t;
                                            }

                                        }

                                        obj.SgContrValues= Math.Round(cc_cap_val - ContributionSum);
                }
                else
                {
                    //TODO: get SGCRatev-verify
                    double sgcRate = 1;

                    if (x.Sgrate == "SGC")
                    {
                        var sgc = sgcRates.OrderBy(a => a.Year).ToList();
                        var currentYear = clientDetails.StartDate + i;


                        for (var j = 0; j < sgc.Count(); j++)
                        {
                            if ((j == (sgc.Count() - 1)) && ((clientDetails.StartDate + i) >= sgc[j].Year))
                            {
                                sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);

                            }
                            else if (j == 0 && ((clientDetails.StartDate + i) <= sgc[j].Year))
                            {
                                sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);
                                                    break;

                            }
                            else if ((clientDetails.StartDate + i) > sgc[j].Year && (clientDetails.StartDate + i) <= sgc[j + 1].Year)
                            {
                                sgcRate = Convert.ToDouble(sgc[j + 1].Sgcrate1);
                                                    break;

                            }
                        }

                    }
                    else
                    {
                        sgcRate = Convert.ToDouble(x.Sgrate);
                                        }

                    var cEmploymentIncome = clientEmploymentIncome.Where(g => g.Type == "Employment").ToList();

                    double totalcEmploymentIncome = 0;
                                        for (var v = 0; v < cEmploymentIncome.Count(); v++)
                                        {
                                            if (Double.IsNaN(cEmploymentIncome[v].Value))
                                            {
                                                continue;
                                            }
                                            totalcEmploymentIncome += cEmploymentIncome[v].Value;
                                        }





                                        // sgContr[clientDetails.StartDate + i] = Math.min((x.superSalary * (sgcRate / 100)), mscb_val);
                                        obj.SgContrValues = Math.Round(Math.Min((totalcEmploymentIncome * (sgcRate / 100)), mscb_val));

                }
            }
                                else {
                         obj.SgContrValues = 0;
            }

            //Salary Sacrifice

            double SalarySacrificeSum = 0;
            if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
            {
                                    foreach (SuperDetailsViewModel y in superSS)
                                    { // client

                    if (y.FromDateType == "Start")
                    {
                        y.FromDate = clientDetails.StartDate;
                    }
                    if (y.ToDateType == "End")
                    {
                        y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                    }

                    if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                    {
                        if (y.IncreaseToLimit == "Y")
                        {

                            double sgContr = 0;

                                                double sgcRate = 1;

                                                if (x.Sgrate == "SGC")
                                                {
                                                    var sgc = sgcRates.OrderBy(a => a.Year).ToList();
                                                    var currentYear = clientDetails.StartDate + i;


                                                    for (var j = 0; j < sgc.Count(); j++)
                                                    {
                                                        if ((j == (sgc.Count() - 1)) && ((clientDetails.StartDate + i) >= sgc[j].Year))
                                                        {
                                                            sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);

                                                        }
                                                        else if (j == 0 && ((clientDetails.StartDate + i) <= sgc[j].Year))
                                                        {
                                                            sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);
                                                            break;

                                                        }
                                                        else if ((clientDetails.StartDate + i) > sgc[j].Year && (clientDetails.StartDate + i) <= sgc[j + 1].Year)
                                                        {
                                                            sgcRate = Convert.ToDouble(sgc[j + 1].Sgcrate1);
                                                            break;

                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    sgcRate = Convert.ToDouble(x.Sgrate);
                                                }

                                                var cEmploymentIncome = clientEmploymentIncome.Where(g => g.Type == "Employment").ToList();

                                                double totalcEmploymentIncome = 0;
                                                for (var v = 0; v < cEmploymentIncome.Count(); v++)
                                                {
                                                    if (Double.IsNaN(cEmploymentIncome[v].Value))
                                                    {
                                                        continue;
                                                    }
                                                    totalcEmploymentIncome += cEmploymentIncome[v].Value;
                                                }

                                                //sgContr = Math.min((x.superSalary * (sgcRate / 100)), mscb_val);
                                                sgContr = Math.Min((totalcEmploymentIncome * (sgcRate / 100)), mscb_val);
                            SalarySacrificeSum = cc_cap_val - sgContr;
                        }
                        else
                        {
                            var t = Convert.ToDouble(y.Amount);
                            SalarySacrificeSum = SalarySacrificeSum + t;
                        }
                    }

                }

                                    obj.SsContrValues = Math.Round(SalarySacrificeSum);
            }
            else
            {
                obj.SsContrValues = 0;
            }

            //PNC Contribution
            double PNCContributionSum = 0;
            if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
            {
                                    foreach (SuperDetailsViewModel y in superPNC)
                                    { // client

                                        if (y.FromDateType == "Start")
                                        {
                                            y.FromDate = clientDetails.StartDate;
                                        }
                                        if (y.ToDateType == "End")
                                        {
                                            y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                        }

                                        if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                        {
                        if (y.IncreaseToLimit == "Y")
                        {
                            double SpouseSum = 0;
                             foreach (SuperDetailsViewModel b in superSpouse) {

                                if (b.FromDateType == "Start")
                                {
                                    b.FromDate = clientDetails.StartDate;
                                }
                                if (b.ToDateType == "End")
                                {
                                    b.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                                    }

                                if (b.FromDate <= clientDetails.StartDate + i && b.ToDate  >= clientDetails.StartDate + i)
                                {
                                    var t = Convert.ToDouble(b.Amount);
                                                        SpouseSum = SpouseSum + t;
                                }

                            }

                            PNCContributionSum = ncc_cap_val - SpouseSum;

                        }
                        else
                        {
                    var t = Convert.ToDouble(y.Amount);
                    PNCContributionSum = PNCContributionSum + t;
                }
                    }

                }
        obj.PncContrValues = Math.Round(PNCContributionSum);
            }
            else
            {
                                    obj.PncContrValues = 0;
            }

            //Spouse Contribution
            double SpouseContributionSum = 0;
            if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
            {
                                    foreach (SuperDetailsViewModel y in superSpouse)
                                    { // client

                                        if (y.FromDateType == "Start")
                                        {
                                            y.FromDate = clientDetails.StartDate;
                                        }
                                        if (y.ToDateType == "End")
                                        {
                                            y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                        }

                                        if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                    {
                        if (y.IncreaseToLimit == "Y")
                        {
                           

                    double PNCSum = 0;
                    foreach (SuperDetailsViewModel b in superPNC)
                    {

                        if (b.FromDateType == "Start")
                        {
                            b.FromDate = clientDetails.StartDate;
                        }
                        if (b.ToDateType == "End")
                        {
                            b.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                        }

                        if (b.FromDate <= clientDetails.StartDate + i && b.ToDate >= clientDetails.StartDate + i)
                        {
                            var t = Convert.ToDouble(y.Amount);
                            PNCSum = PNCSum + t;
                        }

                    }

                    SpouseContributionSum = ncc_cap_val - PNCSum;

                        }
                        else
                        {
                    var t = Convert.ToDouble(y.Amount);
                    SpouseContributionSum = SpouseContributionSum + t;
                }
                    }

                }
                obj.SpouseContrValues = Math.Round(SpouseContributionSum);
            }
            else
            {
                obj.SpouseContrValues = 0;
            }


            //Lumpsum Withdrawals
            double LumpsumWithdrawalsSum = 0;
            double LumpsumTaxableSum = 0;
            if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
            {
                                    foreach (SuperDetailsViewModel y in superLumpSum)
                                    { // client

                                        if (y.FromDateType == "Start")
                                        {
                                            y.FromDate = clientDetails.StartDate;
                                        }
                                        if (y.ToDateType == "End")
                                        {
                                            y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                        }

                                        if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)                        
                    {
                        var t = Convert.ToDouble(y.Amount);
                                            LumpsumWithdrawalsSum = LumpsumWithdrawalsSum + t;
                    }

                }
                obj.LumpSumValues = LumpsumWithdrawalsSum;


                if (LumpsumWithdrawalsSum > 0)
                {
                    var TaxableBeforLS = obj.TaxableBegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues;
                    //TODO: Add govtContr
                    var ValueBeforeLS = obj.BegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues + obj.PncContrValues + obj.SpouseContrValues;
                    if (TaxableBeforLS != 0 && ValueBeforeLS != 0)
                    {
                                            obj.LumpSumTaxableValues = LumpsumWithdrawalsSum * (TaxableBeforLS / ValueBeforeLS);
                    }
                    else
                    {
                                            obj.LumpSumTaxableValues = 0;
                    }
                }
                else
                {
                    obj.LumpSumTaxableValues = 0;
                }

            }
            else
            {
                obj.LumpSumValues = 0;
                obj.LumpSumTaxableValues = 0;
            }


            //Taxes Payable
            if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
            {
                var earningsTax = superAssumptions.Where(a => a.Type == "EarningsTaxRate").FirstOrDefault();
                var earningsTax_val = earningsTax.Value;

                var ccTaxRate = superAssumptions.Where(a => a.Type == "CCTaxRate").FirstOrDefault();
                                    var ccTaxRate_val = ccTaxRate.Value;

                var addTaxRate = superAssumptions.Where(a => a.Type == "AddTaxRate").FirstOrDefault();
                                    var addTaxRate_val = addTaxRate.Value;

                double ccTax = 0;
                var EarningsTax = (obj.GrowthValues + obj.IncomeValues) * (earningsTax_val / 100);

               
                ccTax = (obj.SgContrValues + obj.SsContrValues) * (ccTaxRate_val / 100);

                                    obj.TaxPayableValues = Math.Round(EarningsTax + ccTax);
                          }
            else
            {
                obj.TaxPayableValues = 0;
            }




                                //Ending Value
                                obj.EndingValues = Math.Round(obj.BegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues + obj.PncContrValues + obj.SpouseContrValues - obj.TaxPayableValues - obj.LumpSumValues);

                                ////Ending Value PV
                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = inflation.Percentage / 100;
                                var k = clientDetails.StartDate + i - new DateTime().Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / Math.Pow(Convert.ToDouble(1 + inf), (k)));

                               
                

                                //Ending Value - Taxable
                                obj.TaxableEndingValues = Math.Round(obj.TaxableBegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues - obj.TaxPayableValues - obj.LumpSumValues);

                                obj.TaxFreeEndingValues = Math.Round(obj.EndingValues - obj.TaxableEndingValues);



            if (json.Supers.Where(y => y.Owner == x.SuperId).FirstOrDefault() != null)
            {
                                    json.Supers[json.Supers.FindIndex(c => c.Owner == x.SuperId)] = obj;
            }
            else
            {
                                    json.Supers.Add(obj);

            }

        }
                            foreach (SuperViewModel x in superPartner)
                            { // client

                                superSS = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "SS");
                                superPNC = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "PNC");
                                superSpouse = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "Spouse");
                                superLumpSum = superDetails.Where(c => c.SuperId == x.SuperId).Where(r => r.Type == "LumpSum");


                                if (x.StartDateType == "Start" || x.StartDateType == "Existing")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Retain")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }


                                var obj = json.Supers.Where(y => y.Owner == x.SuperId).FirstOrDefault();


                                if (obj == null)
                                {
                                    obj = new SuperValue();

                                }
                                else
                                {

                                }


                                obj.Owner = x.SuperId;
                                obj.Type = "Partner";
                                obj.Name = "Opening Value";

                                var previousYearVal = new SuperValue();

                                if (i > 0)
                                {
                                    previousYearVal = masterJson.Where(f => f.Date == new DateTime((clientDetails.StartDate + i) - 1, 7, 1)).FirstOrDefault().Supers.Where(y => y.Owner == x.SuperId).FirstOrDefault();
                                }

                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {

                                    //BeginningValue
                                    if (m == 1)
                                    {
                                        if (x.StartDateType == "Existing")
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else
                                        {
                                            obj.BegValues = 0;
                                        }

                                    }
                                    else
                                    {
                                        if ((clientDetails.StartDate + i) == x.StartDate)
                                        {
                                            obj.BegValues = Math.Round(Convert.ToDouble(x.Value));
                                        }
                                        else if ((clientDetails.StartDate + i) < x.StartDate)
                                        {

                                            obj.BegValues = 0;
                                        }
                                        else
                                        {

                                            obj.BegValues = previousYearVal.EndingValues;
                                        }
                                    }

                                    if ((clientDetails.StartDate + i) == x.StartDate)
                                    {
                                        obj.TaxableBegValues = Math.Round(Convert.ToDouble(x.TaxableComponent));
                                        obj.TaxFreeBegValues = Math.Round(Convert.ToDouble(x.TaxFreeComponent));
                                    }
                                    else if ((clientDetails.StartDate + i) < x.StartDate)
                                    {

                                        obj.TaxableBegValues = 0;
                                        obj.TaxFreeBegValues = 0;
                                    }
                                    else
                                    {

                                        obj.TaxableBegValues = previousYearVal.TaxableEndingValues;
                                        obj.TaxFreeBegValues = previousYearVal.TaxFreeEndingValues;
                                    }
                                }
                                else
                                {
                                    obj.TaxableBegValues = 0;
                                    obj.TaxFreeBegValues = 0;
                                    obj.BegValues = 0;

                                }


                                ////growth & income
                                var growthUnAdj = Convert.ToDouble(x.Growth / 100) * obj.BegValues;
                                var incomeUnAdj = Convert.ToDouble(x.Income / 100) * obj.BegValues;

                                var ICR = Convert.ToDouble(x.ProductFees / 100) * obj.BegValues;


                                var growthVal = growthUnAdj - (ICR * (growthUnAdj / (growthUnAdj + incomeUnAdj)));
                                var incomeVal = incomeUnAdj - (ICR * (incomeUnAdj / (growthUnAdj + incomeUnAdj)));


                                obj.GrowthValues = Math.Round(growthVal);
                                obj.IncomeValues = Math.Round(incomeVal);


                                //FrankingCredits

                                var corporateTaxRate = generalAssumptions.Where(a => a.Type == "CorporateTaxRate").FirstOrDefault();
                                double ctr = Convert.ToDouble(corporateTaxRate.Percentage / 100);

                                obj.FrankingCreditsValues = (obj.IncomeValues * (ctr / (1 - ctr)) * Convert.ToDouble(x.Franked / 100));


                                //insurance

                                double insuranceVal = 0;

                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    insuranceVal = Convert.ToDouble(x.InsuranceCost);
                                }

                                obj.InsuranceValues = insuranceVal;

                                //Super Guarantee

                                var cc_cap = superAssumptions.Where(a => a.Type == "CC_Cap").FirstOrDefault();
                                var cc_cap_val = cc_cap.Value;

                                var ncc_cap = superAssumptions.Where(a => a.Type == "NCC_Cap").FirstOrDefault();
                                var ncc_cap_val = ncc_cap.Value;

                                var mscb = superAssumptions.Where(a => a.Type == "MSCB").FirstOrDefault();
                                var mscb_val = mscb.Value;


                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    if (x.IncreaseToLimit == "Y")
                                    {

                                        double ContributionSum = 0;
                                        foreach (SuperDetailsViewModel y in superSS)
                                        {

                                            if (y.FromDateType == "Start")
                                            {
                                                y.FromDate = clientDetails.StartDate;
                                            }
                                            if (y.ToDateType == "End")
                                            {
                                                y.ToDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                            }

                                            if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                            {
                                                var t = Convert.ToDouble(y.Amount);
                                                ContributionSum = ContributionSum + t;
                                            }

                                        }

                                        obj.SgContrValues = Math.Round(cc_cap_val - ContributionSum);
                                    }
                                    else
                                    {
                                        //TODO: get SGCRatev-verify
                                        double sgcRate = 1;

                                        if (x.Sgrate == "SGC")
                                        {
                                            var sgc = sgcRates.OrderBy(a => a.Year).ToList();
                                            var currentYear = clientDetails.StartDate + i;


                                            for (var j = 0; j < sgc.Count(); j++)
                                            {
                                                if ((j == (sgc.Count() - 1)) && ((clientDetails.StartDate + i) >= sgc[j].Year))
                                                {
                                                    sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);

                                                }
                                                else if (j == 0 && ((clientDetails.StartDate + i) <= sgc[j].Year))
                                                {
                                                    sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);
                                                    break;

                                                }
                                                else if ((clientDetails.StartDate + i) > sgc[j].Year && (clientDetails.StartDate + i) <= sgc[j + 1].Year)
                                                {
                                                    sgcRate = Convert.ToDouble(sgc[j + 1].Sgcrate1);
                                                    break;

                                                }
                                            }

                                        }
                                        else
                                        {
                                            sgcRate = Convert.ToDouble(x.Sgrate);
                                        }

                                        var pEmploymentIncome = clientEmploymentIncome.Where(g => g.Type == "Employment").ToList();

                                        double totalpEmploymentIncome = 0;
                                        for (var v = 0; v < pEmploymentIncome.Count(); v++)
                                        {
                                            if (Double.IsNaN(pEmploymentIncome[v].Value))
                                            {
                                                continue;
                                            }
                                            totalpEmploymentIncome += pEmploymentIncome[v].Value;
                                        }





                                        // sgContr[clientDetails.StartDate + i] = Math.min((x.superSalary * (sgcRate / 100)), mscb_val);
                                        obj.SgContrValues = Math.Round(Math.Min((totalpEmploymentIncome * (sgcRate / 100)), mscb_val));

                                    }
                                }
                                else
                                {
                                    obj.SgContrValues = 0;
                                }

                                //Salary Sacrifice

                                double SalarySacrificeSum = 0;
                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    foreach (SuperDetailsViewModel y in superSS)
                                    { // client

                                        if (y.FromDateType == "Start")
                                        {
                                            y.FromDate = clientDetails.StartDate;
                                        }
                                        if (y.ToDateType == "End")
                                        {
                                            y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                        }

                                        if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                        {
                                            if (y.IncreaseToLimit == "Y")
                                            {

                                                double sgContr = 0;

                                                double sgcRate = 1;

                                                if (x.Sgrate == "SGC")
                                                {
                                                    var sgc = sgcRates.OrderBy(a => a.Year).ToList();
                                                    var currentYear = clientDetails.StartDate + i;


                                                    for (var j = 0; j < sgc.Count(); j++)
                                                    {
                                                        if ((j == (sgc.Count() - 1)) && ((clientDetails.StartDate + i) >= sgc[j].Year))
                                                        {
                                                            sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);

                                                        }
                                                        else if (j == 0 && ((clientDetails.StartDate + i) <= sgc[j].Year))
                                                        {
                                                            sgcRate = Convert.ToDouble(sgc[j].Sgcrate1);
                                                            break;

                                                        }
                                                        else if ((clientDetails.StartDate + i) > sgc[j].Year && (clientDetails.StartDate + i) <= sgc[j + 1].Year)
                                                        {
                                                            sgcRate = Convert.ToDouble(sgc[j + 1].Sgcrate1);
                                                            break;

                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    sgcRate = Convert.ToDouble(x.Sgrate);
                                                }

                                                var pEmploymentIncome = partnerEmploymentIncome.Where(g => g.Type == "Employment").ToList();

                                                double totalpEmploymentIncome = 0;
                                                for (var v = 0; v < pEmploymentIncome.Count(); v++)
                                                {
                                                    if (Double.IsNaN(pEmploymentIncome[v].Value))
                                                    {
                                                        continue;
                                                    }
                                                    totalpEmploymentIncome += pEmploymentIncome[v].Value;
                                                }

                                                //sgContr = Math.min((x.superSalary * (sgcRate / 100)), mscb_val);
                                                sgContr = Math.Min((totalpEmploymentIncome * (sgcRate / 100)), mscb_val);
                                                SalarySacrificeSum = cc_cap_val - sgContr;
                                            }
                                            else
                                            {
                                                var t = Convert.ToDouble(y.Amount);
                                                SalarySacrificeSum = SalarySacrificeSum + t;
                                            }
                                        }

                                    }

                                    obj.SsContrValues = Math.Round(SalarySacrificeSum);
                                }
                                else
                                {
                                    obj.SsContrValues = 0;
                                }

                                //PNC Contribution
                                double PNCContributionSum = 0;
                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    foreach (SuperDetailsViewModel y in superPNC)
                                    { // client

                                        if (y.FromDateType == "Start")
                                        {
                                            y.FromDate = clientDetails.StartDate;
                                        }
                                        if (y.ToDateType == "End")
                                        {
                                            y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                        }

                                        if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                        {
                                            if (y.IncreaseToLimit == "Y")
                                            {
                                                double SpouseSum = 0;
                                                foreach (SuperDetailsViewModel b in superSpouse)
                                                {

                                                    if (b.FromDateType == "Start")
                                                    {
                                                        b.FromDate = clientDetails.StartDate;
                                                    }
                                                    if (b.ToDateType == "End")
                                                    {
                                                        b.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                                    }

                                                    if (b.FromDate <= clientDetails.StartDate + i && b.ToDate >= clientDetails.StartDate + i)
                                                    {
                                                        var t = Convert.ToDouble(b.Amount);
                                                        SpouseSum = SpouseSum + t;
                                                    }

                                                }

                                                PNCContributionSum = ncc_cap_val - SpouseSum;

                                            }
                                            else
                                            {
                                                var t = Convert.ToDouble(y.Amount);
                                                PNCContributionSum = PNCContributionSum + t;
                                            }
                                        }

                                    }
                                    obj.PncContrValues = Math.Round(PNCContributionSum);
                                }
                                else
                                {
                                    obj.PncContrValues = 0;
                                }

                                //Spouse Contribution
                                double SpouseContributionSum = 0;
                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    foreach (SuperDetailsViewModel y in superSpouse)
                                    { // client

                                        if (y.FromDateType == "Start")
                                        {
                                            y.FromDate = clientDetails.StartDate;
                                        }
                                        if (y.ToDateType == "End")
                                        {
                                            y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                        }

                                        if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                        {
                                            if (y.IncreaseToLimit == "Y")
                                            {


                                                double PNCSum = 0;
                                                foreach (SuperDetailsViewModel b in superPNC)
                                                {

                                                    if (b.FromDateType == "Start")
                                                    {
                                                        b.FromDate = clientDetails.StartDate;
                                                    }
                                                    if (b.ToDateType == "End")
                                                    {
                                                        b.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                                    }

                                                    if (b.FromDate <= clientDetails.StartDate + i && b.ToDate >= clientDetails.StartDate + i)
                                                    {
                                                        var t = Convert.ToDouble(y.Amount);
                                                        PNCSum = PNCSum + t;
                                                    }

                                                }

                                                SpouseContributionSum = ncc_cap_val - PNCSum;

                                            }
                                            else
                                            {
                                                var t = Convert.ToDouble(y.Amount);
                                                SpouseContributionSum = SpouseContributionSum + t;
                                            }
                                        }

                                    }
                                    obj.SpouseContrValues = Math.Round(SpouseContributionSum);
                                }
                                else
                                {
                                    obj.SpouseContrValues = 0;
                                }


                                //Lumpsum Withdrawals
                                double LumpsumWithdrawalsSum = 0;
                                double LumpsumTaxableSum = 0;
                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    foreach (SuperDetailsViewModel y in superLumpSum)
                                    { // client

                                        if (y.FromDateType == "Start")
                                        {
                                            y.FromDate = clientDetails.StartDate;
                                        }
                                        if (y.ToDateType == "End")
                                        {
                                            y.ToDate = clientDetails.StartDate + (clientDetails.Period - 1);
                                        }

                                        if (y.FromDate <= clientDetails.StartDate + i && y.ToDate >= clientDetails.StartDate + i)
                                        {
                                            var t = Convert.ToDouble(y.Amount);
                                            LumpsumWithdrawalsSum = LumpsumWithdrawalsSum + t;
                                        }

                                    }
                                    obj.LumpSumValues = LumpsumWithdrawalsSum;


                                    if (LumpsumWithdrawalsSum > 0)
                                    {
                                        var TaxableBeforLS = obj.TaxableBegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues;
                                        //TODO: Add govtContr
                                        var ValueBeforeLS = obj.BegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues + obj.PncContrValues + obj.SpouseContrValues;
                                        if (TaxableBeforLS != 0 && ValueBeforeLS != 0)
                                        {
                                            obj.LumpSumTaxableValues = LumpsumWithdrawalsSum * (TaxableBeforLS / ValueBeforeLS);
                                        }
                                        else
                                        {
                                            obj.LumpSumTaxableValues = 0;
                                        }
                                    }
                                    else
                                    {
                                        obj.LumpSumTaxableValues = 0;
                                    }

                                }
                                else
                                {
                                    obj.LumpSumValues = 0;
                                    obj.LumpSumTaxableValues = 0;
                                }


                                //Taxes Payable
                                if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                {
                                    var earningsTax = superAssumptions.Where(a => a.Type == "EarningsTaxRate").FirstOrDefault();
                                    var earningsTax_val = earningsTax.Value;

                                    var ccTaxRate = superAssumptions.Where(a => a.Type == "CCTaxRate").FirstOrDefault();
                                    var ccTaxRate_val = ccTaxRate.Value;

                                    var addTaxRate = superAssumptions.Where(a => a.Type == "AddTaxRate").FirstOrDefault();
                                    var addTaxRate_val = addTaxRate.Value;

                                    double ccTax = 0;
                                    var EarningsTax = (obj.GrowthValues + obj.IncomeValues) * (earningsTax_val / 100);


                                    ccTax = (obj.SgContrValues + obj.SsContrValues) * (ccTaxRate_val / 100);

                                    obj.TaxPayableValues = Math.Round(EarningsTax + ccTax);
                                }
                                else
                                {
                                    obj.TaxPayableValues = 0;
                                }




                                //Ending Value
                                obj.EndingValues = Math.Round(obj.BegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues + obj.PncContrValues + obj.SpouseContrValues - obj.TaxPayableValues - obj.LumpSumValues);

                                ////Ending Value PV
                                var inflation = generalAssumptions.Where(a => a.Type == "Inflation").FirstOrDefault();
                                var inf = inflation.Percentage / 100;
                                var k = clientDetails.StartDate + i - new DateTime().Year;
                                obj.EndingValuesPv = Math.Round(obj.EndingValues / Math.Pow(Convert.ToDouble(1 + inf), (k)));




                                //Ending Value - Taxable
                                obj.TaxableEndingValues = Math.Round(obj.TaxableBegValues + obj.GrowthValues + obj.IncomeValues - obj.InsuranceValues + obj.SgContrValues + obj.SsContrValues - obj.TaxPayableValues - obj.LumpSumValues);

                                obj.TaxFreeEndingValues = Math.Round(obj.EndingValues - obj.TaxableEndingValues);



                                if (json.Supers.Where(y => y.Owner == x.SuperId).FirstOrDefault() != null)
                                {
                                    json.Supers[json.Supers.FindIndex(c => c.Owner == x.SuperId)] = obj;
                                }
                                else
                                {
                                    json.Supers.Add(obj);

                                }

                            }

                            calculateTotalLumpSumWithdrawals("TotalLumpSumWithdrawals", json);
                            calculateTotalSalarySacrificeContribution("TotalSalarySacrificeContributions", json);
                            calculateTotalPNCContibution("TotalPNCContributions", json);
                            calculateTotalSpouseContibution("TotalSpouseContributions", json);
                            calculateLumpSumTax("LumpSum-client", "Client", json);
                            calculateSalarySacrificeTax("SalarySacrifice-client", "Client", json);
                            calculateLumpSumTax("LumpSum-partner", "Partner", json);
                            calculateSalarySacrificeTax("SalarySacrifice-partner", "Partner", json);

                            foreach (CashFlowViewModel x in EPRTClient)
                            { // client

                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }

                                var obj = json.ClientDeductions.Where(y => (y.Id == x.CflowId && y.Type != "Joint")).FirstOrDefault();
                              
                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {
                                    
                                }
                                obj.Owner = x.Owner;
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;
                                obj.Type = "";


                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                        }
                                        else
                                        {
                                           obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));
                                        }
                                        j++;
                                    }
                                    else
                                    {
                                        obj.Value = 0;
                                    }
                                    
                                    obj.Increment = j;
                                }

                                if (json.ClientDeductions.Where(y => (y.Id == x.CflowId && y.Type != "Joint")).FirstOrDefault() != null)
                                {
                                    json.ClientDeductions[json.ClientDeductions.FindIndex(c => (c.Id == x.CflowId && c.Type != "Joint"))] = obj;
                                }
                                else
                                {
                                    json.ClientDeductions.Add(obj);
                                }

                            }
                            foreach (CashFlowViewModel x in EPRTPartner)
                            { // client

                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                var obj = json.PartnerDeductions.Where(y => (y.Id == x.CflowId && y.Type != "Joint")).FirstOrDefault();

                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {

                                }
                                obj.Owner = x.Owner;
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;
                                obj.Type = "";


                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)));
                                        }
                                        j++;
                                    }
                                    else
                                    {
                                        obj.Value = 0;
                                    }

                                    obj.Increment = j;
                                }

                                if (json.PartnerDeductions.Where(y => (y.Id == x.CflowId && y.Type != "Joint")).FirstOrDefault() != null)
                                {
                                    json.PartnerDeductions[json.PartnerDeductions.FindIndex(c => (c.Id == x.CflowId && c.Type != "Joint"))] = obj;
                                }
                                else
                                {
                                    json.PartnerDeductions.Add(obj);
                                }

                            }
                            foreach (CashFlowViewModel x in EPRTJoint)
                            { // client

                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                var obj = json.ClientDeductions.Where(y => (y.Id == x.CflowId && y.Type == "Joint")).FirstOrDefault();

                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {

                                }
                                obj.Owner = "Client";
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;
                                obj.Type = "Joint";


                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value/2));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round((x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j)))/2);
                                        }
                                        j++;
                                    }
                                    else
                                    {
                                        obj.Value = 0;
                                    }

                                    obj.Increment = j;
                                }

                                if (json.ClientDeductions.Where(y => (y.Id == x.CflowId && y.Type == "Joint")).FirstOrDefault() != null)
                                {
                                    json.ClientDeductions[json.ClientDeductions.FindIndex(y => (y.Id == x.CflowId && y.Type == "Joint"))] = obj;
                                }
                                else
                                {
                                    json.ClientDeductions.Add(obj);
                                }

                            }
                            foreach (CashFlowViewModel x in EPRTJoint)
                            { // client

                                if (x.StartDateType == "Start")
                                {
                                    x.StartDate = clientDetails.StartDate;
                                }
                                else if (x.StartDateType == "Client Retirement")
                                {
                                    x.StartDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.StartDateType == "Partner Retirement")
                                {
                                    x.StartDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                if (x.EndDateType == "End")
                                {
                                    x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
                                }
                                else if (x.EndDateType == "Client Retirement")
                                {
                                    x.EndDate = clientDetails.ClientRetirementYear - 1;
                                }
                                else if (x.EndDateType == "Partner Retirement")
                                {
                                    x.EndDate = clientDetails.PartnerRetirementYear - 1;
                                }

                                var obj = json.PartnerDeductions.Where(y => (y.Id == x.CflowId && y.Type == "Joint")).FirstOrDefault();

                                var j = 0;
                                if (obj == null)
                                {
                                    obj = new Income();
                                    j = 0;
                                }
                                else
                                {

                                }
                                obj.Owner = "Partner";
                                obj.Name = x.Cfname;
                                obj.Id = x.CflowId;
                                obj.Type = "Joint";


                                if (q == 0)
                                {
                                    if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
                                    {
                                        if (j == 0)
                                        {
                                            obj.Value = Math.Round(Convert.ToDouble(x.Value / 2));

                                        }
                                        else
                                        {
                                            obj.Value = Math.Round((x.Value * (Math.Pow(Convert.ToDouble(1 + x.Indexation / 100), j))) / 2);
                                        }
                                        j++;
                                    }
                                    else
                                    {
                                        obj.Value = 0;
                                    }

                                    obj.Increment = j;
                                }

                                if (json.PartnerDeductions.Where(y => (y.Id == x.CflowId && y.Type == "Joint")).FirstOrDefault() != null)
                                {
                                    json.PartnerDeductions[json.PartnerDeductions.FindIndex(y => (y.Id == x.CflowId && y.Type == "Joint"))] = obj;
                                }
                                else
                                {
                                    json.PartnerDeductions
.Add(obj);
                                }

                            }

                            calculateTotalTaxIncome("TotalTax-client", "Client", json);
                            calculateCapitalLossAdjustment("CLA-client", "Client", json);
                            calculateTotalAssessibleIncome("ClientAssessibleIncome", "TotalTax-client", "Client", json);
                            calculateClientTotalDeductions("ClientDeductions", "Client", json);
                            calculateClientTaxableIncome(json);
                            calculateLowIncomeTO("ClientLowIncomeTO", "ClientTaxableIncome", "Client", json);
                            calculateRefundableTaxOffset("ClientFrankingCredits", "Client", json);

                            calculateClientTotalNRTaxOffset("ClientTotalTO", "ClientLowIncomeTO", json);
                            calculateGrossTax("ClientGrossTax", "ClientTaxableIncome", "Client", json);
                            calculateMedicareLevy("ClientMedicareLevy", "ClientTaxableIncome", "Client", json);

                            calculateTaxPayableNonRefundable("ClientTPNonRefundable", "ClientGrossTax", "ClientTotalTO", "Client", json);
                            calculateTaxPayableRefundable("ClientTPRefundable", "ClientTPNonRefundable", "ClientFrankingCredits", "Client", json);
                            calculateTotalTaxesPayable("ClientTotalTaxPayable", "ClientTPRefundable", "ClientMedicareLevy", "Client", json);
                            calculateAverageTaxRate("ClientAverageTaxRate", "ClientTotalTaxPayable", "ClientAssessibleIncome", "Client", json);
                            calculateMarginalTaxRate("ClientMarginalTaxRate", "ClientTaxableIncome", "Client", json);


                            calculateTotalTaxIncome("TotalTax-partner", "Partner", json);

                            calculateCapitalLossAdjustment("CLA-partner", "Partner", json);
                            calculateTotalAssessibleIncome("PartnerAssessibleIncome", "TotalTax-partner", "Partner", json);
                            calculatePartnerTotalDeductions("PartnerDeductions", "Partner", json);
                            calculatePartnerTaxableIncome(json);
                            calculateLowIncomeTO("PartnerLowIncomeTO", "PartnerTaxableIncome", "Partner", json);
                            calculateRefundableTaxOffset("PartnerFrankingCredits", "Partner", json);

                            calculatePartnerTotalNRTaxOffset("PartnerTotalTO", "PartnerLowIncomeTO", json);
                            calculateGrossTax("PartnerGrossTax", "PartnerTaxableIncome", "Partner", json);
                            calculateMedicareLevy("PartnerMedicareLevy", "PartnerTaxableIncome", "Partner", json);

                            calculateTaxPayableNonRefundable("PartnerTPNonRefundable", "PartnerGrossTax", "PartnerTotalTO", "Partner", json);
                            calculateTaxPayableRefundable("PartnerTPRefundable", "PartnerTPNonRefundable", "PartnerFrankingCredits", "Partner", json);
                            calculateTotalTaxesPayable("PartnerTotalTaxPayable", "PartnerTPRefundable", "PartnerMedicareLevy", "Partner", json);
                            calculateAverageTaxRate("PartnerAverageTaxRate", "PartnerTotalTaxPayable", "PartnerAssessibleIncome", "Partner", json);
                            calculateMarginalTaxRate("PartnerMarginalTaxRate", "PartnerTaxableIncome", "Partner", json);
                            calculateTotalIncomeTaxPayable("TotalITPayable", json);

                            calculateTotalIncome("Total-client", "Client", json);
                            calculateTotalIncome("Total-partner", "Partner", json);
                            calculateTotalExpenditure("Total-client", "Client", json);
                            calculateTotalExpenditure("Total-partner", "Partner", json);
                            calculateTotalExpenditure("Total-joint", "Joint", json);

                            calculateTotalInflows(json);
                            calculateTotalOutflows(json);
                            calculateNetCashflow(json, q, highestVal);
                            calculateNetAsset(json);

                        }


                        masterJson.Add(json);
                        m++;
                    }

                    finalResult = Json(masterJson);

                }

                return finalResult;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public static T DeepCopy<T>(T obj)

        {

            if (!typeof(T).IsSerializable)

            {

                throw new Exception("The source object must be serializable");

            }

            if (Object.ReferenceEquals(obj, null))

            {

                throw new Exception("The source object must not be null");

            }

            T result = default(T);

            using (var memoryStream = new MemoryStream())

            {

                var formatter = new BinaryFormatter();

                formatter.Serialize(memoryStream, obj);

                memoryStream.Seek(0, SeekOrigin.Begin);

                result = (T)formatter.Deserialize(memoryStream);

                memoryStream.Close();

            }

            return result;

        }

        private void calculateTotalLASaleProceeds(string owner, Projection json)
        {
                var total = json.LATotals.Where(y => y.Owner == owner).FirstOrDefault();
             // var totalVal: any = { };
                if (total == null)
                {
                total = new Common();
                }
                else
                {
                   // totalVal = total.values;
                }
                total.Owner = owner;
                total.Name = "Lifestyle Assets";

                var sum = 0;
                foreach(Lifestyle x in json.Lifestyles) { 
                    if (x.Owner == "Client" || x.Owner == "Partner" || x.Owner == "Joint")
                    {
                        if (x.SaleOfAssetValue != 0)
                        {
                            var t = Convert.ToInt32(x.SaleOfAssetValue);
                            sum = sum + t;
                        }
                    }
                }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.LATotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.LATotals[json.LATotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.LATotals.Add(total);

            }
    }
       private void calculateTotalLAPropertyExpenses(string owner, Projection json)
        {
            var total = json.LATotals.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Lifestyle Assets";

            var sum = 0;
            foreach (Lifestyle x in json.Lifestyles)
            {
                if (x.Owner == "Client" || x.Owner == "Partner" || x.Owner == "Joint")
                {
                    if (x.PurchaseOfAssetValue != 0)
                    {
                        var t = Convert.ToInt32(x.PurchaseOfAssetValue);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.LATotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.LATotals[json.LATotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.LATotals.Add(total);

            }
        }

       private void calculateTotalInvestmentPaidOut(string owner, Projection json)
        {
            var total = json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Financial Assets";

            double sum = 0;
            foreach (InvestmentValue x in json.Investments)
            {
                if (x.Type == "Client" || x.Type == "Partner" || x.Type == "Joint")
                {
                    if (x.IncomePaidOutValues != 0)
                    {
                        var t = Convert.ToDouble(x.IncomePaidOutValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.FaTotals[json.FaTotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.FaTotals.Add(total);

            }
        }
       private void calculateTotalInvestmentWithdrawals(string owner, Projection json)
        {
            var total = json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Financial Assets";

            double sum = 0;
            foreach (InvestmentValue x in json.Investments)
            {
                if (x.Type == "Client" || x.Type == "Partner" || x.Type == "Joint")
                {
                    if (x.RegularWithdrawalsValues != 0)
                    {
                        var t = Convert.ToDouble(x.RegularWithdrawalsValues);
                        sum = sum + t;
                    }

                    if (x.SaleOfAssetValues != 0)
                    {
                        var t = Convert.ToDouble(x.SaleOfAssetValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.FaTotals[json.FaTotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.FaTotals.Add(total);

            }
        }
       private void calculateTotalInvestmentContributions(string owner, Projection json)
        {
            var total = json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Financial Assets";

            double sum = 0;
            foreach (InvestmentValue x in json.Investments)
            {
                if (x.Type == "Client" || x.Type == "Partner" || x.Type == "Joint")
                {
                    if (x.RegularContributionsValues != 0)
                    {
                        var t = Convert.ToDouble(x.RegularContributionsValues);
                        sum = sum + t;
                    }

                    if (x.PurchaseOfAssetValues != 0)
                    {
                        var t = Convert.ToDouble(x.PurchaseOfAssetValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.FaTotals[json.FaTotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.FaTotals.Add(total);

            }
        }
       private void calculateTotalInvestmentEarnings(string owner, string filter, Projection json)
        {
            var total = json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Investment Earnings";

            double sum = 0;
            double jointSum = 0;
            foreach (InvestmentValue x in json.Investments)
            {
                if (x.Type == filter)
                {
                    if (x.IncomePaidOutValues != 0)
                    {
                        var t = Convert.ToDouble(x.IncomePaidOutValues);
                        sum = sum + t;
                    }

                }

                if (x.Type == "Joint")
                {
                    if (x.IncomePaidOutValues != 0)
                    {
                        var t = Convert.ToDouble(x.IncomePaidOutValues);
                        jointSum = jointSum + t;
                    }

                }
            }

            foreach (PropertyValue x in json.Properties)
            {
                if (x.Type == filter)
                {
                    if (x.RentValues != 0)
                    {
                        var t = Convert.ToDouble(x.RentValues);
                        sum = sum + t;
                    }

                }

                if (x.Type == "Joint")
                {
                    if (x.RentValues != 0)
                    {
                        var t = Convert.ToDouble(x.RentValues);
                        jointSum = jointSum + t;
                    }

                }
            }

            if (sum > 0 || jointSum > 0)
            {
                total.Value = Math.Round(sum + (jointSum / 2));

            }
            else
            {
                total.Value = 0;
            }

            if (json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.FaTotals[json.FaTotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.FaTotals.Add(total);

            }
        }
       private void calculateRealizedCGFA(string owner,string filter, Projection json)
        {
            var total = json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Financial Assets";

            double sum = 0;
            double jointSum = 0;
            foreach (InvestmentValue x in json.Investments)
            {
                if (x.Type == filter)
                {
                    if (x.RealCgValues != 0)
                    {
                        var t = Convert.ToDouble(x.RealCgValues);
                        sum = sum + t;
                    }

                }

                if (x.Type == "Joint")
                {
                    if (x.RealCgValues != 0)
                    {
                        var t = Convert.ToDouble(x.RealCgValues);
                        jointSum = jointSum + t;
                    }

                }
            }


            if (sum > 0 || jointSum > 0)
            {
                total.Value = Math.Round(sum + (jointSum/2));

            }
            else
            {
                total.Value = 0;
            }

            if (json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.FaTotals[json.FaTotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.FaTotals.Add(total);

            }
        }
       private void calculateFrankingCredits(string owner, string filter, Projection json)
        {
            var total = json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Franking Credits";

            double sum = 0;
            double jointSum = 0;
            foreach (InvestmentValue x in json.Investments)
            {
                if (x.Type == filter)
                {
                    if (x.FrankingCreditsValues != 0)
                    {
                        var t = Convert.ToDouble(x.FrankingCreditsValues);
                        sum = sum + t;
                    }

                }

                if (x.Type == "Joint")
                {
                    if (x.FrankingCreditsValues != 0)
                    {
                        var t = Convert.ToDouble(x.FrankingCreditsValues);
                        jointSum = jointSum + t;
                    }

                }
            }


            if (sum > 0 || jointSum > 0)
            {
                total.Value = Math.Round(sum + (jointSum / 2));

            }
            else
            {
                total.Value = 0;
            }

            if (json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.FaTotals[json.FaTotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.FaTotals.Add(total);

            }
        }

       private void calculateTotalRent(string owner, Projection json)
        {
            var total = json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Rent";

            double sum = 0;
            foreach (PropertyValue x in json.Properties)
            {
                if (x.Type == "Client" || x.Type == "Partner" || x.Type == "Joint")
                {
                    if (x.RentValues != 0)
                    {
                        var t = Convert.ToDouble(x.RentValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PropertiesTotal[json.PropertiesTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PropertiesTotal.Add(total);

            }
        }
       private void calculateTotalSaleProceeds(string owner, Projection json)
        {
            var total = json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Sale Proceeds";

            double sum = 0;
            foreach (PropertyValue x in json.Properties)
            {
                if (x.Type == "Client" || x.Type == "Partner" || x.Type == "Joint")
                {
                    if (x.PropertySaleValues != 0)
                    {
                        var t = Convert.ToDouble(x.PropertySaleValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PropertiesTotal[json.PropertiesTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PropertiesTotal.Add(total);

            }
        }
       private void calculateTotalPropertyExpenses(string owner, Projection json)
        {
            var total = json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Property";

            double sum = 0;
            foreach (PropertyValue x in json.Properties)
            {
                if (x.Type == "Client" || x.Type == "Partner" || x.Type == "Joint")
                {
                    if (x.ExpensesValues != 0)
                    {
                        var t = Convert.ToDouble(x.ExpensesValues);
                        sum = sum + t;
                    }

                    if (x.PropertyPurchaseValues != 0)
                    {
                        var t = Convert.ToDouble(x.PropertyPurchaseValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PropertiesTotal[json.PropertiesTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PropertiesTotal.Add(total);

            }
        }
       private void calculateInvestmentPropertyExpenses(string owner, string filter, Projection json)
        {
            var total = json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Investment property expenses";

            double sum = 0;
            double jointSum = 0;
            foreach (PropertyValue x in json.Properties)
            {
                if (x.Type == filter)
                {
                    if (x.ExpensesValues != 0)
                    {
                        var t = Convert.ToDouble(x.ExpensesValues);
                        sum = sum + t;
                    }

                }

                if (x.Type == "Joint")
                {
                    if (x.ExpensesValues != 0)
                    {
                        var t = Convert.ToDouble(x.ExpensesValues);
                        jointSum = jointSum + t;
                    }

                }
            }

           

            if (sum > 0 || jointSum > 0)
            {
                total.Value = Math.Round(sum + (jointSum / 2));

            }
            else
            {
                total.Value = 0;
            }

            if (json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PropertiesTotal[json.PropertiesTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PropertiesTotal.Add(total);

            }
        }
       private void calculateRealizedCGP(string owner, string filter, Projection json, int year)
        {
            var total = json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Realized CG";

            double sum = 0;
            double jointSum = 0;
            foreach (PropertyValue x in json.Properties)
            {
                if (x.Type == filter)
                {
                    if (x.StartDateType == "Existing")
                    {
                        if (x.RealCgValues != 0)
                        {
                            var t = Convert.ToDouble(x.RealCgValues);
                            sum = sum + (t / 2);
                        }
                    }
                    else
                    {
                        //Verify
                        if ((year - x.PropPurchase) > 1)
                        {
                            if (x.RealCgValues != 0)
                            {
                                var t = Convert.ToDouble(x.RealCgValues);
                                sum = sum + (t / 2);
                            }
                        }
                        else
                        {
                            if (x.RealCgValues != 0)
                            {
                                var t = Convert.ToDouble(x.RealCgValues);
                                sum = sum + t;
                            }
                        }
                    }

                }

                if (x.Type == "Joint")
                {
                    if (x.StartDateType == "Existing")
                    {
                        if (x.RealCgValues != 0)
                        {
                            var t = Convert.ToDouble(x.RealCgValues);
                            jointSum = jointSum + (t / 2);
                        }
                    }
                    else
                    {
                        //Verify
                        if ((year - x.PropPurchase) > 1)
                        {
                            if (x.RealCgValues != 0)
                            {
                                var t = Convert.ToDouble(x.RealCgValues);
                                jointSum = jointSum + (t / 2);
                            }
                        }
                        else
                        {
                            if (x.RealCgValues != 0)
                            {
                                var t = Convert.ToDouble(x.RealCgValues);
                                jointSum = jointSum + t;
                            }
                        }
                    }

                }
            }



            if (sum > 0 || jointSum > 0)
            {
                total.Value = Math.Round(sum + (jointSum / 2));

            }
            else
            {
                total.Value = 0;
            }

            if (json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PropertiesTotal[json.PropertiesTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PropertiesTotal.Add(total);

            }
        }

       private void calculateTotalDebtRepayment(string owner, Projection json)
        {
            var total = json.LBTotal.Where(y => y.Owner == owner).FirstOrDefault();
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Debt Repayment";

            double sum = 0;
            foreach (LiabilityValue x in json.Liabilities)
            {
                if (x.Type == "Client" || x.Type == "Partner" || x.Type == "Joint")
                {
                    if (x.RepmtValues != 0)
                    {
                        var t = Convert.ToDouble(x.RepmtValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.LBTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.LBTotal[json.LBTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.LBTotal.Add(total);

            }
        }
       private void calculateAccruedLiabilities(string owner, string filter, Projection json)
        {
            var total = json.LBTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Accrued liability interest";

            double sum = 0;
            double jointSum = 0;
            foreach (LiabilityValue x in json.Liabilities)
            {
                if (x.Type == filter)
                {
                    if (x.AccruedInterestValues != 0)
                    {
                        double t = 0;
                        if (x.Deductibility > 0)
                        {
                            t = (x.AccruedInterestValues) * (x.Deductibility / 100);
                        }
                        else
                        {
                            t = 0;
                        }

                        sum = sum + t;
                    }

                }

                if (x.Type == "Joint")
                {
                    double t = 0;
                    if (x.Deductibility > 0)
                    {
                        t = (x.AccruedInterestValues) * (x.Deductibility / 100);
                    }
                    else
                    {
                        t = 0;
                    }

                    jointSum = jointSum + t;

                }
            }

         

            if (sum > 0 || jointSum > 0)
            {
                total.Value = Math.Round(sum + (jointSum / 2));

            }
            else
            {
                total.Value = 0;
            }

            if (json.LBTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.LBTotal[json.LBTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.LBTotal.Add(total);

            }
        }

       private void calculateTotalPensionIncome(string owner, Projection json)
        {
            var total = json.PensionTotal.Where(y => y.Owner == owner).FirstOrDefault();
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Pension Income";

            double sum = 0;
            foreach (PensionValue x in json.Pensions)
            {
                if (x.Type == "Client" || x.Type == "Partner")
                {
                    if (x.PensionIncomeValues != 0)
                    {
                        var t = Convert.ToDouble(x.PensionIncomeValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.PensionTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PensionTotal[json.PensionTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PensionTotal.Add(total);

            }
        }
       private void calculatePensionIncomeTaxable(string owner, string filter, Projection json)
        {
            var total = json.PensionTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Pension Income - Taxable";

            double sum = 0;
            foreach (PensionValue x in json.Pensions)
            {
                if (x.Type == filter)
                {
                    if (x.PITaxAssessableValues != 0)
                    {
                        var t = Convert.ToDouble(x.PITaxAssessableValues);
                        sum = sum + t;
                    }

                }    
            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.PensionTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PensionTotal[json.PensionTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PensionTotal.Add(total);

            }
        }
       private void calculateSuperIncomeTaxOffset(string owner, string filter, Projection json)
        {
            var total = json.PensionTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Super income streams tax offset";

            double sum = 0;
            foreach (PensionValue x in json.Pensions)
            {
                if (x.Type == filter)
                {
                    if (x.PITaxAssessableValues != 0)
                    {
                        var t = Convert.ToDouble(x.PITaxAssessableValues) * (15 / 100);
                        sum = sum + t;
                    }

                }
            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.PensionTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PensionTotal[json.PensionTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PensionTotal.Add(total);

            }
        }

       private void calculateTotalLumpSumWithdrawals(string owner, Projection json)
        {
            var total = json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault();
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Super Lump Sum Withdrawals";

            double sum = 0;
            foreach (SuperValue x in json.Supers)
            {
                if (x.Type == "Client" || x.Type == "Partner")
                {
                    if (x.LumpSumValues != 0)
                    {
                        var t = Convert.ToDouble(x.LumpSumValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.SuperTotal[json.SuperTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.SuperTotal.Add(total);

            }
        }
       private void calculateTotalSalarySacrificeContribution(string owner, Projection json)
        {
            var total = json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault();
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Salary Sacrifice Contributions";

            double sum = 0;
            foreach (SuperValue x in json.Supers)
            {
                if (x.Type == "Client" || x.Type == "Partner")
                {
                    if (x.SsContrValues != 0)
                    {
                        var t = Convert.ToDouble(x.SsContrValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.SuperTotal[json.SuperTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.SuperTotal.Add(total);

            }
        }
       private void calculateTotalPNCContibution(string owner, Projection json)
        {
            var total = json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault();
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Personal Non-Concessional Contributions";

            double sum = 0;
            foreach (SuperValue x in json.Supers)
            {
                if (x.Type == "Client" || x.Type == "Partner")
                {
                    if (x.PncContrValues != 0)
                    {
                        var t = Convert.ToDouble(x.PncContrValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.SuperTotal[json.SuperTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.SuperTotal.Add(total);

            }
        }
       private void calculateTotalSpouseContibution(string owner, Projection json)
        {
            var total = json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault();
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Spouse Contributions";

            double sum = 0;
            foreach (SuperValue x in json.Supers)
            {
                if (x.Type == "Client" || x.Type == "Partner")
                {
                    if (x.SpouseContrValues != 0)
                    {
                        var t = Convert.ToDouble(x.SpouseContrValues);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.SuperTotal[json.SuperTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.SuperTotal.Add(total);

            }
        }
       private void calculateLumpSumTax(string owner, string filter, Projection json)
        {
            var total = json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Super Lump Sum Withdrawal";

            double sum = 0;
            foreach (SuperValue x in json.Supers)
            {
                if (x.Type == filter)
                {
                    if (x.LumpSumTaxableValues != 0)
                    {
                        var t = Convert.ToDouble(x.LumpSumTaxableValues);
                        sum = sum + t;
                    }

                }
            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.SuperTotal[json.SuperTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.SuperTotal.Add(total);

            }
        }
       private void calculateSalarySacrificeTax(string owner, string filter, Projection json)
        {
            var total = json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Salary Sacrifice / Personal Deductible";

            double sum = 0;
            foreach (SuperValue x in json.Supers)
            {
                if (x.Type == filter)
                {
                    if (x.SsContrValues != 0)
                    {
                        var t = Convert.ToDouble(x.SsContrValues);
                        sum = sum + t;
                    }

                }
            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.SuperTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.SuperTotal[json.SuperTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.SuperTotal.Add(total);

            }
        }

       private void calculateTotalTaxIncome(string owner, string filter, Projection json)
        {
            var total = json.Income.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Income";

            double sum = 0;

            if (filter == "Client")
            {
                foreach (Income x in json.Inflow)
                {
                    if (x.Owner == "ClientIncome-tax")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                }
                foreach (Common x in json.FaTotals)
                {
                    if (x.Owner == "TotalTaxIE-client")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                    if (x.Owner == "FrankingCredits-client")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                }

                foreach (Common x in json.PensionTotal)
                {
                    if (x.Owner == "PensionIncome-client")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                   
                }



            }
            else
            {

                foreach (Income x in json.Inflow)
                {
                    if (x.Owner == "PartnerIncome-tax")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                }
                foreach (Common x in json.FaTotals)
                {
                    if (x.Owner == "TotalTaxIE-partner")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                    if (x.Owner == "FrankingCredits-partner")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                }

                foreach (Common x in json.PensionTotal)
                {
                    if (x.Owner == "PensionIncome-partner")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }

                }

            }

            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.Income.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.Income[json.Income.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.Income.Add(total);

            }
        }
       private void calculateCapitalLossAdjustment(string owner, string filter, Projection json)
        {
            var total = json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Capital loss adjustment";

            double AccumCL = 0;

            if (filter == "Client")
            {
                double SumCG = 0;
                double SumCL = 0;

                
                foreach (Common x in json.FaTotals)
                {
                    if (x.Owner == "RCGFA-client")
                    {
                        if (x.Value >= 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCG = SumCG + t;
                        }
                        else if (x.Value < 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCL = SumCL + t;
                        }

                    }
                   
                }

                foreach (Common x in json.PropertiesTotal)
                {
                    if (x.Owner == "RCGP-client")
                    {
                        if (x.Value >= 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCG = SumCG + t;
                        }
                        else if (x.Value < 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCL = SumCL + t;
                        }

                    }

                }

                AccumCL = AccumCL + SumCL;
                var p = SumCG + AccumCL;
                double CLA = 0;
                if (p >= 0)
                {
                    CLA = AccumCL;
                    AccumCL = 0;
                }
                else if (p < 0)
                {
                    CLA = SumCG * (-1);
                    AccumCL = SumCG - AccumCL;
                }

                total.Value = Math.Round(CLA);

            }
            else
            {

                double SumCG = 0;
                double SumCL = 0;


                foreach (Common x in json.FaTotals)
                {
                    if (x.Owner == "RCGFA-partner")
                    {
                        if (x.Value >= 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCG = SumCG + t;
                        }
                        else if (x.Value < 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCL = SumCL + t;
                        }

                    }

                }

                foreach (Common x in json.PropertiesTotal)
                {
                    if (x.Owner == "RCGP-partner")
                    {
                        if (x.Value >= 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCG = SumCG + t;
                        }
                        else if (x.Value < 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            SumCL = SumCL + t;
                        }

                    }

                }

                AccumCL = AccumCL + SumCL;
                var p = SumCG + AccumCL;
                double CLA = 0;
                if (p >= 0)
                {
                    CLA = AccumCL;
                    AccumCL = 0;
                }
                else if (p < 0)
                {
                    CLA = SumCG * (-1);
                    AccumCL = SumCG - AccumCL;
                }

                total.Value = Math.Round(CLA);

            }

          

            if (json.PropertiesTotal.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PropertiesTotal[json.PropertiesTotal.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PropertiesTotal.Add(total);

            }
        }
       private void calculateTotalAssessibleIncome(string owner, string filter, string type,Projection json)
        {
            var total = json.Income.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "ClientAssessibleIncome";

            double sum = 0;

            foreach (Income x in json.Income)
            {
                if (x.Owner == filter)
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }

            if (type == "Client")
            {
                foreach (Common x in json.FaTotals)
                {
                    if (x.Owner == "RCGFA-client")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }

                }
                foreach (Common x in json.PropertiesTotal)
                {
                    if (x.Owner == "RCGP-client" || x.Owner == "CLA-client")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }

                }
                foreach (Common x in json.SuperTotal)
                {
                    if (x.Owner == "LumpSum-client")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }

                }

            }
            else
            {

                foreach (Common x in json.FaTotals)
                {
                    if (x.Owner == "RCGFA-partner")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }

                }
                foreach (Common x in json.PropertiesTotal)
                {
                    if (x.Owner == "RCGP-partner" || x.Owner == "CLA-partner")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }

                }
                foreach (Common x in json.SuperTotal)
                {
                    if (x.Owner == "LumpSum-partner")
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }

                }

            }

            if (sum > 0)
            {
                total.Value = Math.Round(sum);
            }
            else
            {
                total.Value = 0;
            }

            if (json.Income.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.Income[json.Income.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.Income.Add(total);

            }
        }
       private void calculateClientTotalDeductions(string owner, string filter, Projection json)
        {
            var total = json.ClientDeductions.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = owner;

            double sum = 0;

            foreach (Income x in json.ClientDeductions)
            {
                if (x.Owner == filter || x.Owner == "Joint")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.PropertiesTotal)
            {
                if (x.Owner == "InvestmentPropertyExpenses-client")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.SuperTotal)
            {
                if (x.Owner == "SalarySacrifice-client")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.LBTotal)
            {
                if (x.Owner == "Accruedliability-client")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);
            }
            else
            {
                total.Value = 0;
            }

            if (json.ClientDeductions.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.ClientDeductions[json.ClientDeductions.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.ClientDeductions.Add(total);

            }
        }
       private void calculatePartnerTotalDeductions(string owner, string filter, Projection json)
        {
            var total = json.PartnerDeductions.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = owner;

            double sum = 0;

            foreach (Income x in json.ClientDeductions)
            {
                if (x.Owner == filter || x.Owner == "Joint")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.PropertiesTotal)
            {
                if (x.Owner == "InvestmentPropertyExpenses-partner")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.SuperTotal)
            {
                if (x.Owner == "SalarySacrifice-partner")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.LBTotal)
            {
                if (x.Owner == "Accruedliability-partner")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);
            }
            else
            {
                total.Value = 0;
            }

            if (json.PartnerDeductions.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PartnerDeductions[json.PartnerDeductions.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PartnerDeductions.Add(total);

            }
        }
       private void calculateRefundableTaxOffset(string owner, string type, Projection json)
        {
            var total = new Common();
            if (type == "Client")
            {
                 total = json.ClientRTaxOffset.Where(y => y.Owner == owner).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                total = json.PartnerRTaxOffset.Where(y => y.Owner == owner).FirstOrDefault();
            }
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Franking Credits";

            double sum = 0;
            double jointSum = 0;
            foreach (InvestmentValue x in json.Investments)
            {
                if (x.Type == type)
                {
                    if (x.FrankingCreditsValues != 0)
                    {
                        var t = Convert.ToDouble(x.FrankingCreditsValues);
                        sum = sum + t;
                    }
                }

                if (x.Type == "Joint")
                {
                    if (x.FrankingCreditsValues != 0)
                    {
                        var t = Convert.ToDouble(x.FrankingCreditsValues);
                        jointSum = jointSum + t;
                    }
                }
            }
     
            if (sum > 0 || jointSum > 0)
            {
                total.Value = Math.Round(sum + (jointSum/2));
            }
            else
            {
                total.Value = 0;
            }

            if (type == "Client")
            {
                if (json.ClientRTaxOffset.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.ClientRTaxOffset[json.ClientRTaxOffset.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.ClientRTaxOffset.Add(total);

                }
            }
            else if (type == "Partner")
            {
                if (json.PartnerRTaxOffset.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.PartnerRTaxOffset[json.PartnerRTaxOffset.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.PartnerRTaxOffset.Add(total);

                }
            }
        }
       private void calculateClientTotalNRTaxOffset(string owner, string filter, Projection json)
        {
            var total = json.ClientNrTaxOffset.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Total non-refundable tax offsets";

            double sum = 0;

            foreach (Common x in json.ClientNrTaxOffset)
            {
                if (x.Owner == filter)
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.PensionTotal)
            {
                if (x.Owner == "SIncomeTaxOffset-client")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
           

            if (sum > 0)
            {
                total.Value = Math.Round(sum);
            }
            else
            {
                total.Value = 0;
            }

            if (json.ClientNrTaxOffset.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.ClientNrTaxOffset[json.ClientNrTaxOffset.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.ClientNrTaxOffset.Add(total);

            }
        }
       private void calculatePartnerTotalNRTaxOffset(string owner, string filter, Projection json)
        {
            var total = json.PartnerNrTaxOffsets.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Total non-refundable tax offsets";

            double sum = 0;

            foreach (Common x in json.PartnerNrTaxOffsets)
            {
                if (x.Owner == filter)
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }
            foreach (Common x in json.PensionTotal)
            {
                if (x.Owner == "SIncomeTaxOffset-partner")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }

            }


            if (sum > 0)
            {
                total.Value = Math.Round(sum);
            }
            else
            {
                total.Value = 0;
            }

            if (json.PartnerNrTaxOffsets.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.PartnerNrTaxOffsets[json.PartnerNrTaxOffsets.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.PartnerNrTaxOffsets.Add(total);

            }
        }
       private void calculateGrossTax(string owner, string filter, string type, Projection json)
        {
            var taxableIncome = new Common();
            if (type == "Client")
            {
                taxableIncome = json.ClientTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                taxableIncome = json.PartnerTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }

            var index = 0;
            index = marginalTaxRates.Max(x => x.Index);

            var total = json.GrossTax.Where(y => y.Owner == owner).FirstOrDefault();
           

            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Gross tax payable";

            double totalGrTax = 0;
            double incTaxed = 0;

            for (var j = index; j >= 1; j--)
            {
                var threshold = marginalTaxRates.Where(c => c.Index == (j - 1)).FirstOrDefault();
                var rate = marginalTaxRates.Where(c => c.Index == j).FirstOrDefault();
                //if (typeof ta taxableIncome..values[this.clientDetails.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.clientDetails.startDate + i]))
                //{
                    var inc = Math.Max(0, taxableIncome.Value - incTaxed - threshold.Threshold);
                    var grTax = inc * Convert.ToDouble(rate.Rate / 100);
                    totalGrTax = totalGrTax + grTax;
                    incTaxed = incTaxed + inc;
                //}

            }


            if (totalGrTax > 0 )
            {
                total.Value = Math.Round(totalGrTax);
            }
            else
            {
                total.Value = 0;
            }

            
                if (json.GrossTax.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.GrossTax[json.GrossTax.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.GrossTax.Add(total);

                }
            
        }
       private void calculateMedicareLevy(string owner, string filter, string type, Projection json)
        {
            var taxableIncome = new Common();
            if (type == "Client")
            {
                taxableIncome = json.ClientTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                taxableIncome = json.PartnerTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }

            var grossTax = new Common();
            if (type == "Client")
            {
                grossTax = json.GrossTax.Where(y => y.Owner == "ClientGrossTax").FirstOrDefault();
            }
            else if (type == "Partner")
            {
                grossTax = json.GrossTax.Where(y => y.Owner == "PartnerGrossTax").FirstOrDefault();
            }

            var index = 0;
            index = marginalTaxRates.Max(x => x.Index);



            var total = new Common();
            if (type == "Client")
            {
                total = json.ClientMedicareLevy.Where(y => y.Owner == owner).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                total = json.PartnerMedicareLevy.Where(y => y.Owner == owner).FirstOrDefault();
            }

            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Medicare levy";

            double ml = 0;

            if (grossTax.Value > 0)
            {
                //TODO : Verify
                //if (typeof taxableIncome[0].values[this.clientDetails.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.clientDetails.startDate + i]))
                //{

                    double val = taxableIncome.Value;
                    ml = (val * 2) / 100;

              //  }
            }
            else
            {
                ml = 0;
            }


            if (ml > 0)
            {
                total.Value = Math.Round(ml);
            }
            else
            {
                total.Value = 0;
            }

            if (type == "Client")
            {
                if (json.ClientMedicareLevy.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.ClientMedicareLevy[json.ClientMedicareLevy.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.ClientMedicareLevy.Add(total);

                }
            }
            else if (type == "Partner")
            {
                if (json.PartnerMedicareLevy.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.PartnerMedicareLevy[json.PartnerMedicareLevy.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.PartnerMedicareLevy.Add(total);

                }
            }

        }

       private void calculateTaxPayableNonRefundable(string owner, string filter1, string filter2, string type, Projection json)
        {
            var grossTax = new Common();
            var nRTaxOffset = new Common();
            if (type == "Client")
            {
                grossTax = json.GrossTax.Where(y => y.Owner == filter1).FirstOrDefault();
                nRTaxOffset = json.ClientNrTaxOffset.Where(y => y.Owner == filter2).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                grossTax = json.GrossTax.Where(y => y.Owner == filter1).FirstOrDefault();
                nRTaxOffset = json.PartnerNrTaxOffsets.Where(y => y.Owner == filter2).FirstOrDefault();
            }

          

        
             var total = json.NetPayable.Where(y => y.Owner == owner).FirstOrDefault();
            

            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Tax payable after non-refundable tax offsets";

            double val = 0;
            val = Math.Max(0, (grossTax.Value - nRTaxOffset.Value));
            if (val > 0)
            {
                total.Value = Math.Round(val);
            }
            else
            {
                total.Value = 0;
            }

         
                if (json.NetPayable.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.NetPayable[json.NetPayable.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.NetPayable.Add(total);

                }
           

        }
       private void calculateTaxPayableRefundable(string owner, string filter1, string filter2, string type, Projection json)
        {
            var tp = new Common();
            var rTaxOffset = new Common();
            if (type == "Client")
            {
                tp = json.NetPayable.Where(y => y.Owner == filter1).FirstOrDefault();
                rTaxOffset = json.ClientRTaxOffset.Where(y => y.Owner == filter2).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                tp = json.NetPayable.Where(y => y.Owner == filter1).FirstOrDefault();
                rTaxOffset = json.PartnerRTaxOffset.Where(y => y.Owner == filter2).FirstOrDefault();
            }




            var total = json.NetPayable.Where(y => y.Owner == owner).FirstOrDefault();


            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Tax payable after refundable tax offsets";

            double val = 0;
            val = tp.Value - rTaxOffset.Value;

            if (val > 0)
            {
                total.Value = Math.Round(val);
            }
            else
            {
                total.Value = 0;
            }


            if (json.NetPayable.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.NetPayable[json.NetPayable.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.NetPayable.Add(total);

            }


        }
       private void calculateTotalTaxesPayable(string owner, string filter1, string filter2, string type, Projection json)
        {
            var netTax = new Common();
            var medicare = new Common();
            if (type == "Client")
            {
                netTax = json.NetPayable.Where(y => y.Owner == filter1).FirstOrDefault();
                medicare = json.ClientMedicareLevy.Where(y => y.Owner == filter2).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                netTax = json.NetPayable.Where(y => y.Owner == filter1).FirstOrDefault();
                medicare = json.PartnerMedicareLevy.Where(y => y.Owner == filter2).FirstOrDefault();
            }




            var total = json.TotalPayable.Where(y => y.Owner == owner).FirstOrDefault();


            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Total taxes payable";

            double val = 0;
            val =  netTax.Value - medicare.Value;

            if (val > 0)
            {
                total.Value = Math.Round(val);
            }
            else
            {
                total.Value = 0;
            }


            if (json.TotalPayable.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.TotalPayable[json.TotalPayable.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.TotalPayable.Add(total);

            }


        }
       private void calculateAverageTaxRate(string owner, string filter1, string filter2, string type, Projection json)
        {
            var totalTax = new Common();
            var income = new Income();
            if (type == "Client")
            {
                totalTax = json.TotalPayable.Where(y => y.Owner == filter1).FirstOrDefault();
                income = json.Income.Where(y => y.Owner == filter2).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                totalTax = json.TotalPayable.Where(y => y.Owner == filter1).FirstOrDefault();
                income = json.Income.Where(y => y.Owner == filter2).FirstOrDefault();
            }




            var total = json.TotalPayable.Where(y => y.Owner == owner).FirstOrDefault();


            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Average tax rate";

            double val = 0;
            val = (totalTax.Value / income.Value) * 100;

            if (val > 0)
            {
                total.Value = Math.Round(val,1);
            }
            else
            {
                total.Value = 0;
            }


            if (json.TotalPayable.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.TotalPayable[json.TotalPayable.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.TotalPayable.Add(total);

            }


        }
       private void calculateMarginalTaxRate(string owner, string filter, string type, Projection json)
        {
            var taxableIncome = new Common();
            if (type == "Client")
            {
                taxableIncome = json.ClientTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                taxableIncome = json.PartnerTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }

            var index = 0;
            index = marginalTaxRates.Max(x => x.Index);

            var total = json.TotalPayable.Where(y => y.Owner == owner).FirstOrDefault();


            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Marginal tax rate";

            double mtr = 0;
            double val = taxableIncome.Value;

            for (var j = index; j >= 1; j--)
            {
                var one = marginalTaxRates.Where(c => c.Index == (j - 1)).FirstOrDefault();
                var two = marginalTaxRates.Where(c => c.Index == j).FirstOrDefault();
                //if (typeof ta taxableIncome..values[this.clientDetails.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.clientDetails.startDate + i]))
                //{
                if (j == index)
                {
                    if (val > one.Threshold)
                    {
                        mtr = Convert.ToDouble(two.Rate);
                    }
                }
                else
                {
                    if (val > one.Threshold && val <= two.Threshold)
                    {
                        mtr = Convert.ToDouble(two.Rate);
                        break;
                    }
                }
                //}

            }


            if (mtr > 0)
            {
                total.Value = Math.Round(mtr,1);
            }
            else
            {
                total.Value = 0;
            }


            if (json.TotalPayable.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.TotalPayable[json.TotalPayable.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.TotalPayable.Add(total);

            }

        }
       private void calculateTotalIncomeTaxPayable(string owner, Projection json)
        {
            var total = json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault();
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Income Tax Payable";

            double sum = 0;
            foreach (Common x in json.TotalPayable)
            {
                if (x.Owner == "ClientTotalTaxPayable" || x.Owner == "PartnerTotalTaxPayable")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }


            if (sum > 0)
            {
                total.Value = sum;

            }
            else
            {
                total.Value = 0;
            }

            if (json.FaTotals.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.FaTotals[json.FaTotals.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.FaTotals.Add(total);

            }
        }
       private void calculateLowIncomeTO(string owner, string filter, string type, Projection json)
        {
            var taxableIncome = new Common();
            if (type == "Client")
            {
                taxableIncome = json.ClientTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                taxableIncome = json.PartnerTaxableIncome.Where(y => y.Owner == filter).FirstOrDefault();
            }


            var total = new Common();
            if (type == "Client")
            {
                total = json.ClientNrTaxOffset.Where(y => y.Owner == owner).FirstOrDefault();
            }
            else if (type == "Partner")
            {
                total = json.PartnerNrTaxOffsets.Where(y => y.Owner == owner).FirstOrDefault();
            }

            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Low income tax offset";

            double lito = 0;


            var val = taxableIncome.Value;
            if (val <= 37000)
            {
                lito = 445;
            }
            else if (val > 37000 && val <= 66667)
            {
                lito = 445 - ((val - 37000) * 0.015);

              }
            else if (val > 66667)
            {
                lito = 0;
            }


            if (lito > 0)
            {
                total.Value = Math.Round(lito);
            }
            else
            {
                total.Value = 0;
            }

            if (type == "Client")
            {
                if (json.ClientNrTaxOffset.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.ClientNrTaxOffset[json.ClientNrTaxOffset.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.ClientNrTaxOffset.Add(total);

                }
            }
            else if (type == "Partner")
            {
                if (json.PartnerNrTaxOffsets.Where(y => y.Owner == owner).FirstOrDefault() != null)
                {
                    json.PartnerNrTaxOffsets[json.PartnerNrTaxOffsets.FindIndex(y => y.Owner == owner)] = total;
                }
                else
                {
                    json.PartnerNrTaxOffsets.Add(total);

                }
            }

        }
       private void calculateClientTaxableIncome(Projection json)
        {

            double lossBF = 0;
            double lossAdj = 0;
            var income = json.Income.Where(c => c.Owner == "ClientAssessibleIncome").FirstOrDefault();
            var deduction = json.ClientDeductions.Where(c => c.Owner == "ClientDeductions").FirstOrDefault();

      
            var taxable = json.ClientTaxableIncome.Where(y => y.Owner == "ClientTaxableIncome").FirstOrDefault();
            if (taxable == null)
            {
                taxable = new Common();
            }
            else
            {
                
            }

            var adjustment = json.ClientLossAdjustment.Where(y => y.Owner == "ClientAdjustment").FirstOrDefault();
            if (adjustment == null)
            {
                adjustment = new Common();
            }
            else
            {

            }

            taxable.Owner = "ClientTaxableIncome";
            taxable.Name = "ClientTaxableIncome";
            adjustment.Owner = "ClientAdjustment";
            adjustment.Name = "ClientAdjustment";

            double lossG = 0;
            double taxInc = 0;


            var Gti = Math.Max(0, (income.Value - deduction.Value));
            if (Gti > 0)
            {
                lossAdj = Math.Min(lossBF, Gti);
            }


            if (deduction.Value > income.Value)
            {
                lossG = deduction.Value - income.Value;
            }

            //LossCF stands for ‘Loss carried forward’
            var lossCF = lossG + lossBF - lossAdj;
            lossBF = lossCF;

            taxInc = Gti - lossAdj;

            if (lossAdj > 0)
            {
                adjustment.Value = lossAdj;
            }
            else
            {
                adjustment.Value = 0;
            }
           
            if (json.ClientLossAdjustment.Where(y => y.Owner == "ClientAdjustment").FirstOrDefault() != null)
            {
                json.ClientLossAdjustment[json.ClientLossAdjustment.FindIndex(y => y.Owner == "ClientAdjustment")] = adjustment;
            }
            else
            {
                json.ClientLossAdjustment.Add(taxable);

            }

            if (taxInc > 0)
            {
                taxable.Value = taxInc;
            }
            else
            {
                taxable.Value = 0;
            }
          


            if (json.ClientTaxableIncome.Where(y => y.Owner == "ClientTaxableIncome").FirstOrDefault() != null)
                {
                    json.ClientTaxableIncome[json.ClientTaxableIncome.FindIndex(y => y.Owner == "ClientTaxableIncome")] = taxable;
                }
                else
                {
                    json.ClientTaxableIncome.Add(taxable);

                }
          

        }
       private void calculatePartnerTaxableIncome(Projection json)
        {

            double lossBF = 0;
            double lossAdj = 0;
            var income = json.Income.Where(c => c.Owner == "PartnerAssessibleIncome").FirstOrDefault();
            var deduction = json.PartnerDeductions.Where(c => c.Owner == "PartnerDeductions").FirstOrDefault();


            var taxable = json.PartnerTaxableIncome.Where(y => y.Owner == "PartnerTaxableIncome").FirstOrDefault();
            if (taxable == null)
            {
                taxable = new Common();
            }
            else
            {

            }

            var adjustment = json.PartnerLossAdjustment.Where(y => y.Owner == "PartnerAdjustment").FirstOrDefault();
            if (adjustment == null)
            {
                adjustment = new Common();
            }
            else
            {

            }

            taxable.Owner = "PartnerTaxableIncome";
            taxable.Name = "PartnerTaxableIncome";
            adjustment.Owner = "PartnerAdjustment";
            adjustment.Name = "PartnerAdjustment";

            double lossG = 0;
            double taxInc = 0;


            var Gti = Math.Max(0, (income.Value - deduction.Value));
            if (Gti > 0)
            {
                lossAdj = Math.Min(lossBF, Gti);
            }


            if (deduction.Value > income.Value)
            {
                lossG = deduction.Value - income.Value;
            }

            //LossCF stands for ‘Loss carried forward’
            var lossCF = lossG + lossBF - lossAdj;
            lossBF = lossCF;

            taxInc = Gti - lossAdj;

            if (lossAdj > 0)
            {
                adjustment.Value = lossAdj;
            }
            else
            {
                adjustment.Value = 0;
            }

            if (json.PartnerLossAdjustment.Where(y => y.Owner == "PartnerAdjustment").FirstOrDefault() != null)
            {
                json.PartnerLossAdjustment[json.PartnerLossAdjustment.FindIndex(y => y.Owner == "PartnerAdjustment")] = adjustment;
            }
            else
            {
                json.PartnerLossAdjustment.Add(taxable);

            }

            if (taxInc > 0)
            {
                taxable.Value = taxInc;
            }
            else
            {
                taxable.Value = 0;
            }



            if (json.PartnerTaxableIncome.Where(y => y.Owner == "PartnerTaxableIncome").FirstOrDefault() != null)
            {
                json.PartnerTaxableIncome[json.PartnerTaxableIncome.FindIndex(y => y.Owner == "PartnerTaxableIncome")] = taxable;
            }
            else
            {
                json.PartnerTaxableIncome.Add(taxable);

            }


        }

        private void calculateTotalIncome(string owner, string filter, Projection json)
        {
            var total = json.Inflow.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Income";

            double sum = 0;

          
                foreach (Income x in json.Inflow)
                {
                    if (x.Owner == filter)
                    {
                        if (x.Value != 0)
                        {
                            var t = Convert.ToDouble(x.Value);
                            sum = sum + t;
                        }

                    }
                }
            
           

            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.Inflow.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.Inflow[json.Inflow.FindIndex(y => y.Owner == owner)] = total;
                json.Income[json.Income.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.Inflow.Add(total);
                json.Income.Add(total);

            }
        }
        private void calculateTotalExpenditure(string owner, string filter, Projection json)
        {
            var total = json.Outflow.Where(y => y.Owner == owner).FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = owner;
            total.Name = "Expenditure";

            double sum = 0;


            foreach (Income x in json.Outflow)
            {
                if (x.Owner == filter)
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }

                }
            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.Outflow.Where(y => y.Owner == owner).FirstOrDefault() != null)
            {
                json.Outflow[json.Outflow.FindIndex(y => y.Owner == owner)] = total;
            }
            else
            {
                json.Outflow.Add(total);


            }
        }

        private void calculateTotalInflows( Projection json)
        {
            var total = json.Inflow.Where(y => y.Owner == "Inflow").FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = "Inflow";
            total.Name = "Inflow";

            double sum = 0;


            foreach (Income x in json.Inflow)
            {
                if (x.Owner == "Total-client" || x.Owner == "Total-partner")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.FaTotals)
            {
                if (x.Owner == "TotalIPO" || x.Owner == "TotalIW")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.PropertiesTotal)
            {
                if (x.Owner == "TotalRent" || x.Owner == "TotalSaleProceeds")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.PensionTotal)
            {
                if (x.Owner == "TotalPensionIncome")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.SuperTotal)
            {
                if (x.Owner == "TotalLumpSumWithdrawals")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.LATotals)
            {
                if (x.Owner == "TotalLASales")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.Inflow.Where(y => y.Owner == "Inflow").FirstOrDefault() != null)
            {
                json.Inflow[json.Inflow.FindIndex(y => y.Owner == "Inflow")] = total;
            }
            else
            {
                json.Inflow.Add(total);


            }
        }
        private void calculateTotalOutflows(Projection json)
        {
            var total = json.Outflow.Where(y => y.Owner == "Outflow").FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Income();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = "Outflow";
            total.Name = "Outflow";

            double sum = 0;


            foreach (Income x in json.Outflow)
            {
                if (x.Owner == "Total-client" || x.Owner == "Total-partner" || x.Owner == "Total-joint")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.FaTotals)
            {
                if (x.Owner == "TotalITPayable" || x.Owner == "TotalIC")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.PropertiesTotal)
            {
                if (x.Owner == "TotalPropertyExpenses")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.SuperTotal)
            {
                if (x.Owner == "TotalSalarySacrificeContributions" || x.Owner == "TotalPNCContributions" || x.Owner == "TotalSpouseContributions")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.LBTotal)
            {
                if (x.Owner == "TotalDebtRepayment")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }

            foreach (Common x in json.LATotals)
            {
                if (x.Owner == "TotalLAPurchase")
                {
                    if (x.Value != 0)
                    {
                        var t = Convert.ToDouble(x.Value);
                        sum = sum + t;
                    }
                }
            }



            if (sum > 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.Outflow.Where(y => y.Owner == "Outflow").FirstOrDefault() != null)
            {
                json.Outflow[json.Outflow.FindIndex(y => y.Owner == "Outflow")] = total;
            }
            else
            {
                json.Outflow.Add(total);
            }
        }
        private void calculateNetCashflow(Projection json, int q,int id)
        {
            var inflow = json.Inflow.Where(y => y.Owner == "Inflow").FirstOrDefault();
            var outflow = json.Outflow.Where(y => y.Owner == "Outflow").FirstOrDefault();

            var total = json.NetCashflows.Where(y => y.Owner == "NetCashflow").FirstOrDefault();

            if (total == null)
            {
                total = new NetCashflow();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = "NetCashflow";
            total.Name = "NetCashflow";

            double diff = 0;
            diff = inflow.Value - outflow.Value;

            if (q == 1)
            {
                var res = json.Investments.Where(y => y.Owner == id).FirstOrDefault();
                if (diff > 0)
                {
                    total.Values= diff;
                    total.AssetAllocationValues = diff * -1;
                    total.UnfundedNetCfValues = (total.Values + total.AssetAllocationValues);
                }
                else if (diff < 0)
                {
                    total.Values = diff;
                    total.AssetAllocationValues = (res.CashFlowValues) * -1;
                    total.UnfundedNetCfValues = (total.Values + total.AssetAllocationValues);
                }
                else
                {
                    total.Values = 0;
                    total.AssetAllocationValues = 0;
                    total.UnfundedNetCfValues = 0;
                }
            }
            else
            {
                if (diff > 0)
                {
                    total.Values = diff;
                    total.AssetAllocationValues = diff * -1;
                    total.UnfundedNetCfValues = (total.Values + total.AssetAllocationValues);
                }
                else if (diff < 0)
                {
                    total.Values = diff;
                    total.AssetAllocationValues = 0;
                    total.UnfundedNetCfValues = (total.Values + total.AssetAllocationValues);
                }
                else
                {
                    total.Values = 0;
                    total.AssetAllocationValues = 0;
                    total.UnfundedNetCfValues = 0;
                }
            }

            if (json.NetCashflows.Where(y => y.Owner == "NetCashflow").FirstOrDefault() != null)
            {
                json.NetCashflows[json.NetCashflows.FindIndex(y => y.Owner == "NetCashflow")] = total;
            }
            else
            {
                json.NetCashflows.Add(total);
            }
        }
        private void calculateNetAsset( Projection json)
        {
            var total = json.NetAsset.Where(y => y.Owner == "NetAsset").FirstOrDefault();
            // var totalVal: any = { };
            if (total == null)
            {
                total = new Common();
            }
            else
            {
                // totalVal = total.values;
            }
            total.Owner = "NetAsset";
            total.Name = "NetAsset";

            double sum = 0;


           

            foreach (InvestmentValue x in json.Investments)
            {
               
                    if (x.EndingValues != 0)
                    {
                        var t = Convert.ToDouble(x.EndingValues);
                        sum = sum + t;
                    }

            }

            foreach (PropertyValue x in json.Properties)
            {

                if (x.EndingValues != 0)
                {
                    var t = Convert.ToDouble(x.EndingValues);
                    sum = sum + t;
                }

            }
            foreach (SuperValue x in json.Supers)
            {

                if (x.EndingValues != 0)
                {
                    var t = Convert.ToDouble(x.EndingValues);
                    sum = sum + t;
                }

            }

            foreach (PensionValue x in json.Pensions)
            {

                if (x.EndingValues != 0)
                {
                    var t = Convert.ToDouble(x.EndingValues);
                    sum = sum + t;
                }

            }

            foreach (LiabilityValue x in json.Liabilities)
            {

                if (x.EndingValues != 0)
                {
                    var t = Convert.ToDouble(x.EndingValues);
                    sum = sum + t;
                }

            }


            if (sum != 0)
            {
                total.Value = Math.Round(sum);

            }
            else
            {
                total.Value = 0;
            }

            if (json.NetAsset.Where(y => y.Owner == "NetAsset").FirstOrDefault() != null)
            {
                json.NetAsset[json.NetAsset.FindIndex(y => y.Owner == "NetAsset")] = total;
            }
            else
            {
                json.NetAsset.Add(total);
            }
        }


        //    [HttpGet("{clientId}")]
        //    public List<AlternativeClientProductsViewModel> projections(int clientId)
        //    {
        //        IEnumerable<CashFlowViewModel> cashFlowIncome;
        //        IEnumerable<CashFlowViewModel> cashFlowExpenditure;
        //        IEnumerable<MarginalTaxRatesViewModel> marginalTaxRates;
        //        IEnumerable<InvestmentViewModel> investments;
        //        IEnumerable<GeneralViewModel> generalAssumptions;
        //        IEnumerable<InvestmentDetailsViewModel> investmentCW;
        //        IEnumerable<PropertyViewModel> properties;
        //        IEnumerable<SuperViewModel> supers;
        //        IEnumerable<SuperDetailsViewModel> superDetails;
        //        IEnumerable<SgcrateViewModel> sgcRates;
        //        IEnumerable<SuperAssumptionsViewModel> superAssumptions;
        //        IEnumerable<LiabilityViewModel> liabilities;
        //        IEnumerable<LiabilityDetailsViewModel> liabilityDD;
        //        IEnumerable<PensionViewModel> pensions;
        //        IEnumerable<PensionDetailsViewModel> pensionDD;
        //        IEnumerable<PreservationAgeViewModel> preservationAge;
        //        IEnumerable<MinimumPensionDrawdownViewModel> minimumPensionDrawdown;
        //        IEnumerable<LifestyleAssetViewModel> lifestyleAssets;
        //        IEnumerable<AssetTypesViewModel> assetAssumptions;
        //        IEnumerable<QualifyingAgeViewModel> qualifyingAge;

        //        //Filtered
        //        IEnumerable<CashFlowViewModel> cfiClient;
        //        IEnumerable<CashFlowViewModel> cfiPartner;
        //        IEnumerable<CashFlowViewModel> cfeClient;
        //        IEnumerable<CashFlowViewModel> cfePartner;
        //        IEnumerable<CashFlowViewModel> cfeJoint;

        //        IEnumerable<CashFlowViewModel> EPRTClient;
        //        IEnumerable<CashFlowViewModel> EPRTPartner;
        //        IEnumerable<CashFlowViewModel> EPRTJoint;

        //        IEnumerable<InvestmentViewModel> investmentClient;
        //        IEnumerable<InvestmentViewModel> investmentClientOptimized;
        //        IEnumerable<InvestmentViewModel> investmentPartner;
        //        IEnumerable<InvestmentViewModel> investmentPartnerOptimized;
        //        IEnumerable<InvestmentViewModel> investmentJoint;
        //        IEnumerable<InvestmentViewModel> investmentJointOptimized;

        //        IEnumerable<PropertyViewModel> propertiesClient;
        //        IEnumerable<PropertyViewModel> propertiesPartner;
        //        IEnumerable<PropertyViewModel> propertiesJoint;

        //        IEnumerable<SuperViewModel> superClient;
        //        IEnumerable<SuperViewModel> superPartner;

        //        IEnumerable<LiabilityViewModel> liabilityClient;
        //        IEnumerable<LiabilityViewModel> liabilityPartner;
        //        IEnumerable<LiabilityViewModel> liabilityJoint;

        //        IEnumerable<PensionViewModel> pensionClient;
        //        IEnumerable<PensionViewModel> pensionPartner;

        //        IEnumerable<LifestyleAssetViewModel> lifestyleClient;
        //        IEnumerable<LifestyleAssetViewModel> lifestylePartner;
        //        IEnumerable<LifestyleAssetViewModel> lifestyleJoint;
        //        var lifestyles = new List<dynamic>();

        //        BasicDetails clientDetails;

        //        if (clientId != 0)
        //        {

        //            clientDetails = context.BasicDetails.Where(x => x.ClientId == clientId).FirstOrDefault();

        //            cashFlowIncome = this.mapper.Map<IEnumerable<CashFlowViewModel>>(this.context.CashFlow.AsEnumerable().Where(x => (x.ClientId == clientId) && (x.Cftype == "I")));
        //            cashFlowExpenditure = this.mapper.Map<IEnumerable<CashFlowViewModel>>(this.context.CashFlow.AsEnumerable().Where(x => (x.ClientId == clientId) && (x.Cftype == "E")));
        //            marginalTaxRates = this.mapper.Map<IEnumerable<MarginalTaxRatesViewModel>>(this.context.MarginalTaxRates.AsEnumerable());
        //            investments = this.mapper.Map<IEnumerable<InvestmentViewModel>>(this.context.Investment.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            generalAssumptions = this.mapper.Map<IEnumerable<GeneralViewModel>>(this.context.General.AsEnumerable());
        //            investmentCW = this.mapper.Map<IEnumerable<InvestmentDetailsViewModel>>(this.context.InvestmentDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            properties = this.mapper.Map<IEnumerable<PropertyViewModel>>(this.context.Property.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            supers = this.mapper.Map<IEnumerable<SuperViewModel>>(this.context.Super.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            superDetails = this.mapper.Map<IEnumerable<SuperDetailsViewModel>>(this.context.SuperDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            sgcRates = this.mapper.Map<IEnumerable<SgcrateViewModel>>(this.context.Sgcrate.AsEnumerable());
        //            superAssumptions = this.mapper.Map<IEnumerable<SuperAssumptionsViewModel>>(this.context.SuperAssumptions.AsEnumerable());
        //            liabilities = this.mapper.Map<IEnumerable<LiabilityViewModel>>(this.context.Liability.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            liabilityDD = this.mapper.Map<IEnumerable<LiabilityDetailsViewModel>>(this.context.LiabilityDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            pensions = this.mapper.Map<IEnumerable<PensionViewModel>>(this.context.Pension.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            pensionDD = this.mapper.Map<IEnumerable<PensionDetailsViewModel>>(this.context.PensionDetails.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            preservationAge = this.mapper.Map<IEnumerable<PreservationAgeViewModel>>(this.context.PreservationAge.AsEnumerable());
        //            minimumPensionDrawdown = this.mapper.Map<IEnumerable<MinimumPensionDrawdownViewModel>>(this.context.MinimumPensionDrawdown.AsEnumerable());
        //            lifestyleAssets = this.mapper.Map<IEnumerable<LifestyleAssetViewModel>>(this.context.LifestyleAsset.AsEnumerable().Where(x => (x.ClientId == clientId)));
        //            assetAssumptions = this.mapper.Map<IEnumerable<AssetTypesViewModel>>(this.context.AssetTypes.AsEnumerable());
        //            qualifyingAge = this.mapper.Map<IEnumerable<QualifyingAgeViewModel>>(this.context.QualifyingAge.AsEnumerable());


        //            marginalTaxRates = marginalTaxRates.OrderByDescending(i => i.Index);

        //            sgcRates = sgcRates.OrderByDescending(i => i.Sgcrate1);

        //            cfiClient = cashFlowIncome.Where(c => c.Owner == "Client");
        //            cfiPartner = cashFlowIncome.Where(c => c.Owner == "Partner");

        //            cfeClient = cashFlowExpenditure.Where(c => c.Owner == "Client");
        //            cfePartner = cashFlowExpenditure.Where(c => c.Owner == "Partner");
        //            cfeJoint = cashFlowExpenditure.Where(c => c.Owner == "Joint");

        //            //PreTaxExpenditures for tax
        //            EPRTClient = cashFlowExpenditure.Where(c => c.Owner == "Client").Where(r => r.Type == "Pre-tax");
        //            EPRTJoint = cashFlowExpenditure.Where(c => c.Owner == "Joint").Where(r => r.Type == "Pre-tax");
        //            EPRTPartner = cashFlowExpenditure.Where(c => c.Owner == "Partner").Where(r => r.Type == "Pre-tax");

        //            //Investments
        //            investmentClient = investments.Where(c => c.Owner == "Client");
        //            investmentClientOptimized = investments.Where(c => c.Owner == "Client");
        //            investmentPartner = investments.Where(c => c.Owner == "Partner");
        //            investmentPartnerOptimized = investments.Where(c => c.Owner == "Partner");
        //            investmentJoint = investments.Where(c => c.Owner == "Joint");
        //            investmentJointOptimized = investments.Where(c => c.Owner == "Joint");

        //            //Properties
        //            propertiesClient = properties.Where(c => c.Owner == "Client");
        //            propertiesPartner = properties.Where(c => c.Owner == "Partner");
        //            propertiesJoint = properties.Where(c => c.Owner == "Joint");

        //            //Supers
        //            superClient = supers.Where(c => c.Owner == "Client");
        //            superPartner = supers.Where(c => c.Owner == "Partner");

        //            //Liabilities
        //            liabilityClient = liabilities.Where(c => c.Owner == "Client");
        //            liabilityPartner = liabilities.Where(c => c.Owner == "Partner");
        //            liabilityJoint = liabilities.Where(c => c.Owner == "Joint");


        //            //Pensions
        //            pensionClient = pensions.Where(c => c.Owner == "Client");
        //            pensionPartner = pensions.Where(c => c.Owner == "Partner");

        //            lifestyleClient = lifestyleAssets.Where(c => c.Owner == "Client");
        //            lifestylePartner = lifestyleAssets.Where(c => c.Owner == "Partner");
        //            lifestyleJoint = lifestyleAssets.Where(c => c.Owner == "Joint");

        //            var indexRangeInflow = new List<dynamic>();
        //            var indexRangeOutflow = new List<dynamic>();

        //            var clientEmploymentIncome = new List<dynamic>();
        //            var partnerEmploymentIncome = new List<dynamic>();

        //            var m = 1;

        //            for (var i = 0; i < clientDetails.Period; i++)
        //            {
        //                for (var q = 0; q <= 1; q++)
        //                {


        //                    //TO DO : Get Highest.
        //                    var highestVal = 0;
        //                    var highestValObject = new List<InvestmentViewModel>();
        //                    //    highestVal = this.investmentClient.find Math.max(this.investmentClient.map((o: any) => o.value));
        //                    if (clientDetails.MaritalStatus == "S")
        //                    {
        //                        highestValObject = investmentClient.Where(c => c.Type == "Domestic Cash").ToList();
        //                        if (highestValObject != null && highestValObject.Count() != 0)
        //                        {
        //                            highestVal = highestValObject[0].InvestmentId;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        highestValObject = investmentJoint.Where(c => c.Type == "Domestic Cash").ToList();
        //                        if (highestValObject != null && highestValObject.Count() != 0)
        //                        {
        //                            highestVal = highestValObject[0].InvestmentId;
        //                        }
        //                        else
        //                        {
        //                            highestValObject = investmentClient.Where(c => c.Type == "Domestic Cash").ToList();
        //                            if (highestValObject != null && highestValObject.Count() != 0)
        //                            {
        //                                highestVal = highestValObject[0].InvestmentId;
        //                            }
        //                            else
        //                            {
        //                                highestValObject = investmentPartner.Where(c => c.Type == "Domestic Cash").ToList();
        //                                if (highestValObject != null && highestValObject.Count() != 0)
        //                                {
        //                                    highestVal = highestValObject[0].InvestmentId;
        //                                }
        //                            }
        //                        }
        //                    }


        //                    foreach(LifestyleAssetViewModel x in lifestyleClient)
        //                    {


        //                        dynamic obj = lifestyles.Where(y => y.id == x.LassetId).First();
        //                        dynamic obj1 = new JObject();
        //                        dynamic purchaseOfAssets = new JObject();
        //                        dynamic saleOfAssets = new JObject();                           
        //                        var j = 0;
        //                        if (obj == null)
        //                        {
        //                            obj = new JObject();
        //                            j = 0;
        //                        }
        //                        else
        //                        {
        //                            obj1 = obj.values;
        //                            purchaseOfAssets = obj.purchaseOfAssetValues;
        //                            saleOfAssets = obj.saleOfAssetValues;
        //                            j = obj.increment;
        //                        }

        //                        if (q == 0)
        //                        {
        //                            obj["owner"] = x.Owner;
        //                            obj["name"] = x.Name;

        //                            obj["id"] = x.LassetId;
        //                            obj["type"] = x.LassetType;

        //                            if (x.StartDateType == "Start" || x.StartDateType == "Existing")
        //                            {
        //                                x.StartDate = clientDetails.StartDate;
        //                            }
        //                            else if (x.StartDateType == "Client Retirement")
        //                            {
        //                                x.StartDate = clientDetails.ClientRetirementYear - 1;
        //                            }


        //                            if (x.EndDateType == "End")
        //                            {
        //                                x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period - 1);
        //                            }
        //                            else if (x.EndDateType == "Retain")
        //                            {
        //                                x.EndDate = Convert.ToInt32(clientDetails.StartDate) + Convert.ToInt32(clientDetails.Period);
        //                            }
        //                            else if (x.EndDateType == "Client Retirement")
        //                            {
        //                                x.EndDate = clientDetails.ClientRetirementYear - 1;
        //                            }

        //                            if ((clientDetails.StartDate + i) >= x.EndDate)
        //                            {
        //                                obj1[clientDetails.StartDate + i] = 0;
        //                            }


        //                            else if (x.StartDate <= clientDetails.StartDate + i && x.EndDate >= clientDetails.StartDate + i)
        //                            {

        //                                if (x.LassetType == "PrimaryResidence")
        //                                {
        //                                    if (j == 0)
        //                                    {
        //                                        obj1[clientDetails.StartDate + i] = Math.Round(Convert.ToDecimal(x.Value));

        //                                    }
        //                                    else
        //                                    {
        //                                        obj1[clientDetails.StartDate + i] = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j))); 
        //                                    }
        //                                    j++;
        //                                }
        //                                else
        //                                {
        //                                    obj1[clientDetails.StartDate + i] = Math.Round(Convert.ToDecimal(x.Value));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                obj1[clientDetails.StartDate + i] = 0;
        //                            }

        //                            //Purchase of assets
        //                            if (x.StartDateType != "Existing")
        //                            {

        //                                if (x.StartDate == clientDetails.StartDate + i)
        //                                {
        //                                    purchaseOfAssets[clientDetails.StartDate + i] = Math.Round(Convert.ToDecimal(x.Value));
        //                                }
        //                                else
        //                                {
        //                                    purchaseOfAssets[clientDetails.StartDate + i] = 0;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                purchaseOfAssets[clientDetails.StartDate + i] = 0;
        //                            }

        //                            //Sale of assets
        //                            if (x.EndDateType != "Retain")
        //                            {

        //                                if (x.EndDate == clientDetails.StartDate + i)
        //                                {
        //                                    if (x.LassetType == "PrimaryResidence")
        //                                    {
        //                                        if (j == 0)
        //                                        {
        //                                            saleOfAssets[clientDetails.StartDate + i] = Math.Round(Convert.ToDecimal(x.Value));

        //                                        }
        //                                        else
        //                                        {
        //                                            saleOfAssets[clientDetails.StartDate + i] = Math.Round(x.Value * (Math.Pow(Convert.ToDouble(1 + x.Growth / 100), j)));
        //                                        }
        //                                        j++;
        //                                    }
        //                                    else
        //                                    {
        //                                        saleOfAssets[clientDetails.StartDate + i] = Math.Round(Convert.ToDecimal(x.Value));
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    saleOfAssets[clientDetails.StartDate + i] = 0;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                saleOfAssets[clientDetails.StartDate + i] = 0;
        //                            }
        //                        }

        //                        obj["values"] = obj1;
        //                        obj["increment"] = j;
        //                        obj["purchaseOfAssetValues"] = purchaseOfAssets;
        //                        obj["saleOfAssetValues"] = saleOfAssets;


        //                        if (lifestyles.Where(y => y.id == x.LassetId).First() != null)
        //                        {
        //                            lifestyles[lifestyles.FindIndex(y => y.id == x.LassetId)] = obj;
        //                        }
        //                        else
        //                        {
        //                            lifestyles.Add(obj);

        //                        }
        //                    }


        //        }

        //        m++;
        //    }


        //}



        //        return new List<AlternativeClientProductsViewModel>();
        //    }







    }
}
