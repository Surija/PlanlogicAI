import { Component, OnInit } from '@angular/core';
import { ProductService } from './../../services/product.service';
import { FeeService } from './../../services/fee.service';
import { InvestmentFundService } from './../../services/investment-fund.service';
import { ClientService } from './../../services/client.service';
import { CurrentPortfolioService } from './../../services/current-portfolio.service';
import { CurrentPortfolioFundService } from './../../services/current-portfolio-fund.service';


import { Client } from './../../models/Client';

import * as $ from 'jquery';
import * as XLSX from 'xlsx';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { DragulaModule, DragulaService } from 'ng2-dragula';

@Component({
  selector: 'app-current-portfolio',
  templateUrl: './current-portfolio.component.html',
  styleUrls: ['./current-portfolio.component.css']
})
export class CurrentPortfolioComponent implements OnInit {

    clients: Client[] | undefined;
    owner: any = [];
    clientDetails: any;
    selectedClient: any;
    selectedOwner: any = "Client";
    selectedProduct: number = 0;
    currentProducts: any = [];
    currentProductDetails: any = {
        productId: 0,
        value: 0,
        percentage: 100,
        owner: 'Client'
    };
    currentFunds: any = [];
    currentFundDetails: any = {
        apircode: "",
        value: 0,
        percentage: 0
    };
    products: any = [];
    funds: any = [];
    totalPercentage: number = 0;
    backDropDisplay = 'none';
    fundDisplay = 'none';
    selectedProductDetails: any = {};
    newFund = 'none';
    existingFund = 'block';
    investmentDetails: any = {
        apircode: ""
        , fundName: ""
        , amount: 0
        , buySpread: 0
        , icr: 0
        , domesticEquity: 0
        , internationalEquity: 0
        , domesticProperty: 0
        , internationalProperty: 0
        , growthAlternatives: 0
        , defensiveAlternatives: 0
        , domesticFixedInterest: 0
        , internationalFixedInterest: 0
        , domesticCash: 0
        , internationalCash: 0
        , otherGrowth: 0
    };

    newFundDefault: any;
  

    constructor(private route: ActivatedRoute,
        private router: Router, private clientService: ClientService, private portfolioService: CurrentPortfolioService, private portfolioFundService: CurrentPortfolioFundService, private feeService: InvestmentFundService) { }

    ngOnInit() {
        $(document).ready(function () {
            (<any>$('[data-toggle="popover"]')).popover({ html: true });

        });

        $(document).on('click', function (e) {
            $('[data-toggle="popover"],[data-original-title]').each(function () {
                if (!$(this).is(<any>(e.target)) && $(this).has(<any>(e.target)).length === 0 && $('.popover').has(<any>(e.target)).length === 0) {
                    (((<any>$(this)).popover('hide').data('bs.popover') || {}).inState || {}).click = false 
                }
            });
        })

        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
        this.clientDetails = JSON.parse(localStorage.getItem('ClientDetails') || '{}');
        if (this.clientDetails.maritalStatus == "S") {
            this.owner = ["Client"];
        } else {
            this.owner = ["Client", "Partner", "Joint"];
        }   

        this.portfolioService.getCurrentProducts(this.selectedClient, this.owner).subscribe(
            currentProducts => {
                 this.currentProducts = currentProducts;

            }
        );

       // this.changeOwner("Client");
        this.portfolioService.getProducts().subscribe(
            prod => {
                this.products = prod;

            }
        );
       
  }

    //changeOwner(owner: any) {
    //    this.selectedOwner = owner;

    //    this.portfolioService.getCurrentProducts(this.selectedClient,owner).subscribe(
    //        currentProducts => {
    //            this.currentProducts = currentProducts;

    //        }
    //    );

    //}

    onCloseFundPortfolio() {
        this.backDropDisplay = 'none';
        this.fundDisplay = 'none';
        this.selectedProductDetails = {};
    }

    deleteCurrentProduct(item: any, index: any) {
        var id$ = item.recId;
        this.portfolioService.delete(id$).subscribe((data) => {
            this.currentProducts.forEach((pData: any, index: any) => {
                if (pData.recId == id$) {
                    this.currentProducts.splice(index, 1);
                }
            });
            this.currentFunds.forEach((fData: any, index: any) => {
                if (fData.headerId == id$) {
                    this.currentFunds.splice(index, 1);
                }
            });

            this.selectedProduct = 0;
        });
    }

