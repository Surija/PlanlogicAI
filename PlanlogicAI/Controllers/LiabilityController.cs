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
    [Route("/api/liability")]
    public class LiabilityController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public LiabilityController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       // GET api/values
       [HttpGet("{id}")]
        public IEnumerable<LiabilityViewModel> GetLiabilities(int id)
        {
            IEnumerable<LiabilityViewModel> list = this.mapper.Map<IEnumerable<LiabilityViewModel>>(this.context.Liability.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        [HttpGet("{id}/{liabilityID}")]
        public IEnumerable<LiabilityDetailsViewModel> GetLiabilityDetails(int id,int liabilityID)
        {
            IEnumerable<LiabilityDetailsViewModel> list = this.mapper.Map<IEnumerable<LiabilityDetailsViewModel>>(this.context.LiabilityDetails.AsEnumerable().Where(m => (m.ClientId == id) && (m.LiabilityId == liabilityID)));
            return list;
        }

        [HttpGet("{id}/{type}/{val}")]
        public IEnumerable<LiabilityDetailsViewModel> GetAllLiabilityDetails(int id, int type, int val)
        {
            IEnumerable<LiabilityDetailsViewModel> list = this.mapper.Map<IEnumerable<LiabilityDetailsViewModel>>(this.context.LiabilityDetails.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        // POST api/values
        [HttpPut("{id}")]
        public IActionResult CreateLiability(int id, [FromBody] LiabilityDrawdowns _liability)
        {
            if (ModelState.IsValid)
            {

                var existingLiability= this.context.Liability.Where(b => b.LiabilityId == _liability.liability.LiabilityId).FirstOrDefault();
                if (existingLiability == null)
                {
                    var liability = this.mapper.Map<Liability>(_liability.liability);

                    liability.ClientId = id;
                    this.context.Liability.Add(liability);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<LiabilityViewModel>(liability);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<LiabilityViewModel, Liability>(_liability.liability, existingLiability);

                    this.context.Liability.Update(existingLiability);
                    this.context.SaveChanges();

                    this.context.RemoveRange(this.context.LiabilityDetails.Where(m => (m.ClientId == id) && (m.LiabilityId == _liability.liability.LiabilityId)));
                    foreach (LiabilityDetailsViewModel ldrawdown in _liability.liabilitydd)
                    {
                        var ldd = this.mapper.Map<LiabilityDetails>(ldrawdown);
                        ldd.ClientId = id;
                        ldd.LiabilityId = _liability.liability.LiabilityId;
                        this.context.LiabilityDetails.Add(ldd);
                    }
                    this.context.SaveChanges();

                    var result = this.mapper.Map<LiabilityViewModel>(_liability.liability);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLiability(int id)
        {
            var existingLiability = this.context.Liability.Where(b => b.LiabilityId == id).FirstOrDefault();
            if (existingLiability == null)
            {
                return NotFound();
            }
            else
            {
                this.context.Liability.Remove(existingLiability);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
