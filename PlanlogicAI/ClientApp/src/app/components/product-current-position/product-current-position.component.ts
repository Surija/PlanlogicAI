import { Component, OnInit, NgZone } from '@angular/core';
import { ProductService } from './../../services/product.service';
import { FeeService } from './../../services/fee.service';
import { InvestmentFundService } from './../../services/investment-fund.service';

import * as $ from 'jquery';
import * as XLSX from 'xlsx';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { DragulaModule, DragulaService } from 'ng2-dragula';

@Component({
  selector: 'app-product-current-position',
  templateUrl: './product-current-position.component.html',
  styleUrls: ['./product-current-position.component.css']
})
export class ProductCurrentPositionComponent implements OnInit {

    SelectedFileForUpload = null;
    Message: string = "";
    platforms: any[] = [];
    backDropDisplay = 'none';
    productDisplay = 'none';
    fundDisplay = 'none';
    pf: any = {
        platformId: 0,   
        platformName: ''
       
        
    };

    products: any = [];
    productDetails: any = {
        productId: 0,
        productName: "",
        productType: ""
        
    };

    fees: any = [];
    feeDetails: any = {
        feeId: 0,
        costType: "ongoing",
        feeName: "",
        feeType: "F",
        amount: 0
    };

    allInvestments: any[] = [];
    selectedInvestments: any[] = [];
    investmentDetails: any = {
        apircode : ""
        , fundName: ""
        , amount : 0
        , buySpread : 0
        , icr : 0
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
      , isSingle: ""
      , investorProfile: ""
       ,isActive : ""
    };

    costType = [{ name: "Ongoing Costs", val: "ongoing" }, { name: "Transactional Costs", val: "transactional" }];
    feeType = [{ name: "Fixed", val: "F" }, { name: "Percentage", val: "P" }];

    selectedPlatform: number = 0;
    selectedProduct: number = 0;
    selectedProductName: string = "";
    isEdit: boolean = false;

    constructor(private route: ActivatedRoute, private dragulaService: DragulaService,
        private router: Router, private zone: NgZone, private productService: ProductService, private feeService: FeeService, private fundService: InvestmentFundService) {


        dragulaService.dropModel.subscribe((value : any) => {
            this.onDropModel(value.slice(1));
        });

        dragulaService.removeModel.subscribe((value: any) => {
            this.onRemoveModel(value.slice(1));
        });
         
    }
    private onDropModel(args: any) {
        let [el, target, source] = args;    
    }

    private onRemoveModel(args: any) {
        let [el, source] = args;
    }

    //private UploadFile(event: any) {      
    //    this.SelectedFileForUpload = event.target.files[0];
    //    this.Message = "";
    //}

    //private ParseExcelDataAndSave() {
    //    var file = this.SelectedFileForUpload;
       
    //    if (file) {
    //        var reader = new FileReader();
    //        reader.onload = (e: any) => {
    //            var data = e.target.result;
    //            //XLSX from js-xlsx library , which I will add in page view page
    //            var workbook = XLSX.read(data, { type: 'binary' });
    //            var sheetName = workbook.SheetNames[0];
    //            var excelData = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName]);
    //            if (excelData.length > 0) {
    //                this.SaveData(excelData);
    //          }
    //            else {
                    
    //                //  = "No data found";
    //            }

    //        }
    //        reader.onerror = function (ex) {
    //            console.log(ex);
    //        }

           

    //        reader.readAsBinaryString(file);
    //    }
    //}

    //public SaveData(excelData: any) {
    //    this.productService.create(excelData, 1).subscribe((data: any) => {

    //    });
    //}
    Search(val: number) {
        if (val == 1) {
            $(".selected").hide();
            var t = String($("#searchSelected").val());
            $('.selected').each(function () {
                if ($(this).text().toUpperCase().indexOf(t.toUpperCase()) != -1) {
                    $(this).show();
                }
            });
        }
        else if (val == 0)  {
            $(".existing").hide();
            var t = String($("#searchExisting").val());
            $('.existing').each(function () {
                if ($(this).text().toUpperCase().indexOf(t.toUpperCase()) != -1) {
                    $(this).show();
                }
            });
        }
    }

    ngOnInit() {

        $(document).ready(function () {
            $('#sub').hide();
        });
       
            
     
          var sources = [];
          sources.push(this.productService.getPlatforms()
            );
        
          Observable.forkJoin(sources).subscribe((data: any) => {
                var platform = data[0];
            
                platform.forEach((x: any) => {                
                      var obj: any = {};
                      obj["platformName"] = x.platformName;
                      obj["platformId"] = x.platformId;                
                      this.platforms.push(obj);                
                });
              //  this.allInvestments = investment;
            }, err => {
                  if (err.status == 404)
                     this.router.navigate(['/home']);
            });

  }



  saveProductFund() {
      this.fundService.createProductFund(this.selectedInvestments, this.selectedProduct,0).subscribe((data) => {


      });
  }

  changePlatform(platformId: any) {
      if (platformId == "0") {
          this.selectedProduct = 0;
          this.selectedProductName = "";
          this.backDropDisplay = 'block';
          this.productDisplay = 'block';
      }
      else {
          this.selectedProduct = 0;
          this.selectedProductName = "";
          this.selectedPlatform = platformId;
          this.productService.getProductDetails(platformId).subscribe(
              productDetails => {
                  this.products = productDetails;
              }
          );
      }
  }


  showNewFund() {
      this.isEdit = false;
      this.backDropDisplay = 'block';
      this.fundDisplay = 'block';
  }