    addcurrentProduct(currentProduct: any) {
        this.portfolioService.createProduct(this.currentProductDetails, this.selectedClient, this.selectedOwner).subscribe((data) => {
            this.currentProducts.push(data);

            this.setCurrentProduct(data);
            this.currentProductDetails = {
                productId: 0,
                value: 0,
                percentage: 100,
                owner: 'Client'
            };
        });
    }

    editCurrentProduct(currentProduct: any) {
        currentProduct.percentage = 100;
        this.portfolioService.createProduct(currentProduct, this.selectedClient, this.selectedOwner).subscribe((data: any) => {
            var id$ = data.recId;
            if (this.currentProducts.length > 0) {
                this.currentProducts.forEach((pData: any, index: any) => {
                    if (pData.recId == id$) {
                        this.currentProducts.splice(index, 1, data);
                      // this.currentProducts.push(data);
                    }

                });
            }

            this.currentProductDetails = {
                productId: 0,
                value: 0,
                percentage: 100,
                owner: 'Client'
            };
        });
    }

    displayFundPopup(selectedVal: any) {
        if (selectedVal == "Add new fund") {
            this.existingFund = 'none';
            this.newFund = 'block';
        }
    }

    //getFundDefault() {
    //    return this.newFundDefault;
    //}


    calculatePercentage(val: any, i: number) {
        var tempTotal = 0;
        if (i == 0) {
            this.currentFunds.forEach((pData: any) => {
                tempTotal += parseFloat(pData.percentage);
            });
        }
        else {
            this.currentFunds.forEach((pData: any) => {
                if (pData.recId != i) {
                    tempTotal += parseFloat(pData.percentage);
                }

            });
        }
     
        var percentage = (parseFloat(val) / this.selectedProductDetails.value) * 100;
        var tempPercentage = tempTotal + percentage;
        if (tempPercentage > 100) {
            if (i == 0) {
                this.currentFundDetails.value = 0;
                this.currentFundDetails.percentage = 0;
            }
            else {
                var obj = $.grep(this.currentFunds, function (obj: any) {
                    return obj.recId == i;
                });
                obj[0].value = 0;
                obj[0].percentage = 0;
            }
            alert("Value cannot exceed total ");
            return;
        }
        else {

            if (i == 0) {

                this.currentFundDetails.percentage = percentage.toFixed(2);
            }
            else {
                var obj = $.grep(this.currentFunds, function (obj: any) {
                    return obj.recId == i;
                });
                obj[0].percentage = percentage.toFixed(2);
            }
        
        }
    }

  

    calculateValue(val: any, i: number) {
        var tempTotal = 0;
        if (i == 0) {
            this.currentFunds.forEach((pData: any) => {
                tempTotal += parseFloat(pData.value);
            });
        }
        else {
            this.currentFunds.forEach((pData: any) => {
                if (pData.recId != i) {
                    tempTotal += parseFloat(pData.value);
                }
                
            });
        }
        var value = (parseFloat(val) * this.selectedProductDetails.value) / 100;
        var tempValue = tempTotal + value;
        if (tempValue > this.selectedProductDetails.value) {
            if (i == 0) {
                this.currentFundDetails.value = 0;
                this.currentFundDetails.percentage = 0;
            }
            else {
                var obj = $.grep(this.currentFunds, function (obj: any) {
                    return obj.recId == i;
                });
                obj[0].value = 0;
                obj[0].percentage = 0;
            }
            alert("Value cannot exceed total ");
            return;
        }
        else {
            if (i == 0) {

                this.currentFundDetails.value = value.toFixed(2);
            }
            else {
                var obj = $.grep(this.currentFunds, function (obj: any) {
                    return obj.recId == i;
                });
                obj[0].value = value.toFixed(2);
            }
           

        }
    }

