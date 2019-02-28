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
    [Route("/api/currentPortfolioFund")]
    public class CurrentPortfolioFundController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public CurrentPortfolioFundController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPut("{id}/{percentage}")]
        public IActionResult CreateFund(int id,decimal percentage, [FromBody] CurrentClientFundsViewModel _fund)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = this.context.CurrentClientProducts.Where(a => a.RecId == id).FirstOrDefault();

                var existingFund = this.context.CurrentClientFunds.Where(b => b.RecId == _fund.RecId).FirstOrDefault();
                if (existingFund == null)
                {
                    var fund = this.mapper.Map<CurrentClientFunds>(_fund);

                    fund.HeaderId = id;
                    this.context.CurrentClientFunds.Add(fund);
                    existingProduct.Percentage = percentage;
                    this.context.CurrentClientProducts.Update(existingProduct);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<CurrentClientFundsViewModel>(fund);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<CurrentClientFundsViewModel, CurrentClientFunds>(_fund, existingFund);

                    this.context.CurrentClientFunds.Update(existingFund);
                    existingProduct.Percentage = percentage;
                    this.context.CurrentClientProducts.Update(existingProduct);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<CurrentClientFundsViewModel>(_fund);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFund(int id)
        {
            var existingFund = this.context.CurrentClientFunds.Where(b => b.RecId == id).FirstOrDefault();
            if (existingFund == null)
            {
                return NotFound();
            }
            else
            {
                this.context.CurrentClientFunds.Remove(existingFund);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

        [HttpGet("{id}")]
        public IEnumerable<ProductReplacementViewModel> GetProductReplacement(int id)
        {


            IEnumerable<ProductReplacementViewModel> list = this.mapper.Map<IEnumerable<ProductReplacementViewModel>>(this.context.ProductReplacement.AsEnumerable().Where(m => (m.ProposedId == id)));
            return list;
        }
        [HttpGet]
        public IEnumerable<ProductReplacementViewModel> GetAllProductReplacement(int id)
        {


            IEnumerable<ProductReplacementViewModel> list = this.mapper.Map<IEnumerable<ProductReplacementViewModel>>(this.context.ProductReplacement.AsEnumerable());
            return list;
        }


    }  
}
