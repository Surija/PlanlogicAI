using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanlogicAI.Data;
using PlanlogicAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanlogicAI.Controllers
{
    [Route("/api/fund")]
    public class InvestmentFundController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public InvestmentFundController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public IEnumerable<InvestmentFundViewModel> GetFund(int id)
        {
            IEnumerable<InvestmentFundViewModel> list = this.mapper.Map<IEnumerable<InvestmentFundViewModel>>(from s in this.context.InvestmentFund.AsEnumerable() join sa in this.context.ProductFund.AsEnumerable() on s.Apircode equals sa.Apircode where sa.ProductId == id &&  !(s.DefensiveAlternatives == 0 && s.DomesticCash == 0 && s.DomesticEquity == 0 && s.DomesticFixedInterest == 0 && s.DomesticProperty == 0 && s.GrowthAlternatives == 0 && s.InternationalCash == 0 && s.InternationalEquity == 0 && s.InternationalFixedInterest == 0 && s.InternationalProperty == 0 && s.OtherGrowth == 0 && s.Apircode != "Cash") select s).ToList();
                //this.mapper.Map<IEnumerable<InvestmentFundViewModel>>(this.context.InvestmentFund.AsEnumerable().Where(m => (m == id)));
            return list;
        }

        [HttpGet("{id}/{temp}")]
        public IEnumerable<InvestmentFundViewModel> GetAllFund(int id,int temp )
        { 
            IEnumerable<InvestmentFundViewModel> list2 = this.mapper.Map<IEnumerable<InvestmentFundViewModel>>(from s in this.context.InvestmentFund.AsEnumerable() where !(s.DefensiveAlternatives == 0 && s.DomesticCash == 0 && s.DomesticEquity == 0 && s.DomesticFixedInterest == 0 && s.DomesticProperty == 0 && s.GrowthAlternatives == 0 && s.InternationalCash == 0 && s.InternationalEquity == 0 && s.InternationalFixedInterest == 0 && s.InternationalProperty == 0 && s.OtherGrowth == 0 && s.Apircode != "Cash") select s).ToList();
            return list2;           
        }


        [HttpPost]
        public  IActionResult CreateFund([FromBody] InvestmentFundViewModel _fund)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _fund.IsActive = _fund.IsActive.Trim();
                    _fund.IsSingle = _fund.IsSingle.Trim();
                    var existingFund = this.context.InvestmentFund.Where(b => b.Apircode == _fund.Apircode).FirstOrDefault();
                    if (existingFund == null)
                    {
                        var fund = this.mapper.Map<InvestmentFund>(_fund);
                        this.context.InvestmentFund.Add(fund);
                        this.context.SaveChanges();
                        var result = this.mapper.Map<InvestmentFundViewModel>(fund);
                        return Ok(result);
                    }
                    else
                    {
                        this.mapper.Map<InvestmentFundViewModel, InvestmentFund>(_fund, existingFund);

                        this.context.InvestmentFund.Update(existingFund);
                        this.context.SaveChanges();

                        var result = this.mapper.Map<InvestmentFundViewModel>(_fund);
                        return Ok(result);
                    }


                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public IActionResult CreateFundDetails(int id, [FromBody] ProductInvestmentFundViewModel _fund)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        _fund.fund.UpdatedOn = DateTime.Now;
                        _fund.fund.IsActive = _fund.fund.IsActive.Trim();
                        _fund.fund.IsSingle = _fund.fund.IsSingle.Trim();
                        var existingFund = this.context.InvestmentFund.Where(b => b.Apircode == _fund.fund.Apircode).FirstOrDefault();
                        if (existingFund == null)
                        {
                            var fund = this.mapper.Map<InvestmentFund>(_fund.fund);
                            this.context.InvestmentFund.Add(fund);
                            this.context.SaveChanges();

                            var pfDetails = this.mapper.Map<ProductFund>(_fund.productFund);
                            this.context.ProductFund.Add(pfDetails);
                            this.context.SaveChanges();

                            dbContextTransaction.Commit();
                            var result = this.mapper.Map<InvestmentFundViewModel>(fund);
                            return Ok(result);
                        }
                        else
                        {
                            this.mapper.Map<InvestmentFundViewModel, InvestmentFund>(_fund.fund, existingFund);
                            this.context.InvestmentFund.Update(existingFund);
                            this.context.SaveChanges();

                            var existingProductFund = this.context.ProductFund.Where(b => b.Apircode == _fund.fund.Apircode && b.ProductId == _fund.productFund.ProductId).FirstOrDefault();
                            if (existingProductFund == null)
                            {
                                var pfDetails = this.mapper.Map<ProductFund>(_fund.productFund);
                                this.context.ProductFund.Add(pfDetails);
                              
                            }
                            else
                            {
                                this.mapper.Map<ProductFundViewModel, ProductFund>(_fund.productFund, existingProductFund);
                                this.context.ProductFund.Update(existingProductFund);
                               
                            }
                            this.context.SaveChanges();


                            var result = this.mapper.Map<InvestmentFundViewModel>(_fund.fund);
                            return Ok(result);
                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
        }

        [HttpPut("{id}/{type}")]
        public IActionResult CreateProductFund(int id,int type, [FromBody] InvestmentFundViewModel[] _pf)
        {
            if (ModelState.IsValid)
            {

                var existingPF = this.context.ProductFund.Where(b => b.ProductId == id).FirstOrDefault();
                if (existingPF == null)
                {
                    foreach (InvestmentFundViewModel investment in _pf)
                    {
                        if (investment != null)
                        {
                            ProductFundViewModel pf = new ProductFundViewModel();
                            pf.ProductId = id;
                            pf.Apircode = investment.Apircode;
                            var pfDetails = this.mapper.Map<ProductFund>(pf);
                            this.context.ProductFund.Add(pfDetails);
                        }
                    }
                    this.context.SaveChanges();

                    var result = this.mapper.Map<InvestmentFundViewModel[]>(_pf);
                    return Ok(result);
                }
                else
                {
                    if (type == 0)
                    {
                        this.context.RemoveRange(this.context.ProductFund.Where(m => (m.ProductId == id)));
                    }

                    foreach (InvestmentFundViewModel investment in _pf)
                    {
                        if (investment != null)
                        {
                            ProductFundViewModel pf = new ProductFundViewModel();
                            pf.ProductId = id;
                            pf.Apircode = investment.Apircode;
                            var pfDetails = this.mapper.Map<ProductFund>(pf);
                            this.context.ProductFund.Add(pfDetails);
                        }
                    }
                    this.context.SaveChanges();

                    var result = this.mapper.Map<InvestmentFundViewModel[]>(_pf);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }  
}
