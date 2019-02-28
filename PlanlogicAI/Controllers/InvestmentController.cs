using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanlogicAI.Data;
using PlanlogicAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanlogicAI.Controllers
{
    [Route("/api/investment")]
    public class InvestmentController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public InvestmentController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       // GET api/values
       [HttpGet("{id}")]
        public IEnumerable<InvestmentViewModel> GetInvestments(int id)
        {
            IEnumerable<InvestmentViewModel> list = this.mapper.Map<IEnumerable<InvestmentViewModel>>(this.context.Investment.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        [HttpGet("{id}/{investmentID}")]
        public IEnumerable<InvestmentDetailsViewModel> GetInvestmentDetails(int id,int investmentID)
        {
            IEnumerable<InvestmentDetailsViewModel> list = this.mapper.Map<IEnumerable<InvestmentDetailsViewModel>>(this.context.InvestmentDetails.AsEnumerable().Where(m => (m.ClientId == id) && (m.InvestmentId == investmentID)));
            return list;
        }

        [HttpGet("{id}/{type}/{val}")]
        public IEnumerable<InvestmentDetailsViewModel> GetAllInvestmentDetails(int id, int type, int val)
        {
            IEnumerable<InvestmentDetailsViewModel> list = this.mapper.Map<IEnumerable<InvestmentDetailsViewModel>>(this.context.InvestmentDetails.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        // POST api/values
        [HttpPut("{id}")]
        public IActionResult CreateInvestment(int id, [FromBody] InvestmentCW _investment)
        {
            if (ModelState.IsValid)
            {

                var existingInvestment= this.context.Investment.Where(b => b.InvestmentId == _investment.investmentDetails.InvestmentId).FirstOrDefault();
                if (existingInvestment == null)
                {
                    var investment = this.mapper.Map<Investment>(_investment.investmentDetails);

                    investment.ClientId = id;
                    this.context.Investment.Add(investment);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<InvestmentViewModel>(investment);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<InvestmentViewModel, Investment>(_investment.investmentDetails, existingInvestment);

                    this.context.Investment.Update(existingInvestment);
                    this.context.SaveChanges();

                    this.context.RemoveRange(this.context.InvestmentDetails.Where(m => (m.ClientId == id) && (m.InvestmentId == _investment.investmentDetails.InvestmentId)));
                    foreach (InvestmentDetailsViewModel cf in _investment.cw)
                    {
                        var cw = this.mapper.Map<InvestmentDetails>(cf);
                        cw.ClientId = id;
                        cw.InvestmentId = _investment.investmentDetails.InvestmentId;
                        this.context.InvestmentDetails.Add(cw);
                    }
                    this.context.SaveChanges();

                    var result = this.mapper.Map<InvestmentViewModel>(_investment.investmentDetails);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteInvestment(int id)
        {
            var existingInvestment = this.context.Investment.Where(b => b.InvestmentId == id).FirstOrDefault();
            if (existingInvestment == null)
            {
                return NotFound();
            }
            else
            {
                this.context.Investment.Remove(existingInvestment);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
