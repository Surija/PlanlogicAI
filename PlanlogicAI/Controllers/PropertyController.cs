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
    [Route("/api/property")]
    public class PropertyController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public PropertyController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       // GET api/values
       [HttpGet("{id}")]
        public IEnumerable<PropertyViewModel> GetProperties(int id)
        {
            IEnumerable<PropertyViewModel> list = this.mapper.Map<IEnumerable<PropertyViewModel>>(this.context.Property.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        // POST api/values
        [HttpPut("{id}")]
        public IActionResult CreateProperty(int id, [FromBody] PropertyViewModel _property)
        {
            if (ModelState.IsValid)
            {

                var existingProp= this.context.Property.Where(b => b.PropertyId == _property.PropertyId).FirstOrDefault();
                if (existingProp == null)
                {
                    var property = this.mapper.Map<Property>(_property);

                    property.ClientId = id;
                    this.context.Property.Add(property);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<PropertyViewModel>(property);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<PropertyViewModel, Property>(_property, existingProp);

                    this.context.Property.Update(existingProp);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<PropertyViewModel>(_property);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProperty(int id)
        {
            var existingProp = this.context.Property.Where(b => b.PropertyId == id).FirstOrDefault();
            if (existingProp == null)
            {
                return NotFound();
            }
            else
            {
                this.context.Property.Remove(existingProp);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
