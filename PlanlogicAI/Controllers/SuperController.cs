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
    [Route("/api/super")]
    public class SuperController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public SuperController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       // GET api/values
       [HttpGet("{id}")]
        public IEnumerable<SuperViewModel> GetSupers(int id)
        {
            IEnumerable<SuperViewModel> list = this.mapper.Map<IEnumerable<SuperViewModel>>(this.context.Super.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        [HttpGet("{id}/{superID}")]
        public IEnumerable<SuperDetailsViewModel> GetSuperDetails(int id, int superID)
        {
            IEnumerable<SuperDetailsViewModel> list = this.mapper.Map<IEnumerable<SuperDetailsViewModel>>(this.context.SuperDetails.AsEnumerable().Where(m => (m.ClientId == id) && (m.SuperId == superID)));
            return list;
        }

        [HttpGet("{id}/{type}/{val}")]
        public IEnumerable<SuperDetailsViewModel> GetAllSuperDetails(int id, int type, int val)
        {
            IEnumerable<SuperDetailsViewModel> list = this.mapper.Map<IEnumerable<SuperDetailsViewModel>>(this.context.SuperDetails.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }


        // POST api/values
        [HttpPut("{id}")]
        public IActionResult CreateSuper(int id, [FromBody] SuperAsset _super)
        {
            if (ModelState.IsValid)
            {

                var existingSuper= this.context.Super.Where(b => b.SuperId == _super.super.SuperId).FirstOrDefault();
                if (existingSuper == null)
                {
                    var super = this.mapper.Map<Super>(_super.super);

                    super.ClientId = id;
                    this.context.Super.Add(super);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<SuperViewModel>(super);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<SuperViewModel, Super>(_super.super, existingSuper);

                    this.context.Super.Update(existingSuper);
                    this.context.SaveChanges();

                    this.context.RemoveRange(this.context.SuperDetails.Where(m => (m.ClientId == id) && (m.SuperId == _super.super.SuperId)));
                    foreach (SuperDetailsViewModel sp in _super.superDetails)
                    {
                        var sup = this.mapper.Map<SuperDetails>(sp);
                        sup.ClientId = id;
                        sup.SuperId = _super.super.SuperId;
                        this.context.SuperDetails.Add(sup);
                    }
                    this.context.SaveChanges();

                    var result = this.mapper.Map<SuperViewModel>(_super.super);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSuper(int id)
        {
            var existingSuper = this.context.Super.Where(b => b.SuperId == id).FirstOrDefault();
            if (existingSuper == null)
            {
                return NotFound();
            }
            else
            {
                this.context.Super.Remove(existingSuper);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
