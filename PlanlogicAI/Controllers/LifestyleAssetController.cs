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
    [Route("/api/lifestyleAsset")]
    public class LifestyleAssetController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public LifestyleAssetController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       // GET api/values
       [HttpGet("{id}")]
        public IEnumerable<LifestyleAssetViewModel> GetLifestyleAssets(int id)
        {
            IEnumerable<LifestyleAssetViewModel> list = this.mapper.Map<IEnumerable<LifestyleAssetViewModel>>(this.context.LifestyleAsset.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        // POST api/values
        [HttpPut("{id}")]
        public IActionResult CreateLifestyleAsset(int id, [FromBody] LifestyleAssetViewModel _lAsset)
        {
            if (ModelState.IsValid)
            {

                var existingLA = this.context.LifestyleAsset.Where(b => b.LassetId == _lAsset.LassetId).FirstOrDefault();
                if (existingLA == null)
                {
                    var lasset = this.mapper.Map<LifestyleAsset>(_lAsset);

                    lasset.ClientId = id;
                    this.context.LifestyleAsset.Add(lasset);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<LifestyleAssetViewModel>(lasset);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<LifestyleAssetViewModel, LifestyleAsset>(_lAsset, existingLA);

                    this.context.LifestyleAsset.Update(existingLA);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<LifestyleAssetViewModel>(_lAsset);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLifestyleAsset(int id)
        {
            var existingLA = this.context.LifestyleAsset.Where(b => b.LassetId == id).FirstOrDefault();
            if (existingLA == null)
            {
                return NotFound();
            }
            else
            {
                this.context.LifestyleAsset.Remove(existingLA);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
