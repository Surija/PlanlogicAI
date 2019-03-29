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
    [Route("/api/ropCurrent")]
    public class ROPCurrentController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public ROPCurrentController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<RopcurrentProductsViewModel> GetAllROPCurrentProducts()
        {
            IEnumerable<RopcurrentProductsViewModel> list = this.mapper.Map<IEnumerable<RopcurrentProductsViewModel>>(this.context.RopcurrentProducts.AsEnumerable());
            return list;
        }

        [HttpGet("{id}/{isSelective}")]
        public IEnumerable<RopcurrentProductsViewModel> GetROPCurrentProducts(int id,int isSelective)
        {
            IEnumerable<RopcurrentProductsViewModel> list = this.mapper.Map<IEnumerable<RopcurrentProductsViewModel>>(from s in context.RopcurrentProducts.AsEnumerable()
                                                                                                                            join sa in context.Product.AsEnumerable() on s.ProductId equals sa.ProductId
                                                                                                                            where s.ProposedProduct == id
                                                                                                                            select new RopcurrentProductsViewModel
                                                                                                                            {
                                                                                                                                RecId = s.RecId,
                                                                                                                                ProductId = s.ProductId,
                                                                                                                                ClientId = s.ClientId,
                                                                                                                                Owner = s.Owner,
                                                                                                                                Value = s.Value,
                                                                                                                                Percentage = s.Percentage,
                                                                                                                                Product = sa.ProductName,
                                                                                                                                ProposedProduct = s.ProposedProduct,
                                                                                                                                OriginalProduct = s.OriginalProduct

                                                                                                                            });
            return list;
           
        }

        [HttpGet("{id}/{productId}/{temp2}/{temp3}")]
        public IEnumerable<RopcurrentFundsViewModel> GetROPCurrentFunds(int id, int productId, int temp2,int temp3)
        {
            IEnumerable<RopcurrentFundsViewModel> list = this.mapper.Map<IEnumerable<RopcurrentFundsViewModel>>(from s in context.RopcurrentFunds.AsEnumerable()
                                                                                                                      join sa in context.InvestmentFund.AsEnumerable() on s.Apircode equals sa.Apircode
                                                                                                                      join p in context.ProductFund.AsEnumerable() on s.Apircode equals p.Apircode
                                                                                                                      where s.HeaderId == id && p.ProductId == productId
                                                                                                                      select new RopcurrentFundsViewModel
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
        public IActionResult UpdateProduct(int id, [FromBody] RopcurrentProductsViewModel _newProduct)
        {
            try
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    if (ModelState.IsValid)
                    {

                        var existingProduct = this.context.RopcurrentProducts.Where(b => b.RecId == _newProduct.RecId).FirstOrDefault();
                        if (existingProduct == null)
                        {
                            var product = this.mapper.Map<RopcurrentProducts>(_newProduct);
                            product.ProposedProduct = id;
                            this.context.RopcurrentProducts.Add(product);


                            //CurrentClientProductsViewModel current = this.mapper.Map<CurrentClientProductsViewModel>(this.context.CurrentClientProducts.Where(m => (m.ProductId == product.ProductId)).FirstOrDefault());
                            //if (current != null && current.RecId != 0)
                            //{
                            //    IEnumerable<CurrentClientFundsViewModel> list = this.mapper.Map<IEnumerable<CurrentClientFundsViewModel>>(this.context.CurrentClientFunds.AsEnumerable().Where(m => (m.HeaderId == current.RecId)));

                            //    foreach (CurrentClientFundsViewModel fund in list)
                            //    {
                            //        if (fund != null)
                            //        {

                            //            RopcurrentFunds pf = new RopcurrentFunds();
                            //            pf.HeaderId = id;
                            //            pf.Apircode = fund.Apircode;
                            //            pf.Value = fund.Value;
                            //            pf.Percentage = fund.Percentage;

                            //            var pfDetails = this.mapper.Map<RopcurrentFunds>(pf);
                            //            this.context.RopcurrentFunds.Add(pfDetails);
                            //        }
                            //    }
                            //}



                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            var res = this.context.Product.Where(b => b.ProductId == product.ProductId).FirstOrDefault();
                            var result = this.mapper.Map<RopcurrentProductsViewModel>(product);
                            result.Product = res.ProductName;
                            return Ok(result);
                        }
                        else
                        {
                            this.mapper.Map<RopcurrentProductsViewModel, RopcurrentProducts>(_newProduct, existingProduct);
                            existingProduct.ProposedProduct = id;
                            this.context.RopcurrentProducts.Update(existingProduct);
                            this.context.SaveChanges();

                            IEnumerable<RopcurrentFundsViewModel> list = this.mapper.Map<IEnumerable<RopcurrentFundsViewModel>>(this.context.RopcurrentFunds.AsEnumerable().Where(m => (m.HeaderId == existingProduct.RecId)));

                            foreach (RopcurrentFundsViewModel fund in list)
                            {
                                if (fund != null)
                                {
                                    var existingFund = this.context.RopcurrentFunds.Where(b => b.RecId == fund.RecId).FirstOrDefault();
                                    if (fund.Percentage != 0)
                                    {
                                        fund.Value = existingProduct.Value * (fund.Percentage / 100);
                                    }
                                    this.mapper.Map<RopcurrentFundsViewModel, RopcurrentFunds>(fund, existingFund);

                                    this.context.RopcurrentFunds.Update(existingFund);
                                }
                            }

                            this.context.SaveChanges();
                            dbContextTransaction.Commit();

                            var res = this.context.Product.Where(b => b.ProductId == _newProduct.ProductId).FirstOrDefault();
                            var result = this.mapper.Map<RopcurrentProductsViewModel>(_newProduct);
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
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}/{currentId}")]
        public IActionResult addROPCurrentFund(int id, int currentId, [FromBody] RopcurrentFundsViewModel _fund)
        {
            if (ModelState.IsValid)
            {
                 var updatedProduct = this.context.RopcurrentProducts.Where(b => b.RecId == id).FirstOrDefault();
                var existingFund = this.context.RopcurrentFunds.Where(b => b.RecId == _fund.RecId).FirstOrDefault();
                if (existingFund == null)
                {
                    var fund = this.mapper.Map<RopcurrentFunds>(_fund);

                    fund.HeaderId = id;
                    this.context.RopcurrentFunds.Add(fund);
                  
                    this.context.SaveChanges();

                    var result = this.mapper.Map<RopcurrentFundsViewModel>(fund);                  
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<RopcurrentFundsViewModel, RopcurrentFunds>(_fund, existingFund);

                    this.context.RopcurrentFunds.Update(existingFund);
                
                    this.context.SaveChanges();

                    var result = this.mapper.Map<RopcurrentFundsViewModel>(_fund);
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
            var existingFund = this.context.RopcurrentFunds.Where(b => b.RecId == id).FirstOrDefault();
            if (existingFund == null)
            {
                return NotFound();
            }
            else
            {
                this.context.RopcurrentFunds.Remove(existingFund);
                this.context.SaveChanges();
                return Ok(id);
            }

        }

        [HttpDelete("{id}/{clientId}")]
        public IActionResult DeleteProduct(int id, int clientId)
        {
            var existingProduct = this.context.RopcurrentProducts.Where(b => b.RecId == id).FirstOrDefault();
            if (existingProduct == null)
            {
                return NotFound();
            }
            else
            {
                var funds = this.context.RopcurrentFunds.Where(b => b.HeaderId == id);
                this.context.RopcurrentFunds.RemoveRange(funds);
                this.context.RopcurrentProducts.Remove(existingProduct);
                this.context.SaveChanges();
                return Ok(id);
            }

        }
    }
}
