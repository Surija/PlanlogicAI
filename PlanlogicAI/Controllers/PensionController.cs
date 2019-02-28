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
    [Route("/api/pension")]
    public class PensionController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public PensionController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       // GET api/values
       [HttpGet("{id}")]
        public IEnumerable<PensionViewModel> GetPensions(int id)
        {
            IEnumerable<PensionViewModel> list = this.mapper.Map<IEnumerable<PensionViewModel>>(this.context.Pension.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        [HttpGet("{id}/{pensionID}")]
        public IEnumerable<PensionDetailsViewModel> GetPensionDetails(int id, int pensionID)
        {
            IEnumerable<PensionDetailsViewModel> list = this.mapper.Map<IEnumerable<PensionDetailsViewModel>>(this.context.PensionDetails.AsEnumerable().Where(m => (m.ClientId == id) && (m.PensionId == pensionID)));
            return list;
        }


        [HttpGet("{id}/{type}/{val}")]
        public IEnumerable<PensionDetailsViewModel> GetAllPensionDetails(int id, int type, int val)
        {
            IEnumerable<PensionDetailsViewModel> list = this.mapper.Map<IEnumerable<PensionDetailsViewModel>>(this.context.PensionDetails.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        // POST api/values
        [HttpPut("{id}")]
        public IActionResult CreatePension(int id, [FromBody] PensionAsset _pension)
        {
            if (ModelState.IsValid)
            {

                var existingPension= this.context.Pension.Where(b => b.PensionId == _pension.pension.PensionId).FirstOrDefault();
                if (existingPension == null)
                {
                    var pension = this.mapper.Map<Pension>(_pension.pension);

                    pension.ClientId = id;
                    this.context.Pension.Add(pension);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<PensionViewModel>(pension);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<PensionViewModel, Pension>(_pension.pension, existingPension);

                    this.context.Pension.Update(existingPension);
                    this.context.SaveChanges();

                    this.context.RemoveRange(this.context.PensionDetails.Where(m => (m.ClientId == id) && (m.PensionId == _pension.pension.PensionId)));
                    foreach (PensionDetailsViewModel p in _pension.pensionDrawdown)
                    {
                        var pen = this.mapper.Map<PensionDetails>(p);
                        pen.ClientId = id;
                        pen.PensionId = _pension.pension.PensionId;
                        this.context.PensionDetails.Add(pen);
                    }
                    this.context.SaveChanges();

                    var result = this.mapper.Map<PensionViewModel>(_pension.pension);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePension(int id)
        {
            var existingPension = this.context.Pension.Where(b => b.PensionId == id).FirstOrDefault();
            if (existingPension == null)
            {
                return NotFound();
            }
            else
            {
                this.context.Pension.Remove(existingPension);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
