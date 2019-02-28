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
    [Route("/api/pensionAssumptions")]
    public class PensionAssumptionsController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public PensionAssumptionsController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<MinimumPensionDrawdownViewModel> GetMinimumPensionDrawdown()
        {
            IEnumerable<MinimumPensionDrawdownViewModel> list = this.mapper.Map<IEnumerable<MinimumPensionDrawdownViewModel>>(this.context.MinimumPensionDrawdown.AsEnumerable());
            return list;
        }

        [HttpGet("{id}")]
        public IEnumerable<PreservationAgeViewModel> GetPreservationAge(int id)
        {
            IEnumerable<PreservationAgeViewModel> list = this.mapper.Map<IEnumerable<PreservationAgeViewModel>>(this.context.PreservationAge.AsEnumerable());
            return list;
        }

    }
}
