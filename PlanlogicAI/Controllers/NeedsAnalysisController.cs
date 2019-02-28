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
    [Route("/api/needsAnalysis")]
    public class NeedsAnalysisController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public NeedsAnalysisController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id}/{isSelective}")]
        public IEnumerable<NeedsAnalysisViewModel> GetNeedsAnalysis(int id,int isSelective)
        {
            IEnumerable<NeedsAnalysisViewModel> list ;
            if (isSelective == 0)
            {
                 list = this.mapper.Map<IEnumerable<NeedsAnalysisViewModel>>(this.context.NeedsAnalysis.Where(b => b.ClientId == id && b.Owner == "Client"));
            }
            else
            {
                list = this.mapper.Map<IEnumerable<NeedsAnalysisViewModel>>(this.context.NeedsAnalysis.Where(b => b.ClientId == id && b.Owner == "Partner"));
            }
            return list;
           
        }
        [HttpPut("{id}")]
        public IActionResult CreateNeedsAnalysis(int id, [FromBody] NeedsAnalysisData _needsAnalysis)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {

                    foreach (NeedsAnalysisViewModel needsAnalysisView in _needsAnalysis.clientNeedsAnalysis)
                    {

                        var existingNA = this.context.NeedsAnalysis.Where(b => (b.RecId == needsAnalysisView.RecId) && (b.RecId != 0)).FirstOrDefault();
                        if (existingNA == null)
                        {
                            var needsAnalysis = this.mapper.Map<NeedsAnalysis>(needsAnalysisView);
                            needsAnalysis.ClientId = id;
                            this.context.NeedsAnalysis.Add(needsAnalysis);
                        }
                        else
                        {
                            this.mapper.Map<NeedsAnalysisViewModel, NeedsAnalysis>(needsAnalysisView, existingNA);
                            this.context.NeedsAnalysis.Update(existingNA);   
                        }

                    }

                    this.context.SaveChanges();

                    if (_needsAnalysis.isMarried == 1)
                    {
                        foreach (NeedsAnalysisViewModel needsAnalysisView in _needsAnalysis.partnerNeedsAnalysis)
                        {

                            var existingNA = this.context.NeedsAnalysis.Where(b => (b.RecId == needsAnalysisView.RecId) && (b.RecId != 0)).FirstOrDefault();
                            if (existingNA == null)
                            {
                                var needsAnalysis = this.mapper.Map<NeedsAnalysis>(needsAnalysisView);
                                needsAnalysis.ClientId = id;
                                this.context.NeedsAnalysis.Add(needsAnalysis);
                            }
                            else
                            {
                                this.mapper.Map<NeedsAnalysisViewModel, NeedsAnalysis>(needsAnalysisView, existingNA);
                                this.context.NeedsAnalysis.Update(existingNA);
                            }

                        }
                    }

                    this.context.SaveChanges();

                    dbContextTransaction.Commit();

                   var res = this.mapper.Map<List<NeedsAnalysis>, List<NeedsAnalysisViewModel>>(this.context.NeedsAnalysis.Where(r => r.ClientId == id).ToList());
                    return Ok(res);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNeedsAnalysis(int id)
        {
            var existingNA = this.context.NeedsAnalysis.Where(b => b.RecId == id).FirstOrDefault();
            if (existingNA == null)
            {
                return NotFound();
            }
            else
            {
                this.context.NeedsAnalysis.Remove(existingNA);
                this.context.SaveChanges();
                return Ok(id);
            }

        }
    }
}
