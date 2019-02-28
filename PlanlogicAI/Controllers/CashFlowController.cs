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
    [Route("/api/cashFlow")]
    public class CashFlowController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public CashFlowController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/values
        [HttpGet("{id}/{type}")]
        public IEnumerable<CashFlowViewModel> GetCashFlows(int id,string type)
        {
            IEnumerable<CashFlowViewModel> list = this.mapper.Map<IEnumerable<CashFlowViewModel>>(this.context.CashFlow.AsEnumerable().Where(m => (m.ClientId == id ) && (m.Cftype == type )));
            return list;
        }

        // POST api/values
        [HttpPut("{id}/{type}")]
        public IActionResult CreateCashFlow(int id, string type,[FromBody] CashFlowViewModel _cashflows)
        {
            if (ModelState.IsValid)
            {

                var existingCF = this.context.CashFlow.Where(b => b.CflowId == _cashflows.CflowId).FirstOrDefault();
                if (existingCF == null)
                {
                    var cashFlow = this.mapper.Map<CashFlow>(_cashflows);

                    cashFlow.ClientId = id;
                    this.context.CashFlow.Add(cashFlow);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<CashFlowViewModel>(cashFlow);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<CashFlowViewModel, CashFlow>(_cashflows, existingCF);

                    this.context.CashFlow.Update(existingCF);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<CashFlowViewModel>(_cashflows);
                    return Ok(result);
                }

               
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCashFlow(int id)
        {
            var existingCF = this.context.CashFlow.Where(b => b.CflowId == id).FirstOrDefault();
            if (existingCF == null)
            {
                return NotFound();
            }
            else
            {
                this.context.CashFlow.Remove(existingCF);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
