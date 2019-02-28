using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
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
    [Route("/api/currentPortfolio")]
    public class CurrentPortfolioController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public CurrentPortfolioController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ProductViewModel> GetProducts ()
        {
            //IEnumerable<ProductViewModel> list = this.mapper.Map<IEnumerable<ProductViewModel>>(this.context.Product.AsEnumerable());
            IEnumerable<ProductViewModel> list = this.mapper.Map<IEnumerable<ProductViewModel>>(from s in context.Product.AsEnumerable()

                                                                                                select new ProductViewModel
                                                                                                {
                                                                                                    ProductId = s.ProductId,
                                                                                                    PlatformId = s.PlatformId,
                                                                                                    ProductName = s.SubType != "" ? "( " + s.SubType + " ) " +s.ProductName : s.ProductName,
                                                                                                    ProductType = s.ProductType,
                                                                                                    SubType = s.SubType,
                                                                                                });
            return list;
        }

      

        [HttpGet("{id}/{b}")]
        public IEnumerable<CurrentClientProductsViewModel> GetCurrentProducts(int id, string owner)
        {
            IEnumerable<CurrentClientProductsViewModel> list = this.mapper.Map<IEnumerable<CurrentClientProductsViewModel>>(this.context.CurrentClientProducts.AsEnumerable().Where(m => (m.ClientId == id)));
            return list;
        }

        [HttpGet("{id}")]
        public IEnumerable<CurrentClientFundsViewModel> GetCurrentFunds(int id)
        {
            //IEnumerable<CurrentClientFundsViewModel> list = this.mapper.Map<IEnumerable<CurrentClientFundsViewModel>>(this.context.CurrentClientFunds.AsEnumerable().Where(m => (m.HeaderId == id)));
            //return list;

            IEnumerable<CurrentClientFundsViewModel> list = this.mapper.Map<IEnumerable<CurrentClientFundsViewModel>>(from s in context.CurrentClientFunds.AsEnumerable()
                                                                                                                join sa in context.InvestmentFund.AsEnumerable() on s.Apircode equals sa.Apircode
                                                                                                                where s.HeaderId == id
                                                                                                                select new CurrentClientFundsViewModel
                                                                                                                {
                                                                                                                    RecId = s.RecId,
                                                                                                                    HeaderId = s.HeaderId,
                                                                                                                    Apircode = s.Apircode,
                                                                                                                    Value = s.Value,
                                                                                                                    Percentage = s.Percentage,
                                                                                                                    FundName = sa.FundName

                                                                                                                });
            return list;
        }

       

        [HttpPut("{clientId}/{owner}")]
        public IActionResult CreateProduct(int clientId,string owner, [FromBody] CurrentClientProductsViewModel _product)
        {
            if (ModelState.IsValid)
            {

                var existingProduct = this.context.CurrentClientProducts.Where(b => b.RecId == _product.RecId).FirstOrDefault();
                if (existingProduct == null)
                {
                    var product = this.mapper.Map<CurrentClientProducts>(_product);

                    product.ClientId = clientId;
                    product.Owner = product.Owner;
                    product.UnutilizedValue = product.Value;
                    this.context.CurrentClientProducts.Add(product);

                    this.context.SaveChanges();

                    var result = this.mapper.Map<IEnumerable<CurrentClientProductsViewModel>>(from s in context.CurrentClientProducts.AsEnumerable()
                                                                                              join sa in context.Product.AsEnumerable() on s.ProductId equals sa.ProductId
                                                                                              where s.RecId == product.RecId
                                                                                              select new CurrentClientProductsViewModel
                                                                                              {
                                                                                                  RecId = s.RecId,
                                                                                                  ProductId = s.ProductId,
                                                                                                  ClientId = s.ClientId,
                                                                                                  Owner = s.Owner,
                                                                                                  Value = s.Value,
                                                                                                  UnutilizedValue = s.UnutilizedValue,
                                                                                                  Percentage = s.Percentage,
                                                                                                  Product = sa.ProductName

                                                                                              }).FirstOrDefault();
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<CurrentClientProductsViewModel, CurrentClientProducts>(_product, existingProduct);

                    this.context.CurrentClientProducts.Update(existingProduct);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<CurrentClientProductsViewModel>(_product);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        //delete underlying fees and - funds ?
        public IActionResult DeleteProduct(int id)
        {
            var existingProduct = this.context.CurrentClientProducts.Where(b => b.RecId == id).FirstOrDefault();
            if (existingProduct == null)
            {
                return NotFound();
            }
            else
            {
                this.context.CurrentClientProducts.Remove(existingProduct);
                this.context.RemoveRange(this.context.CurrentClientFunds.Where(m => (m.HeaderId == id)));
                this.context.SaveChanges();

                 

                return Ok(id);
            }

        }



    }  
}
