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
    [Route("/api/newProduct")]
    public class NewProductController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
       public NewProductController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

      

        //[HttpPut("{id}")]
        // public IActionResult UpdateProduct(int clientId, string owner, [FromBody] ProposedClientProductsViewModel _producint id, [FromBody] NewProduct _newProduct)
        [HttpPut("{clientId}")]
        public IActionResult UpdateProduct(int clientId, [FromBody] ProposedClientProductsViewModel _product)

        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {

                    var existingProduct = this.context.ProposedClientProducts.Where(b => b.RecId == _product.RecId).FirstOrDefault();
                    if (existingProduct == null)
                    {
                        var product = this.mapper.Map<ProposedClientProducts>(_product);
                        //   product.RecId = id;
                        product.Status = 1;
                        product.ClientId = clientId;
                        product.Owner = product.Owner;
                        this.context.ProposedClientProducts.Add(product);
                        // this.context.SaveChanges();
                        // int newId = product.RecId;

                        this.context.SaveChanges();
                        dbContextTransaction.Commit();

                        var res = this.context.Product.Where(b => b.ProductId == product.ProductId).FirstOrDefault();
                     //   var result = this.mapper.Map<ProposedClientProductsViewModel>(product);
                        var result = mapper.Map<ProposedClientProductsViewModel>(from s in context.ProposedClientProducts.AsEnumerable()
                                                                                                                                     join sa in context.Product.AsEnumerable() on s.ProductId equals sa.ProductId
                                                                                                                                     join p in context.Platform.AsEnumerable() on sa.PlatformId equals p.PlatformId
                                                                                                                                     where s.RecId == product.RecId
                                                                                                                                     select new ProposedClientProductsViewModel
                                                                                                                                     {
                                                                                                                                         RecId = s.RecId,
                                                                                                                                         ProductId = s.ProductId,
                                                                                                                                         ClientId = s.ClientId,
                                                                                                                                         Owner = s.Owner,
                                                                                                                                         Value = s.Value,
                                                                                                                                         Percentage = s.Percentage,
                                                                                                                                         Product = sa.ProductName,
                                                                                                                                         Status = s.Status,
                                                                                                                                         isEqual = 1,
                                                                                                                                         CurrentId = 0,
                                                                                                                                         PlatformName = p.PlatformName,
                                                                                                                                         PlatformId = p.PlatformId

                                                                                                                                     });

                        result.Product = res.ProductName; 
                        return Ok(result);




                    }
                    else
                    {
                        this.mapper.Map<ProposedClientProductsViewModel, ProposedClientProducts>(_product, existingProduct);

                        this.context.ProposedClientProducts.Update(existingProduct);
                        this.context.SaveChanges();

                        IEnumerable<ProposedClientFundsViewModel> list = this.mapper.Map<IEnumerable<ProposedClientFundsViewModel>>(this.context.ProposedClientFunds.AsEnumerable().Where(m => (m.HeaderId == existingProduct.RecId)));

                        foreach (ProposedClientFundsViewModel fund in list)
                        {
                            if (fund != null)
                            {
                                var existingFund = this.context.ProposedClientFunds.Where(b => b.RecId == fund.RecId).FirstOrDefault();
                                if (fund.Percentage != 0)
                                {
                                    fund.Value = existingProduct.Value * (fund.Percentage / 100);
                                }
                                this.mapper.Map<ProposedClientFundsViewModel, ProposedClientFunds>(fund, existingFund);

                                this.context.ProposedClientFunds.Update(existingFund);
                            }
                        }

                        this.context.SaveChanges();
                        dbContextTransaction.Commit();


                        var result = this.mapper.Map<ProposedClientProductsViewModel>(_product);
                        return Ok(result);
                    }


                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
        }

        //[HttpPut("{id}/{currentId}")]
        //public IActionResult UpdateRollFundsIn(int id,int currentId, [FromBody] NewProduct _newProduct)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var existingProduct = this.context.ProposedClientProducts.Where(b => b.ProductId == _newProduct.proposedProduct.ProductId &&  b.RecId == id).FirstOrDefault();
        //        if (existingProduct == null)
        //        {
        //            var product = this.mapper.Map<ProposedClientProducts>(_newProduct.proposedProduct);
        //            product.RecId = id;
        //            product.Status = 3;
        //            this.context.ProposedClientProducts.Add(product);

        //            ProductReplacement prReplacementMain = new ProductReplacement();
        //            prReplacementMain.CurrentId = currentId;
        //            prReplacementMain.ProposedId = _newProduct.proposedProduct.RecId;
        //            this.context.ProductReplacement.Add(prReplacementMain);

        //            foreach (ProductLinks prod in _newProduct.currentProducts)
        //            {
        //                if (prod.product != 0)
        //                {
        //                    ProductReplacement prReplacement = new ProductReplacement();
        //                    prReplacement.CurrentId = prod.product;
        //                    prReplacement.ProposedId = id;
        //                    this.context.ProductReplacement.Add(prReplacement);
        //                }
        //            }


        //            IEnumerable<CurrentClientFundsViewModel> list = this.mapper.Map<IEnumerable<CurrentClientFundsViewModel>>(this.context.CurrentClientFunds.AsEnumerable().Where(m => (m.HeaderId == currentId)));

        //            foreach (CurrentClientFundsViewModel fund in list)
        //            {
        //                if (fund != null)
        //                {

        //                    ProposedClientFunds pf = new ProposedClientFunds();
        //                    pf.HeaderId = _newProduct.proposedProduct.RecId;
        //                    pf.Apircode = fund.Apircode;
        //                    if (_newProduct.proposedProduct.isEqual == 1)
        //                    {
        //                        pf.Value = fund.Value;
        //                        pf.Percentage = fund.Percentage;
        //                    }
        //                    else
        //                    {
        //                        pf.Value = 0;
        //                        pf.Percentage = 0;
        //                    }
        //                    var pfDetails = this.mapper.Map<ProposedClientFunds>(pf);
        //                    this.context.ProposedClientFunds.Add(pfDetails);
        //                }
        //            }


        //            this.context.SaveChanges();




        //            var res = this.context.Product.Where(b => b.ProductId == product.ProductId).FirstOrDefault();


        //            var result = this.mapper.Map<ProposedClientProductsViewModel>(product);
        //            result.Product = res.ProductName;
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            this.mapper.Map<ProposedClientProductsViewModel, ProposedClientProducts>(_newProduct.proposedProduct, existingProduct);

        //            this.context.ProposedClientProducts.Update(existingProduct);
        //            this.context.SaveChanges();

        //            var result = this.mapper.Map<ProposedClientProductsViewModel>(_newProduct.proposedProduct);
        //            return Ok(result);
        //        }


        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}


        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var existingProduct = this.context.ProposedClientProducts.Where(b => b.RecId == id).FirstOrDefault();
            if (existingProduct == null)
            {
                return NotFound();
            }
            else
            {
                var funds = this.context.ProposedClientFunds.Where(b => b.HeaderId == id);
                this.context.ProposedClientFunds.RemoveRange(funds);

                RopcurrentProducts currentProduct = this.context.RopcurrentProducts.Where(r => r.ProposedProduct == id).FirstOrDefault() ;
                if(currentProduct != null)
                {
                    var currentFunds = this.context.RopcurrentFunds.Where(f => f.HeaderId == currentProduct.RecId);
                    this.context.RopcurrentFunds.RemoveRange(currentFunds);
                    this.context.RopcurrentProducts.Remove(currentProduct);
                }

                AlternativeClientProducts altProduct = this.context.AlternativeClientProducts.Where(r => r.ProposedProduct == id).FirstOrDefault();
                if (altProduct != null)
                {
                    var altFunds = this.context.AlternativeClientFunds.Where(f => f.HeaderId == altProduct.RecId);
                    this.context.AlternativeClientFunds.RemoveRange(altFunds);
                    this.context.AlternativeClientProducts.Remove(altProduct);
                }

                var replacement = this.context.ProductReplacement.Where(p => p.ProposedId == id);
                if(replacement != null)
                {
                    this.context.RemoveRange(replacement);
                }

                this.context.ProposedClientProducts.Remove(existingProduct);
                this.context.SaveChanges();
                return Ok(id);
            }

        }
    }
}