  updateFund(fund: any) {
     
      this.fundService.createFunds(this.investmentDetails).subscribe((data: any) => { 

          var exist = $.grep(this.allInvestments, function (obj: any) {
              return obj.apircode == data.apircode;
          });
          var exist1 = $.grep(this.selectedInvestments, function (obj: any) {
              return obj.apircode == data.apircode;
          });
          if (exist.length === 0 && exist1.length === 0) {
              this.allInvestments.push(data);
          }
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
            , isSingle: ""
            , investorProfile: ""
            , isActive: ""
          };

      });
      this.onCloseFund();
  }

  //Platform
  updatePlatform(pf: any) {
      this.productService.create(this.pf).subscribe((data: any) => {
          var id$ = data.platformId;
          this.platforms.push(data); 

          this.pf = {
              platformId: 0,
              platformName: '',
              isEditable: false

          };
       });
      this.onClosePlatform();
  }

  public onClick(fund: any): void {

  
      this.isEdit = true;
      this.investmentDetails = fund;
      this.backDropDisplay = 'block';
      this.fundDisplay = 'block';
  }

  onClosePlatform() {
      this.isEdit = false;
      this.backDropDisplay = 'none';
      this.productDisplay = 'none';
      this.pf = {
        platformId: 0,   
        platformName: ''
      };
  }

  onCloseFund() {
      this.isEdit = false;
      this.backDropDisplay = 'none';
      this.fundDisplay = 'none';
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
        , isSingle: ""
        , investorProfile: ""
        , isActive: ""
      };

  }

  //Product
  deleteProduct(item: any, index: any) {
      var id$ = item.productId;
      this.productService.delete(id$, item.platformId).subscribe((data: any) => {
          this.products.forEach((pData: any, index: any) => {
              if (pData.productId == id$) {
                  this.products.splice(index, 1);
               
              }

          });
         });
      }
  
  addProduct(product: any) {
      this.productService.createProducts(product, this.selectedPlatform).subscribe((data: any) => {
          var id$ = data.productId;
          this.products.push(data);

          this.productDetails = {
              productId: 0,
              productName: "",
              productType: ""
             
          };
      }); 
  }

  editProduct(product: any) {
      this.productService.createProducts(product, this.selectedPlatform).subscribe((data: any) => {
          var id$ = data.productId;
          if (this.products.length > 0) {
              this.products.forEach((pData: any, index: any) => {
                  if (pData.productId == id$) {
                      this.products.splice(index, 1, data);
                     
                  }

              });
          }
         
          this.productDetails = {
              productId: 0,
              productName: "",
              productType: ""
              
          };
      });
  }

  goBack() {
      $("#heading").show();
      $('#sub').hide();
      this.selectedProduct = 0;
      this.selectedProductName = "";
  }

  setProduct(product: any) {

      $("#heading").hide();
      $('#sub').show();
      this.selectedProductName = product.productName;
      this.selectedProduct = product.productId;
      var productSources = [];
      productSources.push(
          this.fundService.getSelectedFunds(product.productId),
          this.fundService.getAllInvestments(product.productId)
        
      );

      Observable.forkJoin(productSources).subscribe((data: any) => {
       //   this.fees = data[0];
          this.selectedInvestments = data[0];
          var allFunds = data[1];

          this.selectedInvestments.forEach((y: any) => {
              
                  var index = allFunds.map(function (x: any) { return x.apircode }).indexOf(y.apircode);
                  allFunds.splice(index, 1);
             
          });

          this.allInvestments = allFunds;
          //  this.allInvestments = investment;
      }, err => {
          if (err.status == 404)
              this.router.navigate(['/home']);
      });
      //this.feeService.getFeeDetails(product.productId).subscribe(
      //    feeDetails => {
      //        this.fees = feeDetails;
      //    }
      //);

      //this.fundService.getAllInvestments(product.productId).subscribe(
      //    existingInvestments => {
      //        this.allInvestments = existingInvestments;
      //    }
      //);

      //this.fundService.getSelectedFunds(product.productId).subscribe(
      //    selectedInvestments => {
      //        this.selectedInvestments = selectedInvestments;
      //    }
      //);
  }

  //Fees
  //deleteFee(item: any, index: any) {
  //    var id$ = item.feeId;
  //    this.feeService.delete(id$, item.productId).subscribe((data: any) => {
  //        this.fees.forEach((fData: any, index: any) => {
  //            if (fData.feeId == id$) {
  //                this.fees.splice(index, 1);

  //            }

  //        });
  //    });
  //}

  //addFee(fee: any) {
  //    this.feeService.createFees(this.feeDetails, this.selectedProduct).subscribe((data: any) => {
  //        var id$ = data.feeId;
  //        this.fees.push(data);

  //        this.feeDetails = {
  //            feeId: 0,
  //            costType: "ongoing",
  //            feeName: "",
  //            feeType: "F",
  //            amount: 0
  //        };
  //    });
  //}

  //editFee(fee: any) {
  //    this.feeService.createFees(fee, this.selectedProduct).subscribe((data: any) => {
  //        var id$ = data.feeId;
  //        if (this.fees.length > 0) {
  //            this.fees.forEach((fData: any, index: any) => {
  //                if (fData.feeId == id$) {
  //                    this.fees.splice(index, 1, data);                     
  //                }

  //            });
  //        }

  //        this.feeDetails = {
  //            feeId: 0,
  //            costType: "ongoing",
  //            feeName: "",
  //            feeType: "F",
  //            amount: 0
  //        };
  //    });
  //}
 
}
