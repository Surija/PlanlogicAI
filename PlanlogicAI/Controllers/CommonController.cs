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
    [Route("/api/common")]
    public class CommonController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public CommonController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<GeneralViewModel> GetGeneralAssumption()
        {
            IEnumerable<GeneralViewModel> list = this.mapper.Map<IEnumerable<GeneralViewModel>>(this.context.General.AsEnumerable());
            return list;
        }

        [HttpGet("{id}")]
        public IEnumerable<MarginalTaxRatesViewModel> GetMarginalTaxRates(int id)
        {
            IEnumerable<MarginalTaxRatesViewModel> list = this.mapper.Map<IEnumerable<MarginalTaxRatesViewModel>>(this.context.MarginalTaxRates.AsEnumerable());
            return list;
        }

        [HttpGet("{id}/{type}")]
        public IEnumerable<AssetTypesViewModel> GetAssetTypesAssumption(int id,int type)
        {
            IEnumerable<AssetTypesViewModel> list = this.mapper.Map<IEnumerable<AssetTypesViewModel>>(this.context.AssetTypes.AsEnumerable());
            return list;
        }

        [HttpGet("{id}/{type}/{type1}")]
        public IEnumerable<PensionDrawdownViewModel> GetPensionDrawdownAssumption(int id, int type , int type1)
        {
            IEnumerable<PensionDrawdownViewModel> list = this.mapper.Map<IEnumerable<PensionDrawdownViewModel>>(this.context.PensionDrawdown.AsEnumerable());
            return list;
        }

    }
}
