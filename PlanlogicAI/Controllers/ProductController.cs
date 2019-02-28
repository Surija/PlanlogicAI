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
    [Route("/api/product")]
    public class ProductController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public ProductController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<PlatformViewModel> GetPlatforms()
        {
            IEnumerable<PlatformViewModel> list = this.mapper.Map<IEnumerable<PlatformViewModel>>(this.context.Platform.AsEnumerable());
            return list;
        }

        [HttpGet("{id}")]
        public IEnumerable<ProductViewModel> GetProducts(int id)
        {
            IEnumerable<ProductViewModel> list = this.mapper.Map<IEnumerable<ProductViewModel>>(this.context.Product.AsEnumerable().Where(m => (m.PlatformId == id)));
            return list;
        }

        [HttpPost]
        public IActionResult CreatePlatform([FromBody] PlatformViewModel _platform)
        {
            if (ModelState.IsValid)
            {
                var newPlatform = this.mapper.Map<Platform>(_platform);
              
                this.context.Platform.Add(newPlatform);
                this.context.SaveChanges();

                var result = this.mapper.Map<PlatformViewModel>(newPlatform);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public IActionResult CreateProduct(int id, [FromBody] ProductViewModel _product)
        {
            if (ModelState.IsValid)
            {

                var existingProduct = this.context.Product.Where(b => b.ProductId == _product.ProductId && b.PlatformId == id ).FirstOrDefault();
                if (existingProduct == null)
                {
                    var product = this.mapper.Map<Product>(_product);

                    product.PlatformId = id;
                    this.context.Product.Add(product);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<ProductViewModel>(product);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<ProductViewModel, Product>(_product, existingProduct);

                    this.context.Product.Update(existingProduct);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<ProductViewModel>(_product);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}/{platformID}")]
        //delete underlying fees and - funds ?
        public IActionResult DeleteProduct(int id,int platformID)
        {
            var existingProduct = this.context.Product.Where(b => b.ProductId == id && b.PlatformId == platformID).FirstOrDefault();
            if (existingProduct == null)
            {
                return NotFound();
            }
            else
            {
                this.context.Product.Remove(existingProduct);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

        //[HttpPut("{id}")]
        //public IActionResult SaveData(int id, [FromBody] List<InvestmentFundViewModel> funds)
        //{


        //    bool status = false;


        //     if (ModelState.IsValid)  
        //    {

        //        foreach (var fund in funds)
        //        {

        //            var existingFund= this.context.InvestmentFund.Where(b => b.FundName == fund.FundName).FirstOrDefault();
        //            if (existingFund == null)
        //            {
        //                var f = this.mapper.Map<InvestmentFund>(fund);

        //                this.context.InvestmentFund.Add(f);
        //                //var result = this.mapper.Map<PropertyViewModel>(property);
        //                //return Ok(result);
        //            }
        //            else
        //            {
        //                this.mapper.Map<InvestmentFundViewModel, InvestmentFund>(fund, existingFund);

        //                this.context.InvestmentFund.Update(existingFund);

        //                //var result = this.mapper.Map<PropertyViewModel>(_property);
        //                //return Ok(result);
        //            }
        //        }

        //        this.context.SaveChanges();
        //        //foreach (var i in funds)
        //        //{
        //        //    this.context.InvestmentFund.Add(i);
        //        //}

        //        //this.context.SaveChanges();
        //        //status = true;
        //        ////var result = this.mapper.Map<ClientViewModel>(newClient);
        //        return Ok(status);
        //        ////return new JsonResult{ Data = new { status = status } };
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }

        //}

    }  
}
