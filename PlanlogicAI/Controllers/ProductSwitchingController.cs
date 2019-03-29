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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace PlanlogicAI.Controllers
{
    [Route("/api/productSwitching")]
    public class ProductSwitchingController : Controller
    {
        private readonly IMapper mapper;
        private readonly StrategyOptimizerPrototypeContext context;
        public ProductSwitchingController(StrategyOptimizerPrototypeContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ProposedClientProductsViewModel> GetAllProposedProducts()
        {
            IEnumerable<ProposedClientProductsViewModel> list = this.mapper.Map<IEnumerable<ProposedClientProductsViewModel>>(this.context.ProposedClientProducts.AsEnumerable());
            return list;
        }

        [HttpGet("{id}")]
        public IEnumerable<CurrentClientProductsViewModel> GetCurrentProducts(int id)
        {


            IEnumerable<CurrentClientProductsViewModel> list = this.mapper.Map<IEnumerable<CurrentClientProductsViewModel>>(from s in context.CurrentClientProducts.AsEnumerable()
                                                                                                                            join sa in context.Product.AsEnumerable() on s.ProductId equals sa.ProductId
                                                                                                                            join p in context.Platform.AsEnumerable() on sa.PlatformId equals p.PlatformId
                                                                                                                            where s.ClientId == id
                                                                                                                            select new CurrentClientProductsViewModel
                                                                                                                            {
                                                                                                                                RecId = s.RecId,
                                                                                                                                ProductId = s.ProductId,
                                                                                                                                ClientId = s.ClientId,
                                                                                                                                Owner = s.Owner,
                                                                                                                                Value = s.Value,
                                                                                                                                UnutilizedValue = s.UnutilizedValue,
                                                                                                                                Percentage = s.Percentage,
                                                                                                                                Product = sa.ProductName,
                                                                                                                                PlatformName = p.PlatformName
                                                                                                                              
                                                                                                                            });
            return list;
        }

        [HttpGet("{id}/{isProposed}")]
        public IEnumerable<ProposedClientProductsViewModel> GetProposedProducts(int id, int isProposed)
        {

            try
            {

                //var t = (from s in context.ProposedClientProducts.AsEnumerable()
                //        join sa in context.Product.AsEnumerable() on s.ProductId equals sa.ProductId
                //        join p in context.Platform.AsEnumerable() on sa.PlatformId equals p.PlatformId
                //        where s.ClientId == id
                //        select new ProposedClientProductsViewModel
                //        {
                //            RecId = s.RecId,
                //            ProductId = s.ProductId,
                //            ClientId = s.ClientId,
                //            Owner = s.Owner,
                //            Value = s.Value,
                //            Percentage = s.Percentage,
                //            Product = sa.ProductName,
                //            Status = s.Status,
                //            isEqual = 1,
                //            CurrentId = 0,
                //            PlatformName = p.PlatformName


                //        });
                //IEnumerable<ProposedClientProductsViewModel> list;
                //if (t.Any())
                //{
                //  list  = this.mapper.Map<IEnumerable<ProposedClientProductsViewModel>>(t);
                //}
                //else
                //{
                //    list = new List<ProposedClientProductsViewModel>();
                //}

                IEnumerable<ProposedClientProductsViewModel> list = mapper.Map<IEnumerable<ProposedClientProductsViewModel>>(from s in context.ProposedClientProducts.AsEnumerable()
                                                                                                                             join sa in context.Product.AsEnumerable() on s.ProductId equals sa.ProductId
                                                                                                                             join p in context.Platform.AsEnumerable() on sa.PlatformId equals p.PlatformId
                                                                                                                             where s.ClientId == id
                                                                                                                             select new CurrentClientProductsViewModel
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
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("{id}/{temp1}/{temp2}")]
        public IEnumerable<CurrentClientFundsViewModel> GetCurrentFunds(int id, int temp1, int temp2)
        {
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

        [HttpGet("{id}/{productId}/{temp2}/{temp3}")]
        public IEnumerable<ProposedClientFundsViewModel> GetProposedFunds(int id, int productId, int temp2, int temp3)
        {
            IEnumerable<ProposedClientFundsViewModel> list = this.mapper.Map<IEnumerable<ProposedClientFundsViewModel>>(from s in context.ProposedClientFunds.AsEnumerable()
                                                                                                                        join sa in context.InvestmentFund.AsEnumerable() on s.Apircode equals sa.Apircode
                                                                                                                        join p in context.ProductFund.AsEnumerable() on s.Apircode equals p.Apircode
                                                                                                                        where s.HeaderId == id && p.ProductId == productId
                                                                                                                        select new ProposedClientFundsViewModel
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
        public IActionResult Retain(int id, [FromBody] ProposedClientProductsViewModel _product)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        var existingProduct = this.context.ProposedClientProducts.Where(b => b.ProductId == _product.ProductId && b.RecId == _product.RecId).FirstOrDefault();
                        if (existingProduct == null)
                        {
                            var product = this.mapper.Map<ProposedClientProducts>(_product);
                            product.Status = 0;
                            this.context.ProposedClientProducts.Add(product);

                            this.context.SaveChanges();

                            var header = this.mapper.Map<ProposedClientProductsViewModel>(product);

                            //ProductReplacement prReplacement = new ProductReplacement();
                            //prReplacement.CurrentId = id;
                            //prReplacement.ProposedId = _product.RecId;
                            //this.context.ProductReplacement.Add(prReplacement);

                            IEnumerable<CurrentClientFundsViewModel> list = this.mapper.Map<IEnumerable<CurrentClientFundsViewModel>>(this.context.CurrentClientFunds.AsEnumerable().Where(m => (m.HeaderId == id)));

                            foreach (CurrentClientFundsViewModel fund in list)
                            {
                                if (fund != null)
                                {

                                    ProposedClientFunds pf = new ProposedClientFunds();
                                    pf.HeaderId = _product.RecId;
                                    pf.Apircode = fund.Apircode;
                                    if (_product.isEqual == 1)
                                    {
                                        pf.Value = fund.Value;
                                        pf.Percentage = fund.Percentage;
                                    }
                                    else
                                    {
                                        pf.Value = 0;
                                        pf.Percentage = 0;
                                    }
                                    var pfDetails = this.mapper.Map<ProposedClientFunds>(pf);
                                    this.context.ProposedClientFunds.Add(pfDetails);
                                }
                            }

                            RopcurrentProducts currentProduct = new RopcurrentProducts();
                            currentProduct.RecId = _product.CurrentId;
                            currentProduct.Owner = header.Owner;
                            currentProduct.Percentage = header.Percentage;
                            currentProduct.ProductId = header.ProductId;
                            currentProduct.Value = header.Value;
                            currentProduct.ProposedProduct = header.RecId;
                            currentProduct.OriginalProduct = id;
                            currentProduct.ClientId = header.ClientId;
                            this.context.RopcurrentProducts.Add(currentProduct);

                            this.context.SaveChanges();

                            var currentHeader = this.mapper.Map<RopcurrentProductsViewModel>(currentProduct);

                            foreach (CurrentClientFundsViewModel fund in list)
                            {
                                if (fund != null)
                                {

                                    RopcurrentFunds pf = new RopcurrentFunds();
                                    pf.HeaderId = currentHeader.RecId;
                                    pf.Apircode = fund.Apircode;
                                    if (_product.isEqual == 1)
                                    {
                                        pf.Value = fund.Value;
                                        pf.Percentage = fund.Percentage;
                                    }
                                    else
                                    {
                                        pf.Value = 0;
                                        pf.Percentage = 0;
                                    }
                                    var pfDetails = this.mapper.Map<RopcurrentFunds>(pf);
                                    this.context.RopcurrentFunds.Add(pfDetails);
                                }
                            }

                            this.context.SaveChanges();
                            dbContextTransaction.Commit();



                            var result = this.mapper.Map<ProposedClientProductsViewModel>(product);
                            result.RopCurrentId = currentHeader.RecId;
                            return Ok(result);
                        }
                        else
                        {
                       
                            this.mapper.Map<ProposedClientProductsViewModel, ProposedClientProducts>(_product, existingProduct);

                            this.context.ProposedClientProducts.Update(existingProduct);
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
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        [HttpPut("{id}/{currentId}")]
        public IActionResult Rebalance(int id, int currentId, [FromBody] ProposedClientProductsViewModel _product)
        {
            if (ModelState.IsValid)
            {

                var existingProduct = this.context.ProposedClientProducts.Where(b => b.RecId == id).FirstOrDefault();
                if (existingProduct == null)
                {
                    CurrentClientProducts prod = this.context.CurrentClientProducts.Where(a => a.RecId == currentId).FirstOrDefault();
                    ProposedClientProducts proposedProduct = new ProposedClientProducts();
                    proposedProduct.RecId = id;
                    proposedProduct.ProductId = prod.ProductId;
                    proposedProduct.Owner = prod.Owner;
                    proposedProduct.ClientId = prod.ClientId;
                    proposedProduct.Percentage = prod.Percentage;
                    proposedProduct.Value = prod.UnutilizedValue;
                    proposedProduct.Status = 1;
                    this.context.ProposedClientProducts.Add(proposedProduct);
                    //this.context.SaveChanges();

                    // newId = proposedProduct.RecId;

                    ProductReplacement prReplacement = new ProductReplacement();
                    prReplacement.CurrentId = currentId;
                    prReplacement.ProposedId = id;
                    this.context.ProductReplacement.Add(prReplacement);


                    this.context.SaveChanges();

                    var res = this.context.Product.Where(b => b.ProductId == proposedProduct.ProductId).FirstOrDefault();
                    var result = this.mapper.Map<ProposedClientProductsViewModel>(proposedProduct);
                    result.Product = res.ProductName;
                    return Ok(result);

                }
                else
                {
                    this.mapper.Map<ProposedClientProductsViewModel, ProposedClientProducts>(_product, existingProduct);

                    this.context.ProposedClientProducts.Update(existingProduct);
                    this.context.SaveChanges();

                    var res = this.context.Product.Where(b => b.ProductId == _product.ProductId).FirstOrDefault();
                    var result = this.mapper.Map<ProposedClientProductsViewModel>(_product);
                    result.Product = res.ProductName;
                    return Ok(result);
                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut("{id}/{currentId}/{percentage}")]
        public IActionResult CreateFund(int id, int currentId, decimal percentage, [FromBody] ProposedClientFundsViewModel _fund)
        {
            if (ModelState.IsValid)
            {

                var existingProduct = this.context.ProposedClientProducts.Where(b => b.RecId == id).FirstOrDefault();
                if (existingProduct == null)
                {
                    CurrentClientProducts prod = this.context.CurrentClientProducts.Where(a => a.RecId == currentId).FirstOrDefault();
                    ProposedClientProducts proposedProduct = new ProposedClientProducts();
                    proposedProduct.RecId = id;
                    proposedProduct.ProductId = prod.ProductId;
                    proposedProduct.Owner = prod.Owner;
                    proposedProduct.ClientId = prod.ClientId;
                    proposedProduct.Percentage = prod.Percentage;
                    proposedProduct.Value = prod.UnutilizedValue;
                    proposedProduct.Status = 1;
                    this.context.ProposedClientProducts.Add(proposedProduct);
                    //this.context.SaveChanges();

                    // newId = proposedProduct.RecId;

                    ProductReplacement prReplacement = new ProductReplacement();
                    prReplacement.CurrentId = currentId;
                    prReplacement.ProposedId = id;
                    this.context.ProductReplacement.Add(prReplacement);


                    this.context.SaveChanges();
                }

                var updatedProduct = this.context.ProposedClientProducts.Where(b => b.RecId == id).FirstOrDefault();
                var existingFund = this.context.ProposedClientFunds.Where(b => b.RecId == _fund.RecId).FirstOrDefault();
                if (existingFund == null)
                {
                    var fund = this.mapper.Map<ProposedClientFunds>(_fund);

                    fund.HeaderId = id;
                    this.context.ProposedClientFunds.Add(fund);
                    //updatedProduct.Percentage = percentage;
                    //this.context.ProposedClientProducts.Update(updatedProduct);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<ProposedClientFundsViewModel>(fund);
                    return Ok(result);
                }
                else
                {
                    this.mapper.Map<ProposedClientFundsViewModel, ProposedClientFunds>(_fund, existingFund);

                    this.context.ProposedClientFunds.Update(existingFund);
                    //updatedProduct.Percentage = percentage;
                    //this.context.ProposedClientProducts.Update(updatedProduct);
                    this.context.SaveChanges();

                    var result = this.mapper.Map<ProposedClientFundsViewModel>(_fund);
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
            var existingFund = this.context.ProposedClientFunds.Where(b => b.RecId == id).FirstOrDefault();
            if (existingFund == null)
            {
                return NotFound();
            }
            else
            {
                this.context.ProposedClientFunds.Remove(existingFund);
                this.context.SaveChanges();
                return Ok(id);
            }

        }




        //public void GenerateWord()
        //{

        //    //var stream = new MemoryStream();
        //    //using (WordprocessingDocument doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
        //    //{
        //    //    MainDocumentPart mainPart = doc.AddMainDocumentPart();
        //    //    new Document(new Body()).Save(mainPart);

        //    //    Body body = mainPart.Document.Body;
        //    //    body.Append(new Paragraph(new Run(new Text("hello"))));
        //    //    mainPart.Document.Save();
        //    //}

        //}

    }
}
