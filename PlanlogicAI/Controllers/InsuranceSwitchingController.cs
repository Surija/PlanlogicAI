using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanlogicAI.Data;
using PlanlogicAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace PlanlogicAI.Controllers
{
    [Route("/api/insuranceSwitching")]
    public class InsuranceSwitchingController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public InsuranceSwitchingController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ProposedInsuranceViewModel> GetAllProposedProducts()
        {
            List<ProposedInsuranceViewModel> proposedInsurance = new List<ProposedInsuranceViewModel>();
            IEnumerable<ProposedInsuranceProductsViewModel> list = this.mapper.Map<IEnumerable<ProposedInsuranceProductsViewModel>>(this.context.ProposedInsuranceProducts.AsEnumerable());
            foreach (ProposedInsuranceProductsViewModel proposed in list)
            {
                ProposedInsuranceViewModel p = new ProposedInsuranceViewModel();
                p.RecId = proposed.RecId;
                p.ClientId = proposed.RecId;
                p.Provider = proposed.Provider;
                p.Owner = proposed.Owner;
                p.ReplacementId = proposed.ReplacementId;

                List<ProposedFeeDetailsViewModel> feeDetails = this.mapper.Map<List<ProposedFeeDetailsViewModel>>(this.context.ProposedFeeDetails.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                List<ProposedLifeCoverViewModel> lifeCover = this.mapper.Map<List<ProposedLifeCoverViewModel>>(this.context.ProposedLifeCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                List<ProposedTpdCoverViewModel> tpdCover = this.mapper.Map<List<ProposedTpdCoverViewModel>>(this.context.ProposedTpdCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                List<ProposedTraumaCoverViewModel> traumaCover = this.mapper.Map<List<ProposedTraumaCoverViewModel>>(this.context.ProposedTraumaCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                List<ProposedIncomeCoverViewModel> incomeCover = this.mapper.Map<List<ProposedIncomeCoverViewModel>>(this.context.ProposedIncomeCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                CostOfAdviceViewModel implementation = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "implementation")).FirstOrDefault()) == null ? new CostOfAdviceViewModel() : (this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "implementation")).FirstOrDefault()));
                CostOfAdviceViewModel ongoing = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "ongoing")).FirstOrDefault()) == null ? new CostOfAdviceViewModel() : this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "ongoing")).FirstOrDefault());

                List<InsuranceReplacementViewModel> replacement = this.mapper.Map<List<InsuranceReplacementViewModel>>(this.context.InsuranceReplacement.AsEnumerable().Where(m => (m.ProposedId == proposed.RecId)));


                p.FeeDetails = feeDetails;
                p.LifeCover = lifeCover;
                p.TpdCover = tpdCover;
                p.TraumaCover = traumaCover;
                p.IncomeCover = incomeCover;
                p.Implementation = implementation;
                p.Ongoing = ongoing;
                p.Replacement = replacement;


                proposedInsurance.Add(p);
            }

            return proposedInsurance;
        }

        [HttpGet("{id}")]
        public IEnumerable<CurrentInsuranceViewModel> GetCurrentInsurance(int id)
        {

            List<CurrentInsuranceViewModel> currentInsurance = new List<CurrentInsuranceViewModel>();
            IEnumerable<CurrentInsuranceProductsViewModel> list = this.mapper.Map<IEnumerable<CurrentInsuranceProductsViewModel>>(this.context.CurrentInsuranceProducts.AsEnumerable().Where(m => (m.ClientId == id)));
            foreach (CurrentInsuranceProductsViewModel current in list)
            {
                CurrentInsuranceViewModel c = new CurrentInsuranceViewModel();
                c.RecId = current.RecId;
                c.ClientId = current.RecId;
                c.Provider = current.Provider;
                c.Owner = current.Owner;
                
                List<CurrentFeeDetailsViewModel> feeDetails = this.mapper.Map<List<CurrentFeeDetailsViewModel>>(this.context.CurrentFeeDetails.AsEnumerable().Where(m => (m.HeaderId == current.RecId)));
                List<CurrentLifeCoverViewModel> lifeCover = this.mapper.Map<List<CurrentLifeCoverViewModel>>(this.context.CurrentLifeCover.AsEnumerable().Where(m => (m.HeaderId == current.RecId)));
                List<CurrentTpdCoverViewModel> tpdCover = this.mapper.Map<List<CurrentTpdCoverViewModel>>(this.context.CurrentTpdCover.AsEnumerable().Where(m => (m.HeaderId == current.RecId)));
                List<CurrentTraumaCoverViewModel> traumaCover = this.mapper.Map<List<CurrentTraumaCoverViewModel>>(this.context.CurrentTraumaCover.AsEnumerable().Where(m => (m.HeaderId == current.RecId)));
                List<CurrentIncomeCoverViewModel> incomeCover = this.mapper.Map<List<CurrentIncomeCoverViewModel>>(this.context.CurrentIncomeCover.AsEnumerable().Where(m => (m.HeaderId == current.RecId)));

                c.FeeDetails = feeDetails;
                c.LifeCover = lifeCover;
                c.TpdCover = tpdCover;
                c.TraumaCover = traumaCover;
                c.IncomeCover = incomeCover;

                currentInsurance.Add(c);
            }

                return currentInsurance;

        }

        [HttpGet("{id}/{isProposed}")]
        public IEnumerable<ProposedInsuranceViewModel> GetProposedInsurance(int id,int isProposed)
        {
            try
            {
                List<ProposedInsuranceViewModel> proposedInsurance = new List<ProposedInsuranceViewModel>();
                IEnumerable<ProposedInsuranceProductsViewModel> list = this.mapper.Map<IEnumerable<ProposedInsuranceProductsViewModel>>(this.context.ProposedInsuranceProducts.AsEnumerable().Where(m => (m.ClientId == id)));
                foreach (ProposedInsuranceProductsViewModel proposed in list)
                {
                    ProposedInsuranceViewModel p = new ProposedInsuranceViewModel();
                    p.RecId = proposed.RecId;
                    p.ClientId = proposed.RecId;
                    p.Provider = proposed.Provider;
                    p.Owner = proposed.Owner;
                    p.ReplacementId = proposed.ReplacementId;

                    List<ProposedFeeDetailsViewModel> feeDetails = this.mapper.Map<List<ProposedFeeDetailsViewModel>>(this.context.ProposedFeeDetails.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                    List<ProposedLifeCoverViewModel> lifeCover = this.mapper.Map<List<ProposedLifeCoverViewModel>>(this.context.ProposedLifeCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                    List<ProposedTpdCoverViewModel> tpdCover = this.mapper.Map<List<ProposedTpdCoverViewModel>>(this.context.ProposedTpdCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                    List<ProposedTraumaCoverViewModel> traumaCover = this.mapper.Map<List<ProposedTraumaCoverViewModel>>(this.context.ProposedTraumaCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                    List<ProposedIncomeCoverViewModel> incomeCover = this.mapper.Map<List<ProposedIncomeCoverViewModel>>(this.context.ProposedIncomeCover.AsEnumerable().Where(m => (m.HeaderId == proposed.RecId)));
                    CostOfAdviceViewModel implementation = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "implementation")).FirstOrDefault()) == null ? new CostOfAdviceViewModel() : (this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "implementation")).FirstOrDefault()));
                    CostOfAdviceViewModel ongoing = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "ongoing")).FirstOrDefault()) == null ? new CostOfAdviceViewModel() : this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(m => (m.HeaderId == proposed.RecId && m.CoaType == "ongoing")).FirstOrDefault());
                    List<InsuranceReplacementViewModel> replacement = this.mapper.Map<List<InsuranceReplacementViewModel>>(this.context.InsuranceReplacement.AsEnumerable().Where(m => (m.ProposedId == proposed.RecId)));


                    p.FeeDetails = feeDetails;
                    p.LifeCover = lifeCover;
                    p.TpdCover = tpdCover;
                    p.TraumaCover = traumaCover;
                    p.IncomeCover = incomeCover;
                    p.Implementation = implementation;
                    p.Ongoing = ongoing;
                    p.Replacement = replacement;

                    proposedInsurance.Add(p);
                }

                return proposedInsurance;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        [HttpPut("{id}/{clientId}/{b}")]
        public IActionResult Retain(int id,int clientId, [FromBody] ProposedInsuranceViewModel _product)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var existingProduct = this.context.ProposedInsuranceProducts.Where(b => b.RecId == _product.RecId).FirstOrDefault();
                        if (existingProduct == null)
                        {
                            ProposedInsuranceViewModel newProduct = new ProposedInsuranceViewModel();


                            ProposedInsuranceProducts product = new ProposedInsuranceProducts();
                            product.ClientId = clientId;
                            product.Provider = _product.Provider;
                            product.Owner = _product.Owner;
                            product.ReplacementId = _product.ReplacementId;

                            this.context.ProposedInsuranceProducts.Add(product);
                            this.context.SaveChanges();


                            var header = this.mapper.Map<ProposedInsuranceProductsViewModel>(product);

                            newProduct.Provider = header.Provider;
                            newProduct.RecId = header.RecId;
                            newProduct.Owner = header.Owner;
                            newProduct.ClientId = header.ClientId;
                            newProduct.ReplacementId = header.ReplacementId;

                            IEnumerable<CurrentFeeDetailsViewModel> list = this.mapper.Map<IEnumerable<CurrentFeeDetailsViewModel>>(this.context.CurrentFeeDetails.AsEnumerable().Where(m => (m.HeaderId == id)));

                            foreach (CurrentFeeDetailsViewModel feeDetails in list)
                            {
                                if (feeDetails != null)
                                {
                                    ProposedFeeDetailsViewModel pf = mapper.Map<ProposedFeeDetailsViewModel>(feeDetails);
                                    pf.HeaderId = header.RecId;
                                    var pfDetails = this.mapper.Map<ProposedFeeDetails>(pf);
                                    this.context.ProposedFeeDetails.Add(pfDetails);
                                }
                            }

                            IEnumerable<CurrentLifeCoverViewModel> list1 = this.mapper.Map<IEnumerable<CurrentLifeCoverViewModel>>(this.context.CurrentLifeCover.AsEnumerable().Where(m => (m.HeaderId == id)));

                            foreach (CurrentLifeCoverViewModel lDetails in list1)
                            {
                                if (lDetails != null)
                                {
                                    ProposedLifeCoverViewModel pf = mapper.Map<ProposedLifeCoverViewModel>(lDetails);
                                    pf.HeaderId = header.RecId;
                                    var lcDetails = this.mapper.Map<ProposedLifeCover>(pf);
                                    this.context.ProposedLifeCover.Add(lcDetails);
                                }
                            }

                            IEnumerable<CurrentTpdCoverViewModel> list2 = this.mapper.Map<IEnumerable<CurrentTpdCoverViewModel>>(this.context.CurrentTpdCover.AsEnumerable().Where(m => (m.HeaderId == id)));

                            foreach (CurrentTpdCoverViewModel tDetails in list2)
                            {
                                if (tDetails != null)
                                {
                                    ProposedTpdCoverViewModel pf = mapper.Map<ProposedTpdCoverViewModel>(tDetails);
                                    pf.HeaderId = header.RecId;
                                    var tcDetails = this.mapper.Map<ProposedTpdCover>(pf);
                                    this.context.ProposedTpdCover.Add(tcDetails);
                                }
                            }

                            IEnumerable<CurrentTraumaCoverViewModel> list3 = this.mapper.Map<IEnumerable<CurrentTraumaCoverViewModel>>(this.context.CurrentTraumaCover.AsEnumerable().Where(m => (m.HeaderId == id)));

                            foreach (CurrentTraumaCoverViewModel trDetails in list3)
                            {
                                if (trDetails != null)
                                {
                                    ProposedTraumaCoverViewModel pf = mapper.Map<ProposedTraumaCoverViewModel>(trDetails);
                                    pf.HeaderId = header.RecId;
                                    var trcDetails = this.mapper.Map<ProposedTraumaCover>(pf);
                                    this.context.ProposedTraumaCover.Add(trcDetails);
                                }
                            }

                            IEnumerable<CurrentIncomeCoverViewModel> list4 = this.mapper.Map<IEnumerable<CurrentIncomeCoverViewModel>>(this.context.CurrentIncomeCover.AsEnumerable().Where(m => (m.HeaderId == id)));

                            foreach (CurrentIncomeCoverViewModel iDetails in list4)
                            {
                                if (iDetails != null)
                                {
                                    ProposedIncomeCoverViewModel pf = mapper.Map<ProposedIncomeCoverViewModel>(iDetails);
                                    pf.HeaderId = header.RecId;
                                    var icDetails = this.mapper.Map<ProposedIncomeCover>(pf);
                                    this.context.ProposedIncomeCover.Add(icDetails);
                                }
                            }

                            this.context.SaveChanges();

                            dbContextTransaction.Commit();

                            newProduct.FeeDetails = this.mapper.Map<List<ProposedFeeDetails>, List<ProposedFeeDetailsViewModel>>(this.context.ProposedFeeDetails.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.LifeCover = this.mapper.Map<List<ProposedLifeCover>, List<ProposedLifeCoverViewModel>>(this.context.ProposedLifeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TpdCover = this.mapper.Map<List<ProposedTpdCover>, List<ProposedTpdCoverViewModel>>(this.context.ProposedTpdCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TraumaCover = this.mapper.Map<List<ProposedTraumaCover>, List<ProposedTraumaCoverViewModel>>(this.context.ProposedTraumaCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.IncomeCover = this.mapper.Map<List<ProposedIncomeCover>, List<ProposedIncomeCoverViewModel>>(this.context.ProposedIncomeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.Implementation = new CostOfAdviceViewModel();
                            newProduct.Ongoing = new CostOfAdviceViewModel();
                            newProduct.Replacement.Add(new InsuranceReplacementViewModel());


                            var result = newProduct;
                            return Ok(result);

                           
                        }
                        else
                        {
                            //this.mapper.Map<ProposedClientProductsViewModel, ProposedClientProducts>(_product, existingProduct);

                            //this.context.ProposedClientProducts.Update(existingProduct);
                            //this.context.SaveChanges();

                            var result = this.mapper.Map<ProposedInsuranceViewModel>(_product);
                            return Ok(result);
                        }


                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ModelState);
                    //Log, handle or absorbe I don't care ^_^
                }
            }
        }


        [HttpPut("{id}")]
        public IActionResult AddCurrentInsurance(int id, [FromBody] CurrentInsuranceViewModel _data)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingProduct = this.context.CurrentInsuranceProducts.Where(b => b.RecId == _data.RecId).FirstOrDefault();
                        if (existingProduct == null)
                        {
                            CurrentInsuranceViewModel newProduct = new CurrentInsuranceViewModel();

                            CurrentInsuranceProducts product = new CurrentInsuranceProducts();
                            product.ClientId = id;
                            product.Provider = _data.Provider;
                            product.Owner = _data.Owner;

                            this.context.CurrentInsuranceProducts.Add(product);
                            this.context.SaveChanges();

                            var header = this.mapper.Map<CurrentInsuranceProductsViewModel>(product);

                            newProduct.Provider = header.Provider;
                            newProduct.RecId = header.RecId;
                            newProduct.Owner = header.Owner;
                            newProduct.ClientId = header.ClientId;

                            foreach (CurrentFeeDetailsViewModel feeDetail in _data.FeeDetails)
                            {
                                feeDetail.HeaderId = header.RecId;
                                this.context.CurrentFeeDetails.Add(this.mapper.Map<CurrentFeeDetails>(feeDetail));
                            }

                            foreach (CurrentLifeCoverViewModel lifeCover in _data.LifeCover)
                            {
                                lifeCover.HeaderId = header.RecId;
                                this.context.CurrentLifeCover.Add(this.mapper.Map<CurrentLifeCover>(lifeCover));
                            }
                         
                            foreach (CurrentTpdCoverViewModel tpdCover in _data.TpdCover)
                            {
                                tpdCover.HeaderId = header.RecId;
                                this.context.CurrentTpdCover.Add(this.mapper.Map<CurrentTpdCover>(tpdCover));
                            }
                          
                            foreach (CurrentTraumaCoverViewModel traumaCover in _data.TraumaCover)
                            {
                                traumaCover.HeaderId = header.RecId;                           
                                this.context.CurrentTraumaCover.Add(this.mapper.Map<CurrentTraumaCover>(traumaCover));
                           
                            }
                        
                            foreach (CurrentIncomeCoverViewModel incomeCover in _data.IncomeCover)
                            {
                                incomeCover.HeaderId = header.RecId;                 
                                this.context.CurrentIncomeCover.Add(this.mapper.Map<CurrentIncomeCover>(incomeCover));
                            }
                           

                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            newProduct.FeeDetails = this.mapper.Map<List<CurrentFeeDetails>, List<CurrentFeeDetailsViewModel>>(this.context.CurrentFeeDetails.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.LifeCover = this.mapper.Map<List<CurrentLifeCover>, List<CurrentLifeCoverViewModel>>(this.context.CurrentLifeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TpdCover = this.mapper.Map<List<CurrentTpdCover>, List<CurrentTpdCoverViewModel>>(this.context.CurrentTpdCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TraumaCover = this.mapper.Map<List<CurrentTraumaCover>, List<CurrentTraumaCoverViewModel>>(this.context.CurrentTraumaCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.IncomeCover = this.mapper.Map<List<CurrentIncomeCover>, List<CurrentIncomeCoverViewModel>>(this.context.CurrentIncomeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());

                            var result = newProduct;
                            return Ok(result);
                        }
                        else
                        {
                            CurrentInsuranceViewModel newProduct = new CurrentInsuranceViewModel();

                            existingProduct.ClientId = id;
                            existingProduct.Provider = _data.Provider;
                            existingProduct.Owner = _data.Owner;
                          
                            this.context.CurrentInsuranceProducts.Update(existingProduct);
                            this.context.SaveChanges();

                            newProduct.Provider = existingProduct.Provider;
                            newProduct.RecId = existingProduct.RecId;
                            newProduct.Owner = existingProduct.Owner;
                            newProduct.ClientId = existingProduct.ClientId;

                      
                            foreach (CurrentFeeDetailsViewModel feeDetail in _data.FeeDetails)
                            {
                                var existingFee = this.context.CurrentFeeDetails.Where(b => b.RecId == feeDetail.RecId).FirstOrDefault();
                                if (existingFee == null)
                                {
                                    feeDetail.HeaderId = existingProduct.RecId;
                                    this.context.CurrentFeeDetails.Add(this.mapper.Map<CurrentFeeDetails>(feeDetail));
                                }
                                else
                                {
                                    this.mapper.Map<CurrentFeeDetailsViewModel, CurrentFeeDetails>(feeDetail, existingFee);
                                    this.context.CurrentFeeDetails.Update(existingFee);
                                }
                            }

                            foreach (CurrentLifeCoverViewModel lifeCover in _data.LifeCover)
                            {
                                var existingLC = this.context.CurrentLifeCover.Where(b => b.RecId == lifeCover.RecId).FirstOrDefault();
                                if (existingLC == null)
                                {
                                    lifeCover.HeaderId = existingProduct.RecId;
                                    this.context.CurrentLifeCover.Add(this.mapper.Map<CurrentLifeCover>(lifeCover));
                                }
                                else
                                {
                                    this.mapper.Map<CurrentLifeCoverViewModel, CurrentLifeCover>(lifeCover, existingLC);
                                    this.context.CurrentLifeCover.Update(existingLC);
                                }
                            }

                            foreach (CurrentTpdCoverViewModel tpdCover in _data.TpdCover)
                            {
                                var existingTpd = this.context.CurrentTpdCover.Where(b => b.RecId == tpdCover.RecId).FirstOrDefault();
                                if (existingTpd == null)
                                {
                                    tpdCover.HeaderId = existingProduct.RecId;
                                    this.context.CurrentTpdCover.Add(this.mapper.Map<CurrentTpdCover>(tpdCover));
                                }
                                else
                                {
                                    this.mapper.Map<CurrentTpdCoverViewModel, CurrentTpdCover>(tpdCover, existingTpd);
                                    this.context.CurrentTpdCover.Update(existingTpd);
                                }
                            }

                            foreach (CurrentTraumaCoverViewModel traumaCover in _data.TraumaCover)
                            {
                                var existingTr = this.context.CurrentTraumaCover.Where(b => b.RecId == traumaCover.RecId).FirstOrDefault();
                                if (existingTr == null)
                                {
                                    traumaCover.HeaderId = existingProduct.RecId;
                                    this.context.CurrentTraumaCover.Add(this.mapper.Map<CurrentTraumaCover>(traumaCover));
                                }
                                else
                                {
                                    this.mapper.Map<CurrentTraumaCoverViewModel, CurrentTraumaCover>(traumaCover, existingTr);
                                    this.context.CurrentTraumaCover.Update(existingTr);
                                }

                            }

                            foreach (CurrentIncomeCoverViewModel incomeCover in _data.IncomeCover)
                            {
                                var existingIncome = this.context.CurrentIncomeCover.Where(b => b.RecId == incomeCover.RecId).FirstOrDefault();
                                if (existingIncome == null)
                                {
                                    incomeCover.HeaderId = existingProduct.RecId;
                                    this.context.CurrentIncomeCover.Add(this.mapper.Map<CurrentIncomeCover>(incomeCover));
                                }
                                else
                                {
                                    this.mapper.Map<CurrentIncomeCoverViewModel, CurrentIncomeCover>(incomeCover, existingIncome);
                                    this.context.CurrentIncomeCover.Update(existingIncome);
                                }
                            }


                            var fd = (from e in this.context.CurrentFeeDetails.Where(r => r.HeaderId == existingProduct.RecId)
                                        where !_data.FeeDetails.Any(f => f.RecId == e.RecId)
                                        select e).ToList();
                            this.context.CurrentFeeDetails.RemoveRange(fd);

                            var lc = (from e in this.context.CurrentLifeCover.Where(r => r.HeaderId == existingProduct.RecId)
                                        where !_data.LifeCover.Any(f => f.RecId == e.RecId)
                                        select e).ToList();
                            this.context.CurrentLifeCover.RemoveRange(lc);

                            var tc = (from e in this.context.CurrentTpdCover.Where(r => r.HeaderId == existingProduct.RecId)
                                        where !_data.TpdCover.Any(f => f.RecId == e.RecId)
                                        select e).ToList();
                            this.context.CurrentTpdCover.RemoveRange(tc);

                            var trc = (from e in this.context.CurrentTraumaCover.Where(r => r.HeaderId == existingProduct.RecId)
                                        where !_data.TraumaCover.Any(f => f.RecId == e.RecId)
                                        select e).ToList();
                            this.context.CurrentTraumaCover.RemoveRange(trc);

                            var ic = (from e in this.context.CurrentIncomeCover.Where(r => r.HeaderId == existingProduct.RecId)
                                        where !_data.IncomeCover.Any(f => f.RecId == e.RecId)
                                        select e).ToList();
                            this.context.CurrentIncomeCover.RemoveRange(ic);


                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            newProduct.FeeDetails = this.mapper.Map<List<CurrentFeeDetails>, List<CurrentFeeDetailsViewModel>>(this.context.CurrentFeeDetails.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.LifeCover = this.mapper.Map<List<CurrentLifeCover>, List<CurrentLifeCoverViewModel>>(this.context.CurrentLifeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TpdCover = this.mapper.Map<List<CurrentTpdCover>, List<CurrentTpdCoverViewModel>>(this.context.CurrentTpdCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TraumaCover = this.mapper.Map<List<CurrentTraumaCover>, List<CurrentTraumaCoverViewModel>>(this.context.CurrentTraumaCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.IncomeCover = this.mapper.Map<List<CurrentIncomeCover>, List<CurrentIncomeCoverViewModel>>(this.context.CurrentIncomeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());

                            var result = newProduct;
                            return Ok(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ModelState);
                        //Log, handle or absorbe I don't care ^_^
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}/{isProposed}")]
        public IActionResult AddProposedInsurance(int id, [FromBody] ProposedInsuranceViewModel _data)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingProduct = this.context.ProposedInsuranceProducts.Where(b => b.RecId == _data.RecId).FirstOrDefault();
                        if (existingProduct == null)
                        {
                            ProposedInsuranceViewModel newProduct = new ProposedInsuranceViewModel();

                            ProposedInsuranceProducts product = new ProposedInsuranceProducts();
                            product.ClientId = id;
                            product.Provider = _data.Provider;
                            product.Owner = _data.Owner;
                            product.ReplacementId = _data.ReplacementId;

                            this.context.ProposedInsuranceProducts.Add(product);
                            this.context.SaveChanges();

                            var header = this.mapper.Map<ProposedInsuranceProductsViewModel>(product);

                            newProduct.Provider = header.Provider;
                            newProduct.RecId = header.RecId;
                            newProduct.Owner = header.Owner;
                            newProduct.ClientId = header.ClientId;
                            newProduct.ReplacementId = header.ReplacementId;

                            foreach (ProposedFeeDetailsViewModel feeDetail in _data.FeeDetails)
                            {
                                feeDetail.HeaderId = header.RecId;
                                this.context.ProposedFeeDetails.Add(this.mapper.Map<ProposedFeeDetails>(feeDetail));
                            }

                            foreach (ProposedLifeCoverViewModel lifeCover in _data.LifeCover)
                            {
                                lifeCover.HeaderId = header.RecId;
                                this.context.ProposedLifeCover.Add(this.mapper.Map<ProposedLifeCover>(lifeCover));
                            }

                            foreach (ProposedTpdCoverViewModel tpdCover in _data.TpdCover)
                            {
                                tpdCover.HeaderId = header.RecId;
                                this.context.ProposedTpdCover.Add(this.mapper.Map<ProposedTpdCover>(tpdCover));
                            }

                            foreach (ProposedTraumaCoverViewModel traumaCover in _data.TraumaCover)
                            {
                                traumaCover.HeaderId = header.RecId;
                                this.context.ProposedTraumaCover.Add(this.mapper.Map<ProposedTraumaCover>(traumaCover));

                            }

                            foreach (ProposedIncomeCoverViewModel incomeCover in _data.IncomeCover)
                            {
                                incomeCover.HeaderId = header.RecId;
                                this.context.ProposedIncomeCover.Add(this.mapper.Map<ProposedIncomeCover>(incomeCover));
                            }

                            foreach (InsuranceReplacementViewModel rep in _data.Replacement)
                            {
                                rep.ProposedId = header.RecId;
                                this.context.InsuranceReplacement.Add(this.mapper.Map<InsuranceReplacement>(rep));
                            }


                            _data.Implementation.HeaderId = header.RecId;
                            this.context.CostOfAdvice.Add(this.mapper.Map<CostOfAdvice>(_data.Implementation));
                            _data.Ongoing.HeaderId = header.RecId;
                            this.context.CostOfAdvice.Add(this.mapper.Map<CostOfAdvice>(_data.Ongoing));


                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            newProduct.FeeDetails = this.mapper.Map<List<ProposedFeeDetails>, List<ProposedFeeDetailsViewModel>>(this.context.ProposedFeeDetails.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.LifeCover = this.mapper.Map<List<ProposedLifeCover>, List<ProposedLifeCoverViewModel>>(this.context.ProposedLifeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TpdCover = this.mapper.Map<List<ProposedTpdCover>, List<ProposedTpdCoverViewModel>>(this.context.ProposedTpdCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TraumaCover = this.mapper.Map<List<ProposedTraumaCover>, List<ProposedTraumaCoverViewModel>>(this.context.ProposedTraumaCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.IncomeCover = this.mapper.Map<List<ProposedIncomeCover>, List<ProposedIncomeCoverViewModel>>(this.context.ProposedIncomeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.Implementation = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(r => r.HeaderId == newProduct.RecId && r.CoaType == "implementation").FirstOrDefault());
                            newProduct.Ongoing = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(r => r.HeaderId == newProduct.RecId && r.CoaType == "ongoing").FirstOrDefault());
                           newProduct.Replacement = this.mapper.Map<List<InsuranceReplacementViewModel>>(this.context.InsuranceReplacement.AsEnumerable().Where(m => (m.ProposedId == newProduct.RecId)));

                            var result = newProduct;
                            return Ok(result);
                        }
                        else
                        {
                            ProposedInsuranceViewModel newProduct = new ProposedInsuranceViewModel();

                            existingProduct.ClientId = id;
                            existingProduct.Provider = _data.Provider;
                            existingProduct.Owner = _data.Owner;
                            existingProduct.ReplacementId = _data.ReplacementId;

                            this.context.ProposedInsuranceProducts.Update(existingProduct);
                            this.context.SaveChanges();

                            newProduct.Provider = existingProduct.Provider;
                            newProduct.RecId = existingProduct.RecId;
                            newProduct.Owner = existingProduct.Owner;
                            newProduct.ClientId = existingProduct.ClientId;
                            newProduct.ReplacementId = existingProduct.ReplacementId;

                            foreach (ProposedFeeDetailsViewModel feeDetail in _data.FeeDetails)
                            {
                                var existingFee = this.context.ProposedFeeDetails.Where(b => b.RecId == feeDetail.RecId).FirstOrDefault();
                                if (existingFee == null)
                                {
                                    feeDetail.HeaderId = existingProduct.RecId;
                                    this.context.ProposedFeeDetails.Add(this.mapper.Map<ProposedFeeDetails>(feeDetail));
                                }
                                else
                                {
                                    this.mapper.Map<ProposedFeeDetailsViewModel, ProposedFeeDetails>(feeDetail, existingFee);
                                    this.context.ProposedFeeDetails.Update(existingFee);
                                }
                            }

                            foreach (ProposedLifeCoverViewModel lifeCover in _data.LifeCover)
                            {
                                var existingLC = this.context.ProposedLifeCover.Where(b => b.RecId == lifeCover.RecId).FirstOrDefault();
                                if (existingLC == null)
                                {
                                    lifeCover.HeaderId = existingProduct.RecId;
                                    this.context.ProposedLifeCover.Add(this.mapper.Map<ProposedLifeCover>(lifeCover));
                                }
                                else
                                {
                                    this.mapper.Map<ProposedLifeCoverViewModel, ProposedLifeCover>(lifeCover, existingLC);
                                    this.context.ProposedLifeCover.Update(existingLC);
                                }
                            }

                            foreach (ProposedTpdCoverViewModel tpdCover in _data.TpdCover)
                            {
                                var existingTpd = this.context.ProposedTpdCover.Where(b => b.RecId == tpdCover.RecId).FirstOrDefault();
                                if (existingTpd == null)
                                {
                                    tpdCover.HeaderId = existingProduct.RecId;
                                    this.context.ProposedTpdCover.Add(this.mapper.Map<ProposedTpdCover>(tpdCover));
                                }
                                else
                                {
                                    this.mapper.Map<ProposedTpdCoverViewModel, ProposedTpdCover>(tpdCover, existingTpd);
                                    this.context.ProposedTpdCover.Update(existingTpd);
                                }
                            }

                            foreach (ProposedTraumaCoverViewModel traumaCover in _data.TraumaCover)
                            {
                                var existingTr = this.context.ProposedTraumaCover.Where(b => b.RecId == traumaCover.RecId).FirstOrDefault();
                                if (existingTr == null)
                                {
                                    traumaCover.HeaderId = existingProduct.RecId;
                                    this.context.ProposedTraumaCover.Add(this.mapper.Map<ProposedTraumaCover>(traumaCover));
                                }
                                else
                                {
                                    this.mapper.Map<ProposedTraumaCoverViewModel, ProposedTraumaCover>(traumaCover, existingTr);
                                    this.context.ProposedTraumaCover.Update(existingTr);
                                }

                            }

                            foreach (ProposedIncomeCoverViewModel incomeCover in _data.IncomeCover)
                            {
                                var existingIncome = this.context.ProposedIncomeCover.Where(b => b.RecId == incomeCover.RecId).FirstOrDefault();
                                if (existingIncome == null)
                                {
                                    incomeCover.HeaderId = existingProduct.RecId;
                                    this.context.ProposedIncomeCover.Add(this.mapper.Map<ProposedIncomeCover>(incomeCover));
                                }
                                else
                                {
                                    this.mapper.Map<ProposedIncomeCoverViewModel, ProposedIncomeCover>(incomeCover, existingIncome);
                                    this.context.ProposedIncomeCover.Update(existingIncome);
                                }
                            }


                                var existingImplementation = this.context.CostOfAdvice.Where(b => b.RecId == _data.Implementation.RecId).FirstOrDefault();
                                if (existingImplementation == null)
                                {
                                   _data.Implementation.HeaderId = existingProduct.RecId;
                                    this.context.CostOfAdvice.Add(this.mapper.Map<CostOfAdvice>(_data.Implementation));
                                }
                                else
                                {
                                    this.mapper.Map<CostOfAdviceViewModel, CostOfAdvice>(_data.Implementation, existingImplementation);
                                    this.context.CostOfAdvice.Update(existingImplementation);
                                }

                            var existingOngoing= this.context.CostOfAdvice.Where(b => b.RecId == _data.Ongoing.RecId).FirstOrDefault();
                            if (existingOngoing == null)
                            {
                                _data.Ongoing.HeaderId = existingProduct.RecId;
                                this.context.CostOfAdvice.Add(this.mapper.Map<CostOfAdvice>(_data.Ongoing));
                            }
                            else
                            {
                                this.mapper.Map<CostOfAdviceViewModel, CostOfAdvice>(_data.Ongoing, existingOngoing);
                                this.context.CostOfAdvice.Update(existingOngoing);
                            }


                            foreach (InsuranceReplacementViewModel rep in _data.Replacement)
                            {
                                var existingReplacement = this.context.InsuranceReplacement.Where(b => b.RecId == rep.RecId).FirstOrDefault();
                                if (existingReplacement == null)
                                {
                                    rep.ProposedId = existingProduct.RecId;
                                    this.context.InsuranceReplacement.Add(this.mapper.Map<InsuranceReplacement>(rep));
                                }
                                else
                                {
                                    this.mapper.Map<InsuranceReplacementViewModel, InsuranceReplacement>(rep, existingReplacement);
                                    this.context.InsuranceReplacement.Update(existingReplacement);
                                }
                            }


                            var fd = (from e in this.context.ProposedFeeDetails.Where(r => r.HeaderId == existingProduct.RecId)
                                      where !_data.FeeDetails.Any(f => f.RecId == e.RecId)
                                      select e).ToList();
                            this.context.ProposedFeeDetails.RemoveRange(fd);

                            var lc = (from e in this.context.ProposedLifeCover.Where(r => r.HeaderId == existingProduct.RecId)
                                      where !_data.LifeCover.Any(f => f.RecId == e.RecId)
                                      select e).ToList();
                            this.context.ProposedLifeCover.RemoveRange(lc);

                            var tc = (from e in this.context.ProposedTpdCover.Where(r => r.HeaderId == existingProduct.RecId)
                                      where !_data.TpdCover.Any(f => f.RecId == e.RecId)
                                      select e).ToList();
                            this.context.ProposedTpdCover.RemoveRange(tc);

                            var trc = (from e in this.context.ProposedTraumaCover.Where(r => r.HeaderId == existingProduct.RecId)
                                       where !_data.TraumaCover.Any(f => f.RecId == e.RecId)
                                       select e).ToList();
                            this.context.ProposedTraumaCover.RemoveRange(trc);

                            var ic = (from e in this.context.ProposedIncomeCover.Where(r => r.HeaderId == existingProduct.RecId)
                                      where !_data.IncomeCover.Any(f => f.RecId == e.RecId)
                                      select e).ToList();
                            this.context.ProposedIncomeCover.RemoveRange(ic);

                            var rp = (from e in this.context.InsuranceReplacement.Where(r => r.ProposedId == existingProduct.RecId)
                                      where !_data.Replacement.Any(f => f.RecId == e.RecId)
                                      select e).ToList();
                            this.context.InsuranceReplacement.RemoveRange(rp);

                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            newProduct.FeeDetails = this.mapper.Map<List<ProposedFeeDetails>, List<ProposedFeeDetailsViewModel>>(this.context.ProposedFeeDetails.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.LifeCover = this.mapper.Map<List<ProposedLifeCover>, List<ProposedLifeCoverViewModel>>(this.context.ProposedLifeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TpdCover = this.mapper.Map<List<ProposedTpdCover>, List<ProposedTpdCoverViewModel>>(this.context.ProposedTpdCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.TraumaCover = this.mapper.Map<List<ProposedTraumaCover>, List<ProposedTraumaCoverViewModel>>(this.context.ProposedTraumaCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.IncomeCover = this.mapper.Map<List<ProposedIncomeCover>, List<ProposedIncomeCoverViewModel>>(this.context.ProposedIncomeCover.Where(r => r.HeaderId == newProduct.RecId).ToList());
                            newProduct.Implementation = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(r => r.HeaderId == newProduct.RecId && r.CoaType == "implementation").FirstOrDefault());
                            newProduct.Ongoing = this.mapper.Map<CostOfAdviceViewModel>(this.context.CostOfAdvice.Where(r => r.HeaderId == newProduct.RecId && r.CoaType == "ongoing").FirstOrDefault());
                            newProduct.Replacement = this.mapper.Map<List<InsuranceReplacementViewModel>>(this.context.InsuranceReplacement.AsEnumerable().Where(m => (m.ProposedId == newProduct.RecId)));


                            var result = newProduct;
                            return Ok(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ModelState);
                        //Log, handle or absorbe I don't care ^_^
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCurrent(int id)
        {
            var existingProduct = this.context.CurrentInsuranceProducts.Where(b => b.RecId == id).FirstOrDefault();
            if (existingProduct == null)
            {
                return NotFound();
            }
            else
            {
                this.context.CurrentInsuranceProducts.Remove(existingProduct);
                this.context.CurrentFeeDetails.RemoveRange(this.context.CurrentFeeDetails.Where(r => r.HeaderId == id));
                this.context.CurrentLifeCover.RemoveRange(this.context.CurrentLifeCover.Where(r => r.HeaderId == id));
                this.context.CurrentTpdCover.RemoveRange(this.context.CurrentTpdCover.Where(r => r.HeaderId == id));
                this.context.CurrentTraumaCover.RemoveRange(this.context.CurrentTraumaCover.Where(r => r.HeaderId == id));
                this.context.CurrentIncomeCover.RemoveRange(this.context.CurrentIncomeCover.Where(r => r.HeaderId == id));
                this.context.SaveChanges();
                return Ok(id);
            }

        }

        [HttpDelete("{id}/{isProposed}")]
        public IActionResult DeleteProposed(int id)
        {
            var existingProduct = this.context.ProposedInsuranceProducts.Where(b => b.RecId == id).FirstOrDefault();
            if (existingProduct == null)
            {
                return NotFound();
            }
            else
            {
                this.context.ProposedInsuranceProducts.Remove(existingProduct);
                this.context.ProposedFeeDetails.RemoveRange(this.context.ProposedFeeDetails.Where(r => r.HeaderId == id));
                this.context.ProposedLifeCover.RemoveRange(this.context.ProposedLifeCover.Where(r => r.HeaderId == id));
                this.context.ProposedTpdCover.RemoveRange(this.context.ProposedTpdCover.Where(r => r.HeaderId == id));
                this.context.ProposedTraumaCover.RemoveRange(this.context.ProposedTraumaCover.Where(r => r.HeaderId == id));
                this.context.ProposedIncomeCover.RemoveRange(this.context.ProposedIncomeCover.Where(r => r.HeaderId == id));
                this.context.CostOfAdvice.RemoveRange(this.context.CostOfAdvice.Where(r => r.HeaderId == id));
                this.context.InsuranceReplacement.RemoveRange(this.context.InsuranceReplacement.Where(r => r.ProposedId == id));
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }



}