    setCurrentProduct(product: any) {
        
        this.backDropDisplay = 'block';
        this.fundDisplay = 'block';
        this.selectedProduct = product.recId;
        product.percentage = 100;
        this.selectedProductDetails = product;
        var obj = $.grep(this.products, function (obj: any) {
            return obj.productId == product.productId;
        });
        this.selectedProductDetails.product = obj[0].productName;

        this.portfolioService.getCurrentFunds(this.selectedProduct).subscribe(
            cf => {
                this.currentFunds = cf;
                this.currentFunds.forEach((pData: any, index: any) => {
                    pData.isEditable = false;

                });
                console.log(this.currentFunds);
            }
        );
      


        this.feeService.getSelectedFunds(product.productId).subscribe(
            fnds => {
               //var addNew = {
               //    apircode : "Add new fund"
               // };
               this.funds = fnds;
              // this.funds.unshift(addNew);
               this.newFundDefault = this.funds[1];

            }
        );
   
    }

    deleteCurrentFund(item: any, index: any) {
        var id$ = item.recId;
        this.portfolioFundService.delete(id$).subscribe((data) => {
            this.currentFunds.forEach((pData: any, index: any) => {
                if (pData.recId == id$) {
                    this.currentFunds.splice(index, 1);

                }

            });
        });
    }

    addcurrentFund(currentFund: any) {
        this.portfolioFundService.createFund(this.currentFundDetails, this.selectedProduct, this.totalPercentage).subscribe((data: any) => {
            data.isEditable = false;
            this.currentFunds.push(data);

            this.currentFundDetails = {
                apircode: "",
                value: 0,
                percentage: 0
            };
        });
    }

    editCurrentFund(currentFund: any) {
        this.portfolioFundService.createFund(currentFund, this.selectedProduct, this.totalPercentage).subscribe((data: any) => {
            var id$ = data.recId;
            if (this.currentFunds.length > 0) {
                this.currentFunds.forEach((pData: any, index: any) => {
                    if (pData.recId == id$) {
                        this.currentFunds.splice(index, 1, data);
                        //this.currentFunds.push(data);
                       
                    }

                });
            }

            this.currentFundDetails = {
                apircode: "",
                value: 0,
                percentage: 0
            };
        });
    }

    fundPercentageChange(val: any) {
        var tempTotal = 0;
        this.currentFunds.forEach((pData: any) => {
            tempTotal += parseFloat(pData.percentage);
        });
        this.totalPercentage = tempTotal + parseFloat(val);
        if (this.totalPercentage > 100) {
            this.totalPercentage = tempTotal;
            alert("Percentage cannot be greater than 100 ");
            return;
        }
        this.currentProducts.forEach((pData: any) => {
            if (pData.recId == this.selectedProduct) {
                pData.percentage = this.totalPercentage;
            }

        });
      
    }

    onCloseFund() {
        this.existingFund = 'block';
        this.newFund = 'none';
        this.investmentDetails = {
            apircode: ""
            , fundName: ""
            , amount: 0
            , buySpread: 0
            , icr: 0
            , domesticEquity: 0
            , internationalEquity: 0
            , domesticProperty: 0
            , internationalProperty: 0
            , growthAlternatives: 0
            , defensiveAlternatives: 0
            , domesticFixedInterest: 0
            , internationalFixedInterest: 0
            , domesticCash: 0  
            , internationalCash: 0
            , otherGrowth: 0
        };

    }

    updateFund(fund: any) {

        this.feeService.createFunds(this.investmentDetails).subscribe((data) => {

            this.funds.push(data);
            this.newFundDefault = data;
            var selectedInvestments: any[] = [];
            selectedInvestments.push(data);
            this.feeService.createProductFund(selectedInvestments, this.selectedProductDetails.productId,1).subscribe((data) => {
                this.investmentDetails = {
                    apircode: ""
                    , fundName: ""
                    , amount: 0
                    , buySpread: 0
                    , icr: 0
                    , domesticEquity: 0
                    , internationalEquity: 0
                    , domesticProperty: 0
                    , internationalProperty: 0
                    , growthAlternatives: 0
                    , defensiveAlternatives: 0
                    , domesticFixedInterest: 0
                    , internationalFixedInterest: 0
                    , domesticCash: 0
                    , internationalCash: 0
                    , otherGrowth: 0
                };
                this.existingFund = 'block';
                this.newFund = 'none';

            });
            

            
           
        });

       
      
    }
   
}
