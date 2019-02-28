using AutoMapper;
//using Hangfire;
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
    [Route("/api/clients")]
    public class ClientController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public ClientController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET api/values
        //TODO - get all clients from SO.Clients
        [HttpGet]
        public IEnumerable<ClientViewModel> GetClientsAsync()
        {
            try
            {
                IEnumerable<ClientViewModel> list = this.mapper.Map<IEnumerable<ClientViewModel>>(this.context.Client.AsEnumerable().Where(m => m.AdvisorId == 1));
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //public async Task UpdateFundAsync()
        //{
        //   // var fundArray = await ApiUpdateController.LoadApi();
          
        //}


        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var existingClient = this.context.Client.Find(id);
            if (existingClient == null)
            {
                return NotFound();
            }
            else
            {
                var _client = this.mapper.Map<ClientViewModel>(existingClient);
                return Ok(_client);
            }

        }

        [HttpGet("{id}/{type}")]
        public IActionResult GetBasicDetails(int id,int type)
        {
            var existingClient = this.context.BasicDetails.Find(id);
            if (existingClient == null)
            {
                //var _client = this.mapper.Map<BasicDetailsViewModel>(new BasicDetails());
                //return Ok(_client);
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
        public IActionResult CreateClient([FromBody] ClientViewModel _client)
        {
            if (ModelState.IsValid)
            {
               
                var newClient = this.mapper.Map<Client>(_client);
                //TODO : Add Advisor ID based on login
                newClient.AdvisorId = 1;
                this.context.Client.Add(newClient);
                this.context.SaveChanges();

                var result = this.mapper.Map<ClientViewModel>(newClient);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] ClientBasicDetails _cl)
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
                    if (_cl.client.ClientId != 0)
                    {
                        this.mapper.Map<ClientViewModel, Client>(_cl.client, existingClient);
                        this.context.Client.Update(existingClient);
                        this.context.SaveChanges();
                    }

                    var existingBasicDetails = this.context.BasicDetails.Find(id);
                    if (existingBasicDetails == null)
                    {
                        var newBasicDetails = this.mapper.Map<BasicDetails>(_cl.basicDetails);
                        newBasicDetails.ClientId = id;
                        this.context.BasicDetails.Add(newBasicDetails);
                        this.context.SaveChanges();

                        var result = this.mapper.Map<BasicDetailsViewModel>(newBasicDetails);
                        return Ok(result);
                    }
                    else
                    {
                        this.mapper.Map<BasicDetailsViewModel, BasicDetails>(_cl.basicDetails, existingBasicDetails);

                        this.context.BasicDetails.Update(existingBasicDetails);
                        this.context.SaveChanges();

                        var result = this.mapper.Map<BasicDetailsViewModel>(_cl.basicDetails);
                        return Ok(result);
                    }

                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE api/values/5
        //TODO : Modify as required
        //[HttpDelete("{id}")]
        //public IActionResult DeleteVehicle(int id)
        //{
        //    var existingClient = this.context.Client.Find(id);
        //    if (existingClient == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        this.context.Client.Remove(existingClient);
        //        this.context.SaveChanges();
        //        return Ok(id);
        //    }

        //}

    }
}
