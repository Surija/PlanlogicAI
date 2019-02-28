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
    [Route("/api/financialAsset")]
    public class FinancialAssetController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public FinancialAssetController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //GET api/values

        //TODO: map to view model
       //[HttpGet("{id}")]
       // public IEnumerable<FinancialAsset> GetFinancialAssets(int id)
       // {
       //     IEnumerable<FinancialAsset> list = this.context.FinancialAsset.AsEnumerable().Where(m => (m.ClientId == id));
       //     return list;
       // }

        //// POST api/values
        //[HttpPut("{id}/{type}")]
        //public IActionResult CreateCashFlow(int id, string type,[FromBody] List<CashFlowViewModel> _cashflows)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        this.context.RemoveRange(this.context.CashFlow.Where(m => (m.ClientId == id) && (m.Cftype == type)));
        //        foreach (CashFlowViewModel cf in _cashflows)
        //        {
        //            var cashFlow = this.mapper.Map<CashFlow>(cf);

        //            cashFlow.ClientId = id;
        //            this.context.CashFlow.Add(cashFlow);
        //        }
        //        this.context.SaveChanges();

        //       //var result = this.mapper.Map<CashFlowViewModel>(_cashflows);
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}



    }
}
