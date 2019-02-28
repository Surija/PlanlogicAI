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
    [Route("/api/superAssumptions")]
    public class SuperAssumptionsController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public SuperAssumptionsController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<SuperAssumptionsViewModel> GetSuperAssumption()
        {
            IEnumerable<SuperAssumptionsViewModel> list = this.mapper.Map<IEnumerable<SuperAssumptionsViewModel>>(this.context.SuperAssumptions.AsEnumerable());
            return list;
        }

        [HttpGet("{id}")]
        public IEnumerable<SgcrateViewModel> GetSgcrates(int id)
        {
            IEnumerable<SgcrateViewModel> list = this.mapper.Map<IEnumerable<SgcrateViewModel>>(this.context.Sgcrate.AsEnumerable());
            return list;
        }

    }
}
