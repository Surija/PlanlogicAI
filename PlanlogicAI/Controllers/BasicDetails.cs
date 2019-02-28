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
    [Route("/api/basicDetails")]
    public class BasicDetailsController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public BasicDetailsController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/values
        //TODO - get all clients from SO.Clients
        [HttpGet]
        public IEnumerable<BasicDetailsViewModel> GetClients()
        {
            IEnumerable<BasicDetailsViewModel> list = this.mapper.Map<IEnumerable<BasicDetailsViewModel>>(this.context.Client.AsEnumerable().Where(m => m.AdvisorId == 1));
            return list;
        }




        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var existingClient = this.context.BasicDetails.Find(id);
            if (existingClient == null)
            {
                return NotFound();
            }
            else
            {
                var _client = this.mapper.Map<BasicDetailsViewModel>(existingClient);
                return Ok(_client);
            }

        }

        // POST api/values
        [HttpPost]
        public IActionResult CreateClient([FromBody] BasicDetailsViewModel _client)
        {
            if (ModelState.IsValid)
            {
               
                var newClient = this.mapper.Map<Client>(_client);
                //TODO - Add ClientId 
                newClient.AdvisorId = 1;
                this.context.Client.Add(newClient);
                this.context.SaveChanges();

                var result = this.mapper.Map<BasicDetailsViewModel>(newClient);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] BasicDetailsViewModel _client)
        {
            if (ModelState.IsValid)
            {
                var existingClient = this.context.Client.Find(id);
                if (existingClient == null)
                {
                    return NotFound();
                }
                else
                {
                    this.mapper.Map<BasicDetailsViewModel, Client>(_client, existingClient);
                    //Update client details
                    //existingClient.Name = value.Name;
                    //existingClient.Mark = value.Mark;
                    //existingClient.Model = value.Model;
                    this.context.Client.Update(existingClient);
                    this.context.SaveChanges();
                     
                    var result = this.mapper.Map<BasicDetailsViewModel>(_client);
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE api/values/5
        //TODO : Modify as required
        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var existingClient = this.context.Client.Find(id);
            if (existingClient == null)
            {
                return NotFound();
            }
            else
            {
                this.context.Client.Remove(existingClient);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

    }
}
