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

namespace PlanlogicAI.Controllers
{
    [Route("/api/fee")]
    public class FeeController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public FeeController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<InvestmentFundViewModel> GetIvestments()
        {
            IEnumerable<InvestmentFundViewModel> list = this.mapper.Map<IEnumerable<InvestmentFundViewModel>>(this.context.InvestmentFund.AsEnumerable());
            return list;
        }

        [HttpGet("{id}/{type}")]
        public IEnumerable<ProductFeesViewModel> GetFees(int id,string type)
        {
            IEnumerable<ProductFeesViewModel> list = this.mapper.Map<IEnumerable<ProductFeesViewModel>>(this.context.ProductFees.AsEnumerable().Where(m => (m.HeaderId == id && m.HeaderType == type)));
            return list;
        }

        [HttpGet("{clientId}")]
        public IEnumerable<PlatformDetailsViewModel> GetPlatformDetails(int clientId)
        {
            IEnumerable<PlatformDetailsViewModel> list = this.mapper.Map<IEnumerable<PlatformDetailsViewModel>>(from s in context.Product.AsEnumerable()
                                                                                                                            join sa in context.Platform.AsEnumerable() on s.PlatformId equals sa.PlatformId
                                                                                                                            select new PlatformDetailsViewModel
                                                                                                                            { 
                                                                                                                                ProductId = s.ProductId,
                                                                                                                                PlatformId = s.PlatformId,
                                                                                                                                PlatformName = sa.PlatformName,
                                                                                                                                ProductType = s.ProductType,
                                                                                                                                PlatformType = sa.PlatformType,
                                                                                                                                SubType = sa.SubType,

                                                                                                                            });
            return list;
        }

        //[HttpPut("{id}/{type}")]
        //public IActionResult CreateProductFund(int id, int type, [FromBody] InvestmentFundViewModel[] _pf)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var existingPF = this.context.ProductFund.Where(b => b.ProductId == id).FirstOrDefault();
        //        if (existingPF == null)
        //        {
        //            foreach (InvestmentFundViewModel investment in _pf)
        //            {
        //                if (investment != null)
        //                {
        //                    ProductFundViewModel pf = new ProductFundViewModel();
        //                    pf.ProductId = id;
        //                    pf.Apircode = investment.Apircode;
        //                    var pfDetails = this.mapper.Map<ProductFund>(pf);
        //                    this.context.ProductFund.Add(pfDetails);
        //                }
        //            }
        //            this.context.SaveChanges();

        //            var result = this.mapper.Map<InvestmentFundViewModel[]>(_pf);
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            if (type == 0)
        //            {
        //                this.context.RemoveRange(this.context.ProductFund.Where(m => (m.ProductId == id)));
        //            }

        //            foreach (InvestmentFundViewModel investment in _pf)
        //            {
        //                if (investment != null)
        //                {
        //                    ProductFundViewModel pf = new ProductFundViewModel();
        //                    pf.ProductId = id;
        //                    pf.Apircode = investment.Apircode;
        //                    var pfDetails = this.mapper.Map<ProductFund>(pf);
        //                    this.context.ProductFund.Add(pfDetails);
        //                }
        //            }
        //            this.context.SaveChanges();

        //            var result = this.mapper.Map<InvestmentFundViewModel[]>(_pf);
        //            return Ok(result);
        //        }


        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}


        [HttpPut("{id}/{type}")]
        public IActionResult CreateFee(int id, string type, [FromBody] ProductFeesViewModel[] _fees)
        {
            if (ModelState.IsValid)
            {
                var existingFee = this.context.ProductFees.Where(b => b.HeaderId == id && b.HeaderType == type).FirstOrDefault();
                if (existingFee == null)
                {
                    foreach (ProductFeesViewModel _fee in _fees)
                    {
                        if ((_fee != null && _fee.Amount != 0) || _fee.CostType == "transactional" )
                        {
                            var fee = this.mapper.Map<ProductFees>(_fee);
                            this.context.ProductFees.Add(fee);
                        }
                    }
                    this.context.SaveChanges();

                    var result = this.mapper.Map<ProductFeesViewModel[]>(_fees);
                    return Ok(result);
                }
                else
                {
                    this.context.RemoveRange(this.context.ProductFees.Where(m => (m.HeaderType == type && m.HeaderId == id)));

                    foreach (ProductFeesViewModel _fee in _fees)
                    {
                        if ((_fee != null && _fee.Amount != 0) || _fee.CostType == "transactional")
                        {
                            var fee = this.mapper.Map<ProductFees>(_fee);
                            this.context.ProductFees.Add(fee);
                        }
                    }

                    this.context.SaveChanges();

                    var result = this.mapper.Map<ProductFeesViewModel[]>(_fees);
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}/{productID}")]
        public IActionResult DeleteFee(int id,int productID)
        {
            var existingFee = this.context.ProductFees.Where(b => b.FeeId == id && b.ProductId == productID).FirstOrDefault();
            if (existingFee == null)
            {
                return NotFound();
            }
            else
            {
                this.context.ProductFees.Remove(existingFee);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }  
}
