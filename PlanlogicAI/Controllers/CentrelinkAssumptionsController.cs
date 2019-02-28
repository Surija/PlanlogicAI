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
    [Route("/api/centrelinkAssumptions")]
    public class CentrelinkAssumptionsController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public CentrelinkAssumptionsController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //[HttpGet]
        //public IEnumerable<MinimumPensionDrawdownViewModel> GetMinimumPensionDrawdown()
        //{
        //    IEnumerable<MinimumPensionDrawdownViewModel> list = this.mapper.Map<IEnumerable<MinimumPensionDrawdownViewModel>>(this.context.MinimumPensionDrawdown.AsEnumerable());
        //    return list;
        //}

        [HttpGet("{id}")]
        public IEnumerable<QualifyingAgeViewModel> GetQualifyingAge(int id)
        {
            IEnumerable<QualifyingAgeViewModel> list = this.mapper.Map<IEnumerable<QualifyingAgeViewModel>>(this.context.QualifyingAge .AsEnumerable());
            return list;
        }

    }
}
