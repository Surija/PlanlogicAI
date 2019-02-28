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
    [Route("/api/alternativeProduct")]
    public class AlternativeProductController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public AlternativeProductController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<AlternativeClientProductsViewModel> GetAllAlternativeProducts()
        {
            IEnumerable<AlternativeClientProductsViewModel> list = this.mapper.Map<IEnumerable<AlternativeClientProductsViewModel>>(this.context.AlternativeClientProducts.AsEnumerable());
            return list;
        }

        [HttpGet("{id}/{isSelective}")]
        public IEnumerable<AlternativeClientProductsViewModel> GetAlternativeProducts(int id,int isSelective)
        {
            IEnumerable<AlternativeClientProductsViewModel> list = this.mapper.Map<IEnumerable<AlternativeClientProductsViewModel>>(from s in context.AlternativeClientProducts.AsEnumerable()
                                                                                                                            join sa in context.Product.AsEnumerable() on s.ProductId equals sa.ProductId
                                                                                                                            where s.ProposedProduct == id
                                                                                                                            select new AlternativeClientProductsViewModel
                                                                                                                            {
                                                                                                                                RecId = s.RecId,
                                                                                                                                ProductId = s.ProductId,
                                                                                                                                ClientId = s.ClientId,
                                                                                                                                Owner = s.Owner,
                                                                                                                                Value = s.Value,
                                                                                                                                Percentage = s.Percentage,
                                                                                                                                Product = sa.ProductName,
                                                                                                                                ProposedProduct = s.ProposedProduct


                                                                                                                            });
            return list;
           
        }

        [HttpGet("{id}/{productId}/{temp2}/{temp3}")]
        public IEnumerable<AlternativeClientFundsViewModel> GetAlternativeFunds(int id, int productId, int temp2,int temp3)
        {
            IEnumerable<AlternativeClientFundsViewModel> list = this.mapper.Map<IEnumerable<AlternativeClientFundsViewModel>>(from s in context.AlternativeClientFunds.AsEnumerable()
                                                                                                                              join sa in context.InvestmentFund.AsEnumerable() on s.Apircode equals sa.Apircode
                                                                                                                              join p in context.ProductFund.AsEnumerable() on s.Apircode equals p.Apircode
                                                                                                                              where s.HeaderId == id && p.ProductId == productId
                                                                                                                              select new AlternativeClientFundsViewModel
                                                                                                                              {
                                                                                                                                RecId = s.RecId,
                                                                                                                                HeaderId = s.HeaderId,
                                                                                                                                Apircode = s.Apircode,
                                                                                                                                Value = s.Value,
                                                                                                                                Percentage = s.Percentage,
                                                                                                                                FundName = sa.FundName,
                                                                                                                                FeeLabel1 = p.FeeLabel1,
                                                                                                                                FeeLabel2 = p.FeeLabel2,
                                                                                                                                FeeLabel3 = p.FeeLabel3
                                                                                                                             });
            return list;

        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] AlternativeClientProductsViewModel _newProduct)
        {
            try
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    if (ModelState.IsValid)
                    {

                        var existingProduct = this.context.AlternativeClientProducts.Where(b => b.RecId == _newProduct.RecId).FirstOrDefault();
                        if (existingProduct == null)
                        {
                            var product = this.mapper.Map<AlternativeClientProducts>(_newProduct);
                            product.ProposedProduct = id;
                            this.context.AlternativeClientProducts.Add(product);
                            // this.context.SaveChanges();
                            // int newId = product.RecId;


                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            var res = this.context.Product.Where(b => b.ProductId == product.ProductId).FirstOrDefault();
                            var result = this.mapper.Map<AlternativeClientProductsViewModel>(product);
                            result.Product = res.ProductName;
                            return Ok(result);
                        }
                        else
                        {
                            this.mapper.Map<AlternativeClientProductsViewModel, AlternativeClientProducts>(_newProduct, existingProduct);
                            existingProduct.ProposedProduct = id;
                            this.context.AlternativeClientProducts.Update(existingProduct);
                            this.context.SaveChanges();

                            IEnumerable<AlternativeClientFundsViewModel> list = this.mapper.Map<IEnumerable<AlternativeClientFundsViewModel>>(this.context.AlternativeClientFunds.AsEnumerable().Where(m => (m.HeaderId == existingProduct.RecId)));

                            foreach (AlternativeClientFundsViewModel fund in list)
                            {
                                if (fund != null)
                                {
                                    var existingFund = this.context.AlternativeClientFunds.Where(b => b.RecId == fund.RecId).FirstOrDefault();
                                    if (fund.Percentage != 0)
                                    {
                                        fund.Value = existingProduct.Value * (fund.Percentage / 100);
                                    }
                                    this.mapper.Map<AlternativeClientFundsViewModel, AlternativeClientFunds>(fund, existingFund);

                                    this.context.AlternativeClientFunds.Update(existingFund);        
                                }
                            }

                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            var res = this.context.Product.Where(b => b.ProductId == _newProduct.ProductId).FirstOrDefault();
                            var result = this.mapper.Map<AlternativeClientProductsViewModel>(_newProduct);
                            result.Product = res.ProductName;
                            return Ok(result);
                        }


                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("{id}/{currentId}")]
        public IActionResult addAlternativeProduct(int id, int currentId, [FromBody] AlternativeClientFundsViewModel _fund)
        {
            if (ModelState.IsValid)
            {
     
                //var existingProduct = this.context.AlternativeClientProducts.Where(b => b.RecId == id).FirstOrDefault();
                //if (existingProduct == null)
                //{
                //    CurrentClientProducts prod = this.context.CurrentClientProducts.Where(a => a.RecId == currentId).FirstOrDefault();
                //    AlternativeClientProducts alternativeProduct = new AlternativeClientProducts();
                //    alternativeProduct.RecId = id;
                //    alternativeProduct.ProductId = prod.ProductId;
                //    alternativeProduct.Owner = prod.Owner;
                //    alternativeProduct.ClientId = prod.ClientId;
                //    alternativeProduct.Percentage = prod.Percentage;
                //    alternativeProduct.Value = prod.Value;
                //    this.context.AlternativeClientProducts.Add(alternativeProduct);
               
                //    //AlternativeReplacement prReplacement = new AlternativeReplacement();
                //    //prReplacement.CurrentId = currentId;
                //    //prReplacement.AlternativeId = id;
                //    //this.context.AlternativeReplacement.Add(prReplacement); 


                //    this.context.SaveChanges();
                //}

                 var updatedProduct = this.context.AlternativeClientProducts.Where(b => b.RecId == id).FirstOrDefault();
                var existingFund = this.context.AlternativeClientFunds.Where(b => b.RecId == _fund.RecId).FirstOrDefault();
                if (existingFund == null)
                {
                    var fund = this.mapper.Map<AlternativeClientFunds>(_fund);

                    fund.HeaderId = id;
                    this.context.AlternativeClientFunds.Add(fund);
                  
                    this.context.SaveChanges();

                    var result = this.mapper.Map<AlternativeClientFundsViewModel>(fund);                  
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<AlternativeClientFundsViewModel, AlternativeClientFunds>(_fund, existingFund);

                    this.context.AlternativeClientFunds.Update(existingFund);
                
                    this.context.SaveChanges();

                    var result = this.mapper.Map<AlternativeClientFundsViewModel>(_fund);
                    return Ok(result);
                }


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFund(int id)
        {
            var existingFund = this.context.AlternativeClientFunds.Where(b => b.RecId == id).FirstOrDefault();
            if (existingFund == null)
            {
                return NotFound();
            }
            else
            {
                this.context.AlternativeClientFunds.Remove(existingFund);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

        [HttpDelete("{id}/{clientId}")]
        public IActionResult DeleteProduct(int id, int clientId)
        {
            var existingProduct = this.context.AlternativeClientProducts.Where(b => b.RecId == id).FirstOrDefault();
            if (existingProduct == null)
            {
                return NotFound();
            }
            else
            {
                var funds = this.context.AlternativeClientFunds.Where(b => b.HeaderId == id);
                this.context.AlternativeClientFunds.RemoveRange(funds);
                this.context.AlternativeClientProducts.Remove(existingProduct);
                this.context.SaveChanges();
                return Ok(id);
            }

        }


    }
}
