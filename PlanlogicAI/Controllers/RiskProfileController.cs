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
    [Route("/api/riskProfile")]
    public class RiskProfileController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public RiskProfileController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<RiskProfileViewModel> GetRiskProfile()
        {
            IEnumerable<RiskProfileViewModel> list = this.mapper.Map<IEnumerable<RiskProfileViewModel>>(this.context.RiskProfile.AsEnumerable());
             return list;
        }

    }  
}
