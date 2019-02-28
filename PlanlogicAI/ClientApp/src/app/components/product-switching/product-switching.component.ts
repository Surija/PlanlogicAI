import { Component, OnInit, NgZone } from '@angular/core';
import { ProductService } from './../../services/product.service';
import { FeeService } from './../../services/fee.service';
import { LifestyleAssetService } from './../../services/lifestyleAsset.service';
import { CashFlowService } from './../../services/cashFlow.service';
import { LiabilityService } from './../../services/liability.service';
import { InvestmentFundService } from './../../services/investment-fund.service';
import { ProductSwitchingService } from './../../services/product-switching.service';
import { CurrentPortfolioService } from './../../services/current-portfolio.service';
import { CurrentPortfolioFundService } from './../../services/current-portfolio-fund.service';
import { NewProductService } from './../../services/new-product.service';
import { AlternativeProductService } from './../../services/alternative-product.service';
import { ROPCurrentService } from './../../services/rop-current.service'
import { NgxSelectModule, INgxSelectOption } from 'ngx-select-ex'
import * as $ from 'jquery';
import * as XLSX from 'xlsx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import { ActivatedRoute, Router } from '@angular/router';
import { DragulaModule, DragulaService } from 'ng2-dragula';
import { async } from '@angular/core/testing';
import { BasicDetails } from '../../models/Client';
import { ClientService } from './../../services/client.service';
import { forEach } from '@angular/router/src/utils/collection';
import { saveAs } from 'file-saver';
import * as _ from 'underscore';
import { MatTabChangeEvent, MatSelectChange, MatAutocompleteSelectedEvent} from '@angular/material';
import { FormControl } from '@angular/forms';
import { map, startWith } from 'rxjs/operators';


@Component({
  selector: 'app-product-switching',
  templateUrl: './product-switching.component.html',
  styleUrls: ['./product-switching.component.css']
})


export class ProductSwitchingComponent implements OnInit {

  myControl = new FormControl();
  filteredOptions: Observable<any[]>;
  options: string[] = ['One', 'Two', 'Three'];

  showLoadingIndicator = false;
  sub: any;
  SelectedFileForUpload = null;
  Message: string = "";
  platforms: any[] = [];
  currentProductModal = 'none';
  //rollFundsInModal = 'none';
  productDisplay = 'none';
  fundDisplay = 'none';
  newfundDisplay = 'none';
  riskProfile = 'none';
  generateWord = 'none';
  pf: any = {
    platformId: 0,
    platformName: ''
  };

  platformDetails: any = [];

  currentAltFund: any = [];
  productFees: any = [];
  productFeeDetails: any = [];
  individualProductFeeDetails: any = [];
  transactional: any = [];
  ongoing: any = [];
  products: any = [];
  productDetails: any = {
    productId: 0,
    productName: "",
    productType: "",
    owner: "Client",
    value: 0
  };

  currentSelectedProduct: any = {};
  currentSelectedType: any = ''

  alternativeDetails: any = {
    productId: 0,
    productName: "",
    productType: "",
    owner: "Client",
    value: 0
  };

  ropDetails: any = {
    productId: 0,
    productName: "",
    productType: "",
    owner: "Client",
    value: 0
  };

  fees: any = [];
  feeDetails: any = {
    feeId: 0,
    feeType: "",
    amount: 0
  };

  proposedFundDetails: any = {
    apircode: "",
    value: 0,
    percentage: 0
  };

  alternativeFundDetails: any = {
    apircode: "",
    value: 0,
    percentage: 0
  };
  ropFundDetails: any = {
    apircode: "",
    value: 0,
    percentage: 0
  };
  investmentDetails: any = {
    apircode: ""
    , mId: ""
    , fundName: ""
    , total: 0
    , totalGrowth: 0
    , totalDefensive: 0
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
    , subType: ""
    , isActive: ""
  };

  mainDisplay = 'none';
  previewDisplay = 'none';

  clientDetails: any;
  selectedClient: any;
  currentInvestments: any[] = [];
  filteredInvestments: any[] = [];
  selectedInvestments: any[] = [];
  allProducts: any[] = [];
  alternativeProductsList: any[] = [];
  proposedInvestments: any[] = [];
  allProposedInvestments: any[] = [];
  allAlternativeProducts: any[] = [];
  riskProfiles: any[] = [];
  riskProfileSummary: any[] = [];
  riskProfileMatchSummary: any[] = [];
  proposedProduct: any[] = [];
  dropOptionMain = 'none';
  rebalanceOption = 'none';
  selectedProduct: number = 0;
  selectedcurrentProduct: number = 0;
  alternativeProduct: number = 0;
  ropCurrent: number = 0;
  selectedClientProduct: number = 0;
  selectedProductDetails: any = {};
  currentProductDetails: any = {
    productId: 0,
    value: 0,
    percentage: 100,
    owner: 'Client',
    sub: 'F'
  };
  alternativeProductDetails: any = {};
  ropCurrentDetails: any = {};
  currentFund: any[] = [];
  proposedFund: any = [];
  alternativeFund: any[] = [];
  alternativeProducts: any[] = [];
  funds: any = [];
  originalFunds: any = [];
  filteredFunds: any = [];
  alFunds: any = [];
  ropFunds: any = [];
  totalPercentage: number = 0;
  totalCurrentPercentage: number = 0;
  totalCurrentInvestments: number = 0;
  totalProposedInvestments: number = 0;
  productReplacement: any[] = [];
  alternativeReplacement: any[] = [];
  ropCurrentReplacement: any[] = [];
  owner: any = [];
  allProductReplacement: any[] = [];
  ngxValue: any = [];

  newProductLinks: any = [];
  newProductReplacement: any = {
    product: '',
    value: 0,
  };
  rollFundsLinks: any = [];
  rollFundsIn: any = {
    product: '',
    value: 0,
  };

  sel: number = 0;
  isEqual: boolean = true;
  unutilizedVal: number = 0;
  allFunds: any[] = [];
  fundDetails: any = {
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

  proposedAssetAllocation: any = {};
  allCurrentFund: any[] = [];
  allProposedFund: any[] = [];
  totalAlternativeProducts: any[] = [];
  cashFlowIncome: any = [];
  cashFlowExpenses: any = [];
  lifestyleAssets: any = [];
  liabilities: any = [];

  clientCurrentFund: any[] = [];
  partnerCurrentFund: any[] = [];
  jointCurrentFund: any[] = [];
  clientCurrentOriginalFund: any[] = [];
  partnerCurrentOriginalFund: any[] = [];
  jointCurrentOriginalFund: any[] = [];
  clientProposedFund: any[] = [];
  partnerProposedFund: any[] = [];
  jointProposedFund: any[] = [];
  commonClient: any[] = [];
  totalClient: any[] = [];
  commonPartner: any[] = [];
  totalPartner: any[] = [];
  commonJoint: any[] = [];
  totalJoint: any[] = [];
  document: any[] = [];

  clientProductReplacement: any[] = [];
  partnerProductReplacement: any[] = [];
  jointProductReplacement: any[] = [];

  clientInvestmentProductReplacement: any[] = [];
  partnerInvestmentProductReplacement: any[] = [];
  jointInvestmentProductReplacement: any[] = [];

  clientOverallReplacement: any[] = [];

  clientcurrentFunds: any = [];
  clientcurrentFundDetails: any = {
    apircode: "",
    value: 0,
    percentage: 0,
    actualValue: 0,
    actualPercentage: 0
  };




  constructor(private route: ActivatedRoute, private dragulaService: DragulaService, private liabilityService: LiabilityService,
    private lifestyleAssetService: LifestyleAssetService, private router: Router, private zone: NgZone, private productService: ProductService, private feeService: FeeService, private fundService: InvestmentFundService, private productSwitchingService: ProductSwitchingService, private portfolioService: CurrentPortfolioService, private portfolioFundService: CurrentPortfolioFundService, private newProductService: NewProductService, private alternativeProductService: AlternativeProductService, private ropCurrentService: ROPCurrentService, private clientService: ClientService, private cashFlowService: CashFlowService) {

    dragulaService.setOptions('first-bag', {


      revertOnSpill: true,
      copy: (el: Element, source: Element): boolean => {
        return source.id === 'sourceDiv';
      },
      accepts: (el: Element, target: Element, source: Element, sibling: Element): boolean => {

        var isExist = true;
        var exist = false;

        if (exist == false) {
          isExist = false
        }
        return isExist == false;
      },
      moves: (el: Element, source: Element, handle: Element, sibling: Element) => !el.classList.contains('no-drag')
    });


    dragulaService.dropModel.subscribe((value: any) => {
      this.onDropModel(value.slice(1));
    });

    //dragulaService.removeModel.subscribe((value: any) => {
    //    this.onRemoveModel(value.slice(1));
    //});

  }


  private onDropModel(args: any) {

    let [el, target, source] = args;

    this.productSwitchingService.getAllProposedProducts().subscribe((data: any) => {
      this.allProposedInvestments = data;
      if (this.allProposedInvestments.length > 0) {
        var t = Math.max.apply(Math, this.allProposedInvestments.map(function (o) { return o.recId; }))
        this.selectedProduct = t + 1;
      }
      else {
        this.selectedProduct = 1;
      }

      var exist = $.grep(this.proposedInvestments, function (obj: any) {
        return obj.recId == el.id;
      });
      exist[0].recId = this.selectedProduct;


      this.retainProduct();

    });


    this.selectedClientProduct = el.id;

    var obj = $.grep(this.currentInvestments, function (obj: any) {
      return obj.recId == el.id;
    });
    this.selectedProductDetails = obj[0];
  }

  ngOnInit() {





    //this.myControl.valueChanges
    // .pipe(
    //   startWith(''),
    //  map(value => this._filterAlternative(value))
    //);

    $(document).ready(function () {
      //(<any>$('[data-toggle="popover"]')).popover({ html: true });


    });

    $(document).on('click', function (e) {
      //$('[data-toggle="popover"],[data-original-title]').each(function () {
      //    if (!$(this).is(<any>(e.target)) && $(this).has(<any>(e.target)).length === 0 && $('.popover').has(<any>(e.target)).length === 0) {
      //        (((<any>$(this)).popover('hide').data('bs.popover') || {}).inState || {}).click = false
      //    }
      //});
    })

    this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
    this.clientDetails = JSON.parse(localStorage.getItem('ClientDetails') || '{}');


    if (this.clientDetails.maritalStatus == "S") {
      this.owner = ["Client"];
    } else {
      this.owner = ["Client", "Partner", "Joint"];
    }

    var selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');

    this.sub = this.route.params.subscribe(params => {
      var val = params['id'];
      if (val == "main") {
        var sources = [];

        sources.push(
          this.productSwitchingService.getClientCurrentProducts(this.selectedClient),
          this.productSwitchingService.getClientProposedProducts(this.selectedClient),
          this.portfolioService.getProducts(),
          this.fundService.getAllInvestments(0),
          this.productSwitchingService.getRiskProfiles(),
          this.cashFlowService.getCashFlows(this.selectedClient, "I"),
          this.cashFlowService.getCashFlows(this.selectedClient, "E"),
          this.lifestyleAssetService.getLifestyleAssets(this.selectedClient),
          this.liabilityService.getLiabilities(this.selectedClient),
          this.portfolioFundService.getAllProductReplacement(),
          this.feeService.getPlatformDetails(this.selectedClient)

        );

        Observable.forkJoin(sources).subscribe((data: any) => {
          this.currentInvestments = data[0];
          this.proposedInvestments = data[1];
          this.allProducts = data[2];
          this.selectedInvestments = data[0];
          this.allFunds = data[3];
          this.riskProfiles = data[4];
          this.cashFlowIncome = data[5];
          this.cashFlowExpenses = data[6];
          this.lifestyleAssets = data[7];
          this.liabilities = data[8];
          this.allProductReplacement = data[9];
          this.platformDetails = data[10];

          this.selectedInvestments.forEach(function (obj) {
            obj.checked = false;
          });

          var Currenttotal = 0;
          this.currentInvestments.forEach(function (obj) {
            Currenttotal += obj.value;
          });

          this.totalCurrentInvestments = Currenttotal;

          var Proposedtotal = 0;
          this.proposedInvestments.forEach(function (obj) {
            Proposedtotal += obj.value;
          });

          this.totalProposedInvestments = Proposedtotal;

        }, err => {
          if (err.status == 404)
            this.router.navigate(['/home']);
        });

        this.mainDisplay = 'block';
        this.previewDisplay = 'none';
      }
      else {
        this.showLoadingIndicator = true;
        this.generateWord = "none";
        this.onCloseRebalance();
        var sources = [];

        sources.push(
          this.productSwitchingService.getClientCurrentProducts(this.selectedClient),
          this.productSwitchingService.getClientProposedProducts(this.selectedClient),
          this.portfolioService.getProducts(),
          this.fundService.getAllInvestments(0),
          this.productSwitchingService.getRiskProfiles(),
          this.cashFlowService.getCashFlows(this.selectedClient, "I"),
          this.cashFlowService.getCashFlows(this.selectedClient, "E"),
          this.lifestyleAssetService.getLifestyleAssets(this.selectedClient),
          this.liabilityService.getLiabilities(this.selectedClient),
          this.portfolioFundService.getAllProductReplacement(),
          this.feeService.getPlatformDetails(this.selectedClient)


        );

        Observable.forkJoin(sources).subscribe((data: any) => {
          this.currentInvestments = data[0];
          this.proposedInvestments = data[1];
          this.allProducts = data[2];
          this.selectedInvestments = data[0];
          this.allFunds = data[3];
          this.riskProfiles = data[4];
          this.cashFlowIncome = data[5];
          this.cashFlowExpenses = data[6];
          this.lifestyleAssets = data[7];
          this.liabilities = data[8];
          this.allProductReplacement = data[9];
          this.platformDetails = data[10];
          this.calculateAA();


        }, err => {
          if (err.status == 404)
            this.router.navigate(['/home']);
        });

        this.previewDisplay = 'block';
        this.mainDisplay = 'none';

      }
      window.scroll(0, 0);
    });

  }

  ngOnDestroy() {
    if (this.dragulaService.find('first-bag') !== undefined) {
      this.dragulaService.destroy('first-bag');
    }
  }

  onSearchChange(searchValue: string) {
    //alert(searchValue);
  }

  private _filterAlternative(value: string, id: number) {
    if (value != null) {
      const filterValue = value.toLowerCase();
      var product = $.grep(this.alternativeProducts, function (obj: any) {
        return obj.id == id;
      });
      if (product.length > 0) {
        this.filteredOptions = product[0].alFunds.filter(option => (option.apircode.toLowerCase() + option.fundName.toLowerCase()).includes(filterValue));
        product[0].filteredFund = this.filteredOptions;
      }

    }
  }
  private _filterCurrent(value: string, id: number) {
    if (value != null) {
      const filterValue = value.toLowerCase();
      var product = $.grep(this.currentFund, function (obj: any) {
        return obj.id == id;
      });
      if (product.length > 0) {
        this.filteredOptions = product[0].ropFunds.filter(option => (option.apircode.toLowerCase() + option.fundName.toLowerCase()).includes(filterValue));
        product[0].filteredFund = this.filteredOptions;
      }

    }
  }

  private _filterProposed(value: string, id: number) {
    if (value != null) {
      const filterValue = value.toLowerCase();
      var product = $.grep(this.proposedProduct, function (obj: any) {
        return obj.id == id;
      });
      if (product.length > 0) {
        this.filteredOptions = this.originalFunds.filter(option => (option.apircode.toLowerCase() + option.fundName.toLowerCase()).includes(filterValue));
        this.funds = this.filteredOptions;
      }

    }
  }

  private _filterExisting(value: string, id: number) {
    if (value != null) {
      const filterValue = value.toLowerCase();
      this.filteredOptions = this.originalFunds.filter(option => (option.apircode.toLowerCase() + option.fundName.toLowerCase()).includes(filterValue));
      this.funds = this.filteredOptions;
    }
  }

  openNewProductModal() {

    this.rebalanceOption = 'block';
    this.dropOptionMain = 'block';
    this.selectedProductDetails = {
      productId: 0,
      productName: "",
      productType: "",
      owner: "Client",
      value: 0,
      isEditable: true
    };
    this.selectedProduct = 0;
    this.proposedFund = [];
    this.rebalanceProduct();
  }
  openCurrentModal() {
    this.currentProductModal = 'block';
    this.dropOptionMain = 'block';
  }
  //Close Functions
  onCloseCurrentProduct() {
    this.currentProductModal = 'none';
    this.dropOptionMain = 'none';
    this.currentProductDetails = {
      productId: 0,
      value: 0,
      percentage: 100,
      owner: 'Client'
    };
    this.selectedcurrentProduct = 0;
    this.clientcurrentFundDetails = {
      apircode: "",
      value: 0,
      percentage: 0
    };
  }
  onCloseNewProduct() {
    this.dropOptionMain = 'none';
    $('#replacement').hide();
    $('#btnAddNewFund').hide();
    $('#btnBackNewProduct').hide();
    $('#newProduct').hide();
    $('#btnNewReplacement').hide();
    this.ngxValue = [];
    this.productDetails = {
      productId: 0,
      productName: "",
      productType: "",
      owner: "Client",
      value: 0
    };
    this.newProductReplacement = {
      product: '',
      value: 0,
    };

    if (this.newProductLinks.length > 0) {
      this.newProductLinks.forEach((productLink: any) => {
        this.currentInvestments.forEach((data1: any, index: any) => {
          if (data1.recId == productLink.product) {
            data1.unutilizedValue += productLink.value;
            this.portfolioService.createProduct(data1, this.selectedClient, "Client").subscribe((data) => {
              this.currentInvestments.splice(index, 1);
              this.currentInvestments.splice(index, 0, data);

              this.setFilteredInvestment();

            });
          }
        });
      });


    }
    this.newProductLinks = [];
  }
  onCloseRebalance() {
    this.currentFund = [];
    this.proposedProduct = [];
    this.alternativeProducts = [];
    this.allCurrentFund = [];
    this.allProposedFund = [];
    this.totalAlternativeProducts = [];
    this.commonClient = [];
    this.totalClient = [];
    this.commonPartner = [];
    this.totalPartner = [];
    this.commonJoint = [];
    this.totalJoint = [];
    this.productFeeDetails = [];
    this.individualProductFeeDetails = [];
    this.dropOptionMain = 'none';
    this.rebalanceOption = 'none';
    this.selectedProduct = 0;
    this.selectedProductDetails = {
      productId: 0,
      productName: "",
      productType: "",
      owner: "Client",
      value: 0
    };
    this.alternativeProduct = 0;
    this.ropCurrent = 0;
    this.selectedClientProduct = 0;
    this.alternativeFundDetails = {
      apircode: "",
      value: 0,
      percentage: 0
    };
    this.ropFundDetails = {
      apircode: "",
      value: 0,
      percentage: 0
    };
    this.clientProductReplacement = [];
    this.partnerProductReplacement = [];
    this.jointProductReplacement = [];

    this.clientInvestmentProductReplacement = [];
    this.partnerInvestmentProductReplacement = [];
    this.jointInvestmentProductReplacement = [];

    this.clientOverallReplacement = [];

    for (var i = 0; i <= 3; i++) {

      if ($("#productTabs li").eq(i).hasClass('active')) {
        $("#productTabs li").eq(i).removeClass('active')
      }

      if ($(".swap").eq(i).hasClass('active')) {
        $(".swap").eq(i).removeClass('active')
      }

      if ($(".swap").eq(i).hasClass('in')) {
        $(".swap").eq(i).removeClass('in')
      }
    }


  }
  public onCloseNewFundDisplay() {
    this.newfundDisplay = 'none';

    if (this.currentSelectedType == "CP") {
      this.currentProductModal = 'block';
    }
    else {
      this.rebalanceOption = 'block';
    }

    this.investmentDetails = {
      apircode: ""
      , mId: ""
      , fundName: ""
      , total: 0
      , totalGrowth: 0
      , totalDefensive: 0
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
      , subType: ""
      , isActive: ""
    };
    this.currentSelectedProduct = {};
    this.currentSelectedType = "";
  }

  public onCloseFundDisplay() {
    this.fundDisplay = 'none';
    this.rebalanceOption = 'block';
    this.investmentDetails = {
      apircode: ""
      , mId: ""
      , fundName: ""
      , total: 0
      , totalGrowth: 0
      , totalDefensive: 0
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
      , subType: ""
      , isActive: ""
    };
  }
  public onCloseRiskProfile() {
    this.riskProfile = 'none';
    this.rebalanceOption = 'block';
    this.riskProfileMatchSummary = [];
  }
  public onCloseDropOption() {
    this.proposedInvestments.pop();
    this.dropOptionMain = 'none';
  }
  showNewPropductPP() {
    $('#replacement').show();
    $('#btnAddNewFund').show();
    $('#btnBackNewProduct').show();
    $('#btnAddNewFund').prop('disabled', true);
    $('#newProduct').hide();
    $('#btnNewReplacement').hide();
    this.setFilteredInvestment();
  }
  backToNewProduct() {

    $('#replacement').hide();
    $('#btnAddNewFund').hide();
    $('#btnBackNewProduct').hide();
    $('#newProduct').show();
    $('#btnNewReplacement').show();

    this.newProductReplacement = {
      product: '',
      value: 0,
    };

    if (this.newProductLinks.length > 0) {
      this.newProductLinks.forEach((productLink: any) => {
        this.currentInvestments.forEach((data1: any, index: any) => {
          if (data1.recId == productLink.product) {
            data1.unutilizedValue += productLink.value;
            this.portfolioService.createProduct(data1, this.selectedClient, "Client").subscribe((data) => {
              this.currentInvestments.splice(index, 1);
              this.currentInvestments.splice(index, 0, data);

              this.setFilteredInvestment();

            });
          }
        });
      });


    }



    this.newProductLinks = [];
  }
  retainProduct() {

    var selectedProd = this.selectedProduct;
    var obj = $.grep(this.proposedInvestments, function (obj: any) {
      return obj.recId == selectedProd;
    });

    this.ropCurrentService.getAllROPCurrent().subscribe((cData: any) => {
      var id = 0;
      if (cData.length > 0) {
        var t = Math.max.apply(Math, cData.map(function (o: any) { return o.recId; }))
        id = t + 1;
      }
      else {
        id = 1;
      }

      obj[0].currentId = id;
      obj[0].isEqual = 1;
      this.productSwitchingService.retainProduct(obj[0], this.selectedClientProduct).subscribe((data: any) => {

        this.setProposedInvestment(data.recId);
        this.selectedProduct = data.recId;
        this.selectedProductDetails = data;

        this.calculateFee(data.recId, "P", data.productId);
        this.calculateFee(data.ropCurrentId, "C", data.productId);

      });
    });


  }

  addFieldValue(newProductReplacement: any) {

    this.currentInvestments.forEach((data1: any, index: any) => {
      if (data1.recId == newProductReplacement.product) {
        if (newProductReplacement.value <= data1.unutilizedValue) {
          var exist = false;
          this.newProductLinks.forEach((data1: any, index: any) => {
            if (data1.product == newProductReplacement.product) {
              this.newProductLinks.splice(index, 1);
              this.newProductLinks.splice(index, 0, newProductReplacement);
              exist = true;
            }

          });

          var total = 0;
          if (exist == false) {
            this.newProductLinks.push(newProductReplacement);
            this.newProductLinks.forEach((data1: any, index: any) => {
              total += data1.value;

            });

            if (total == this.productDetails.value) {
              $('#btnAddNewFund').prop('disabled', false);
            }
            else {
              $('#btnAddNewFund').prop('disabled', true);
            }
          }



          data1.unutilizedValue -= newProductReplacement.value;
          //this.portfolioService.createProduct(data1, this.selectedClient, "Client").subscribe((data) => {

          //    var exist = false;
          //    var id$ = data.recId;

          //    this.currentInvestments.forEach((data1: any, index: any) => {
          //        if (data1.recId == id$) {
          this.currentInvestments.splice(index, 1);
          this.currentInvestments.splice(index, 0, data1);

          this.setFilteredInvestment();
          //            exist = true;
          //        }

          //    });

          //    if (exist == false) {
          //        this.currentInvestments.push(data);
          //        this.setFilteredInvestment();
          //    }
          //});



        }
        else {
          alert("value is greater");
        }
      }



    });


    this.newProductReplacement = {
      product: '',
      value: 0,
    };





  }

  addRollFundsIn(rollFundsIn: any) {

    this.currentInvestments.forEach((data1: any, index: any) => {
      if (data1.recId == rollFundsIn.product) {
        if (rollFundsIn.value <= data1.unutilizedValue) {
          var exist = false;
          this.rollFundsLinks.forEach((data1: any, index: any) => {
            if (data1.product == rollFundsIn.product) {
              this.rollFundsLinks.splice(index, 1);
              this.rollFundsLinks.splice(index, 0, rollFundsIn);
              exist = true;
            }
          });
          if (exist == false) {
            this.rollFundsLinks.push(rollFundsIn);
          }

          data1.unutilizedValue -= rollFundsIn.value;
          this.currentInvestments.splice(index, 1);
          this.currentInvestments.splice(index, 0, data1);
          this.setFilteredInvestment();

          //this.portfolioService.createProduct(data1, this.selectedClient, "Client").subscribe((data) => {

          //    var exist = false;
          //    var id$ = data1.recId;

          //    this.currentInvestments.forEach((data2: any, index: any) => {
          //        if (data2.recId == id$) {
          //            this.currentInvestments.splice(index, 1);
          //            this.currentInvestments.splice(index, 0, data1);

          //            var filtered = $.grep(this.currentInvestments, function (obj: any) {
          //                return obj.unutilizedValue > 0;
          //            });
          //            this.filteredInvestments = filtered;
          //            exist = true;
          //        }

          //    });

          //    if (exist == false) {
          //        this.currentInvestments.push(data1);
          //        var filtered = $.grep(this.currentInvestments, function (obj: any) {
          //            return obj.unutilizedValue > 0;
          //        });
          //        this.filteredInvestments = filtered;
          //    }
          //});



        }
        else {
          alert("value is greater");
        }
      }



    });


    this.rollFundsIn = {
      product: '',
      value: 0,
    };





  }

  deleteFieldValue(item: any, index: any, isNew: number) {

    if (isNew == 1) {
      if (this.newProductLinks.length > 0) {
        this.newProductLinks.splice(index, 1);
        this.currentInvestments.forEach((data1: any, index: any) => {
          if (data1.recId == item.product) {
            data1.unutilizedValue += item.value;
            this.portfolioService.createProduct(data1, this.selectedClient, "Client").subscribe((data) => {
              this.currentInvestments.splice(index, 1);
              this.currentInvestments.splice(index, 0, data);

              this.setFilteredInvestment();

            });
          }
        });


      }
    }
    else {
      if (this.rollFundsLinks.length > 0) {
        this.rollFundsLinks.splice(index, 1);
        this.currentInvestments.forEach((data1: any, index: any) => {
          if (data1.recId == item.product) {
            data1.unutilizedValue += item.value;
            this.currentInvestments.splice(index, 1);
            this.currentInvestments.splice(index, 0, data1);
            this.setFilteredInvestment();
          }
        });


      }

    }
  }

  changeFieldValue(newProduct: any, isNew: number) {

    if (isNew == 1) {
      this.newProductLinks.forEach((data1: any, index: any) => {
        if (data1.product == newProduct) {
          alert("please select a new product");
          this.newProductReplacement = {
            product: '',
            value: 0,
          };
          // $('#addNewProductLink').prop('disabled', true);

        }


      });
    }
    else {
      this.rollFundsLinks.forEach((data1: any, index: any) => {
        if (data1.product == newProduct) {
          alert("please select a new product");
          this.rollFundsIn = {
            product: '',
            value: 0,
          };

        }


      });

    }
  }

  setSelected(id: number) {
    this.selectedProduct = id;
    //this.selectedClientProduct = id;
    var obj = $.grep(this.proposedInvestments, function (obj: any) {
      return obj.recId == id;
    });
    this.selectedProductDetails = obj[0];

    this.rebalanceProduct();
  }

  setCurrentProduct(id: number) {

    this.dropOptionMain = 'block';

    this.currentProductModal = 'block';
    this.selectedcurrentProduct = id;
    var obj = $.grep(this.currentInvestments, function (obj: any) {
      return obj.recId == id;
    });

    // product.percentage = 100;
    this.currentProductDetails = obj[0];
    //var obj = $.grep(this.products, function (obj: any) {
    //    return obj.productId == product.productId;
    //});
    //  this.selectedProductDetails.product = obj[0].productName;

    this.portfolioService.getCurrentFunds(this.selectedcurrentProduct).subscribe(
      cf => {
        this.clientcurrentFunds = cf;
        this.clientcurrentFunds.forEach((pData: any, index: any) => {
          pData.isEditable = false;

        });
      }
    );

    this.fundService.getSelectedFunds(obj[0].productId).subscribe(
      fnds => {
        this.funds = fnds;
        this.originalFunds = fnds;
      }
    );

  }

  async  calculateAA() {
    this.allCurrentFund = [];
    this.allProposedFund = [];

    this.f1();
    //.then((res: any) => this.displayPreview());
  }


  f1() {
    return new Promise(async (resolve, reject) => {

      let current = [];
      for (var i = 0; i < this.currentInvestments.length; i++) {
        current.push(this.portfolioService.getCurrentFunds(this.currentInvestments[i].recId));
      }
      var one = Observable.forkJoin(current);
      let pFund = [];
      let pFee = [];
      let alt = [];
      let rop = [];
      for (var i = 0; i < this.proposedInvestments.length; i++) {
        pFund.push(this.productSwitchingService.getUnderlyingFundsProposed(this.proposedInvestments[i].recId, this.proposedInvestments[i].productId));
        pFee.push(this.feeService.getFeeDetails(this.proposedInvestments[i].recId, 'P'));
        alt.push(this.alternativeProductService.getClientAlternativeProducts(this.proposedInvestments[i].recId));
        rop.push(this.ropCurrentService.getClientROPCurrent(this.proposedInvestments[i].recId));

        var proposedProd = { 'id': this.proposedInvestments[i].recId, 'status': this.proposedInvestments[i].status, 'product': this.proposedInvestments[i].product, 'data': [], 'owner': this.proposedInvestments[i].owner, 'productId': this.proposedInvestments[i].productId, 'platformId': this.proposedInvestments[i].platformId, 'value': this.proposedInvestments[i].value, 'feeDetails': [], 'feeId': this.selectedProductDetails.product + "" + this.proposedInvestments[i].recId };
        this.allProposedFund.push(proposedProd);

      }

      var two = Observable.forkJoin(pFund);
      var all = [];
      var three = Observable.forkJoin(pFee)
      var four = Observable.forkJoin(alt)
      var five = Observable.forkJoin(rop)


      all.push(one);
      all.push(two);
      all.push(three);
      all.push(four);
      all.push(five);

      Observable.forkJoin(all).concatMap((firstFiveResults: any) => this.te(firstFiveResults[3]) == 0 ? this.alternateDetails(firstFiveResults[3]).map((anotherResult: any) => [firstFiveResults, anotherResult]) : [1].map((anotherResult: any) => [firstFiveResults, []])

      ).concatMap((nextResult: any) => this.ne(nextResult[0]) == 0 ? this.existingDetails(nextResult[0]).map((anotherResult: any) => [nextResult[0], nextResult[1], anotherResult]) : [1].map((anotherResult: any) => [nextResult[0], nextResult[1], []])
      )
        .subscribe((dataArrays: any) => {
          var dataArray = dataArrays[0];
          var dataArray2 = dataArrays[1];
          var dataArray3 = dataArrays[2];
          //one
          dataArray[0].forEach((dA: any) => {
            if (dA.length > 0) {
              if (this.currentInvestments.length > 0) {
                this.currentInvestments.forEach((pData: any, index: any) => {
                  if (pData.recId == dA[0].headerId) {
                    pData.data = dA;
                  }
                });
              }
            }

          });
          //two
          dataArray[1].forEach((dA: any) => {
            if (dA.length > 0) {
              var obj = $.grep(this.allProposedFund, function (obj: any) {
                return obj.id == dA[0].headerId;
              });
              obj[0].data = dA;
            }

          });
          this.setAllProposedFundAA();
          //three
          dataArray[2].forEach((fee: any) => {
            if (fee.length > 0) {

              fee.forEach((f: any) => {
                this.individualProductFeeDetails.push(f);

                var obj = $.grep(this.productFeeDetails, function (o: any) {
                  return o.feeName == f.feeName;
                });

                if (obj.length > 0) {

                }
                else {
                  var feeDetails: any = { 'feeName': f.feeName, 'feeType': f.feeType, 'feeCost': f.costType };
                  this.productFeeDetails.push(feeDetails);

                }
              });


              var obj1 = $.grep(this.allProposedFund, function (o: any) {
                return o.productId == fee[0].productId && o.id == fee[0].headerId;
              });
              obj1.forEach((ob: any) => {
                ob.feeDetails = fee;
              });


            }

          });

          //four
          var alternateUnderlyingFund: any = [];
          var alternateFees: any = [];
          alternateUnderlyingFund = [].concat.apply([], dataArray2[0]);
          alternateFees = [].concat.apply([], dataArray2[1]);

          if (alternateFees.length > 0) {

            alternateFees.forEach((f: any) => {
              this.individualProductFeeDetails.push(f);

              var obj = $.grep(this.productFeeDetails, function (o: any) {
                return o.feeName == f.feeName;
              });

              if (obj.length > 0) {

              }
              else {
                //fee.forEach((f: any) => {
                var feeDetails: any = { 'feeName': f.feeName, 'feeType': f.feeType, 'feeCost': f.costType };
                this.productFeeDetails.push(feeDetails);
                //});


              }
            });


          }

          dataArray[3].forEach((items: any) => {

            if (undefined !== items && items.length > 0) {
              items.forEach((tempItem: any) => {

                var alt = $.grep(this.totalAlternativeProducts, function (obj: any) {
                  return obj.id == tempItem.recId;
                });
                if (undefined !== alt && alt.length > 0) {
                  if (alternateUnderlyingFund.length > 0) {
                    var fund = $.grep(alternateUnderlyingFund, function (obj: any) {
                      return obj.headerId == tempItem.recId;
                    });
                    alt[0].data = fund;
                  }

                  if (alternateFees.length > 0) {
                    var fee = $.grep(alternateFees, function (o: any) {
                      return o.productId == tempItem.productId && o.headerId == tempItem.recId;
                    });
                    alt[0].feeDetails = fee;
                  }
                }

              });
            }
          });
          //five
          var existingUnderlyingFund: any = [];
          var existingFees: any = [];
          existingUnderlyingFund = [].concat.apply([], dataArray3[0]);
          existingFees = [].concat.apply([], dataArray3[1]);

          if (existingFees.length > 0) {

            existingFees.forEach((f: any) => {
              this.individualProductFeeDetails.push(f);

              var obj = $.grep(this.productFeeDetails, function (o: any) {
                return o.feeName == f.feeName;
              });

              if (obj.length > 0) {

              }
              else {
                //fee.forEach((f: any) => {
                var feeDetails: any = { 'feeName': f.feeName, 'feeType': f.feeType, 'feeCost': f.costType };
                this.productFeeDetails.push(feeDetails);
                //});


              }
            });

          }

          dataArray[4].forEach((items: any) => {

            if (undefined !== items && items.length > 0) {
              items.forEach((tempItem: any) => {

                var existing = $.grep(this.allCurrentFund, function (obj: any) {
                  return obj.id == tempItem.recId;
                });

                if (undefined !== existing && existing.length > 0) {
                  if (existingUnderlyingFund.length > 0) {
                    var fund = $.grep(existingUnderlyingFund, function (obj: any) {
                      return obj.headerId == tempItem.recId;
                    });
                    existing[0].data = fund;
                  }

                  if (existingFees.length > 0) {
                    var fee = $.grep(existingFees, function (o: any) {
                      return o.productId == tempItem.productId && o.headerId == tempItem.recId;
                    });
                    existing[0].feeDetails = fee;
                  }
                }
              });
            }
          });

          this.setAllCurrentFundAA();
          this.setAllAlternativeFundAA();

          this.generateAllFeeDetails();
          this.displayPreview();
          this.generateWord = 'block';
          this.showLoadingIndicator = false;
        });

      resolve();
    });
  }
  te(items: any): any {
    var isEmpty = 1;
    items.forEach((tempitem: any) => {
      if (tempitem.length > 0) {

        tempitem.forEach((item: any) => {

          isEmpty = 0;
        });
      }



    });
    return isEmpty;
  }
  ne(item: any): any {
    var items = item[4];
    var isEmpty = 1;
    items.forEach((tempitem: any) => {
      if (tempitem.length > 0) {

        tempitem.forEach((item: any) => {

          isEmpty = 0;
        });
      }



    });
    return isEmpty;
  }
  alternateDetails(items: any): any {

    var all: any = [];
    var all1: any = [];
    var all2: any = [];
    var arr1: any = [];
    var arr2: any = [];
    items.forEach((tempitem: any) => {
      if (tempitem.length > 0) {

        tempitem.forEach((item: any) => {

          arr1.push(this.alternativeProductService.getUnderlyingFundsAlternative(item.recId, item.productId));
          arr2.push(this.feeService.getFeeDetails(item.recId, 'A'));

          var altFundDetails: any = {
            apircode: "",
            value: 0,
            percentage: 0
          };

          var temp = { 'id': item.recId, 'data': [], 'owner': item.owner, 'proposedId': item.proposedProduct, 'product': item.product, 'value': item.value, 'productId': item.productId, 'alFunds': [], 'fundDetails': altFundDetails, 'feeDetails': [], 'feeId': item.product + "" + item.recId };
          this.totalAlternativeProducts.push(temp);
        });
      }



    });


    var a = Observable.forkJoin(arr1);
    var b = Observable.forkJoin(arr2);

    all.push(a);
    all.push(b);

    return Observable.forkJoin(all);

  }
  existingDetails(items: any): any {

    var rop = items[4];

    var all: any = [];
    var arr1: any = [];
    var arr2: any = [];
    rop.forEach((tempitem: any) => {
      if (tempitem.length > 0) {

        tempitem.forEach((item: any) => {

          arr1.push(this.ropCurrentService.getUnderlyingFundsROPCurrent(item.recId, item.productId));
          arr2.push(this.feeService.getFeeDetails(item.recId, 'C'));

          var altFundDetails: any = {
            apircode: "",
            value: 0,
            percentage: 0
          };

          var temp = { 'id': item.recId, 'data': [], 'owner': item.owner, 'proposedId': item.proposedProduct, 'product': item.product, 'value': item.value, 'productId': item.productId, 'alFunds': [], 'fundDetails': altFundDetails, 'feeDetails': [], 'feeId': item.product + "" + item.recId };
          this.allCurrentFund.push(temp);
        });
      }



    });
    var a = Observable.forkJoin(arr1);
    var b = Observable.forkJoin(arr2);

    all.push(a);
    all.push(b);

    return Observable.forkJoin(all);
  }


  async getAltProducts(id: number, product: string, owner: string, value: number, productId: number, type: number, proposedId: number, isNewAlt: number) {
    var data: any = await this.alternativeProductService.getUnderlyingFundsAlternative(id, productId).toPromise();
    var fund: any = await this.fundService.getSelectedFunds(productId).toPromise();
    var altFundDetails: any = {
      apircode: "",
      value: 0,
      percentage: 0
    };


    var fees: any = await this.feeService.getFeeDetails(id, 'A').toPromise();

    fees.forEach((fee: any) => {

      this.individualProductFeeDetails.push(fee);

      var obj = $.grep(this.productFeeDetails, function (obj: any) {
        return obj.feeName == fee.feeName;
      });
      if (obj.length > 0) {

      }
      else {
        var feeDetails: any = { 'feeName': fee.feeName, 'feeType': fee.feeType, 'feeCost': fee.costType };
        this.productFeeDetails.push(feeDetails);
      }
    });
    var temp = { 'id': id, 'data': data, 'owner': owner, 'proposedId': proposedId, 'product': product, 'value': value, 'productId': productId, 'alFunds': fund, 'filteredFund': fund, 'fundDetails': altFundDetails, 'feeDetails': fees, 'feeId': product + "" + id };

    //this.filteredOptions = this.myControl.valueChanges
    //  .pipe(_filterAlternative
    //    startWith(''),
    //    map(value => this._filterAlternative(value))
    //);


    if (type == 0) {
      this.alternativeProducts.push(temp);
      this.generateFeeDetails();
      this.setAlternativeFundAA();

      if (isNewAlt == 1) {
        this.currentAltFund = JSON.parse(JSON.stringify(fund));
        this.autoAltFundsSelection(id, productId);

      }

      var obj = $.grep(this.proposedInvestments, function (obj: any) {
        return obj.recId == proposedId;
      });
      var productType = $.grep(this.allProducts, function (prd: any) {
        return prd.productId == obj[0].productId;
      });
      var filteredProducts = $.grep(this.allProducts, function (prd: any) {
        return (prd.productType == productType[0].productType) && (prd.productId != productType[0].productId);
      });

      this.alternativeProductsList = filteredProducts;

      this.alternativeProducts.forEach((item: any, index: any) => {

        var isExist = $.grep(this.alternativeProductsList, function (pr: any) {
          return pr.productId == item.productId;
        });
        if (isExist.length > 0) {
          this.alternativeProductsList.splice(this.alternativeProductsList.findIndex(v => v.productId === isExist[0].productId), 1);
        }

      });

    }
    else if (type == 1) {
      this.totalAlternativeProducts.push(temp);
      this.generateAllFeeDetails();
      this.setAllAlternativeFundAA();

    }
    else if (type == 2) {
      if (this.alternativeProducts.length > 0) {
        this.alternativeProducts.forEach((pData: any, index: any) => {
          if (pData.id == id) {
            this.alternativeProducts.splice(index, 1, temp);
            this.generateFeeDetails();
            this.setAlternativeFundAA();
          }

        });
      }
    }
  }
  async getROPCurrent(id: number, product: string, owner: string, value: number, productId: number, type: number, proposedId: number) {
    var data: any = await this.ropCurrentService.getUnderlyingFundsROPCurrent(id, productId).toPromise();
    var fund: any = await this.fundService.getSelectedFunds(productId).toPromise();
    var ropFundDetails: any = {
      apircode: "",
      value: 0,
      percentage: 0
    };

    var fees: any = await this.feeService.getFeeDetails(id, 'C').toPromise();

    fees.forEach((fee: any) => {

      this.individualProductFeeDetails.push(fee);

      var obj = $.grep(this.productFeeDetails, function (obj: any) {
        return obj.feeName == fee.feeName;
      });
      if (obj.length > 0) {

      }
      else {
        var feeDetails: any = { 'feeName': fee.feeName, 'feeType': fee.feeType, 'feeCost': fee.costType };
        this.productFeeDetails.push(feeDetails);
      }
    });
    var temp = { 'id': id, 'data': data, 'owner': owner, 'proposedId': proposedId, 'product': product, 'value': value, 'productId': productId, 'ropFunds': fund, 'filteredFund': fund, 'fundDetails': ropFundDetails, 'feeDetails': fees, 'feeId': product + "" + id };


    if (type == 0) {
      this.currentFund.push(temp);
      this.generateFeeDetails();
      this.setCurrentFundAA();
    }
    else if (type == 1) {
      this.allCurrentFund.push(temp);
      this.generateAllFeeDetails();
      this.setAllCurrentFundAA();

    }
    else if (type == 2) {
      if (this.currentFund.length > 0) {
        this.currentFund.forEach((pData: any, index: any) => {
          if (pData.id == id) {
            this.currentFund.splice(index, 1, temp);
            this.generateFeeDetails();
            this.setCurrentFundAA();
          }

        });
      }
    }


  }

  onSelectionChanged(event: MatAutocompleteSelectedEvent) {
  }
  async getProposedProducts(id: number, product: string, productId: number, val: number, type: number, owner: string, Replacementstatus: number , platformId : number) {

    var data: any = await this.productSwitchingService.getUnderlyingFundsProposed(id, productId).toPromise();
    this.proposedFund = data;
    var fees: any = await this.feeService.getFeeDetails(id, 'P').toPromise();
    fees.forEach((fee: any) => {

      this.individualProductFeeDetails.push(fee);

      var obj = $.grep(this.productFeeDetails, function (obj: any) {
        return obj.feeName == fee.feeName;
      });
      if (obj.length > 0) {

      }
      else {
        var feeDetails: any = { 'feeName': fee.feeName, 'feeType': fee.feeType, 'feeCost': fee.costType };
        this.productFeeDetails.push(feeDetails);
      }
    });
    var proposedProd = { 'id': id, 'status': Replacementstatus, 'product': product, 'platformId': platformId, 'data': data, 'owner': owner, 'productId': productId, 'value': val, 'feeDetails': fees, 'feeId': this.selectedProductDetails.product + "" + id };
    if (type == 0) {
      this.proposedProduct = [];
      this.proposedProduct.push(proposedProd);
      this.generateFeeDetails();
      this.setProposedFundAA();

    }
    else {

      this.allProposedFund.push(proposedProd);
      this.generateAllFeeDetails();
      this.setAllProposedFundAA();
    }


  }

  //async getCurrentProducts(id: number, product: string, productId: number, val: number, type: number, owner: string) {

  //    var data: any = await this.portfolioService.getCurrentFunds(id).toPromise();   
  //    if (this.currentInvestments.length > 0) {
  //        this.currentInvestments.forEach((pData: any, index: any) => {
  //            if (pData.recId == id) {
  //                pData.data = data;
  //            }

  //        });
  //    }


  //}
  setProposedInvestment(recId: number) {
    this.productSwitchingService.getClientProposedProducts(this.selectedClient).subscribe((data: any) => {
      this.proposedInvestments = data;
      this.proposedProductSum();

      var obj = $.grep(this.proposedInvestments, function (obj: any) {
        return obj.recId == recId;
      });
      this.selectedProductDetails = obj[0];
    });
  }
  async setAlternativeProducts(id: number) {
    var data: any = await this.alternativeProductService.getClientAlternativeProducts(id).toPromise();
    data.forEach((item: any) => {
      this.getAltProducts(item.recId, item.product, item.owner, item.value, item.productId, 1, id, 0);
    });
  }
  async setROPCurrent(id: number) {
    var data: any = await this.ropCurrentService.getClientROPCurrent(id).toPromise();
    data.forEach((item: any) => {
      this.getROPCurrent(item.recId, item.product, item.owner, item.value, item.productId, 1, id);
    });
  }

  rebalanceProduct() {
    this.alternativeProductsList = [];
    $("#productTabs li").eq(0).addClass('active');
    $(".swap").eq(0).addClass('active in');
    this.rebalanceOption = "block";
    var icr = $.grep(this.productFeeDetails, function (obj: any) {
      return obj.feeName == "Indirect Cost Ratio (ICR)";
    });
    if (icr.length <= 0) {
      var icrVal: any = { 'feeName': "Indirect Cost Ratio (ICR)", 'feeType': "P", 'feeCost': "ongoing" };
      this.productFeeDetails.push(icrVal);
    }


    var buySell = $.grep(this.productFeeDetails, function (obj: any) {
      return obj.feeName == "Buy/Sell Costs";
    });
    if (buySell.length <= 0) {
      var bSell: any = { 'feeName': "Buy/Sell Costs", 'feeType': 'F', 'feeCost': "transactional" };
      this.productFeeDetails.push(bSell);
    }

    var gross = $.grep(this.productFeeDetails, function (obj: any) {
      return obj.feeName == "Gross On-going Costs";
    });
    if (icr.length <= 0) {
      var icrVal: any = { 'feeName': "Gross On-going Costs", 'feeType': "F", 'feeCost': "ongoing" };
      this.productFeeDetails.push(icrVal);
    }


    this.dropOptionMain = 'block';
    var selectedProd = this.selectedProduct;

    if (this.selectedProduct != 0) {

      var obj = $.grep(this.proposedInvestments, function (obj: any) {
        return obj.recId == selectedProd;
      });

      this.getProposedProducts(this.selectedProduct, obj[0].product, this.selectedProductDetails.productId, obj[0].value, 0, this.selectedProductDetails.owner, 0, this.selectedProductDetails.platformId);


      this.fundService.getSelectedFunds(obj[0].productId).subscribe(
        fnds => {
          this.funds = fnds;
          this.originalFunds = fnds;
        }
      );

      this.alternativeProductService.getClientAlternativeProducts(this.selectedProduct).subscribe((data: any) => {
        this.alternativeReplacement = data;

        if (this.alternativeReplacement.length > 0) {
          for (var i = 0; i < this.alternativeReplacement.length; i++) {
            this.getAltProducts(data[i].recId, data[i].product, data[i].owner, data[i].value, data[i].productId, 0, this.selectedProduct, 0);


          }
        }
        else {
          var t = this.selectedProduct;
          var obj = $.grep(this.proposedInvestments, function (obj: any) {
            return obj.recId == t;
          });
          var productType = $.grep(this.allProducts, function (prd: any) {
            return prd.productId == obj[0].productId;
          });
          var filteredProducts = $.grep(this.allProducts, function (prd: any) {
            return (prd.productType == productType[0].productType) && (prd.productId != productType[0].productId);
          });

          this.alternativeProductsList = filteredProducts;
        }

      });



      this.alternativeDetails.value = this.selectedProductDetails.value;

      //Newly Added - ROP Current

      this.ropCurrentService.getClientROPCurrent(this.selectedProduct).subscribe((data: any) => {
        this.ropCurrentReplacement = data;
        for (var i = 0; i < this.ropCurrentReplacement.length; i++) {
          this.getROPCurrent(data[i].recId, data[i].product, data[i].owner, data[i].value, data[i].productId, 0, this.selectedProduct);
        }
      });



    }
  }

  //Current Fund
  deleteCurrentFund(item: any, index: any) {
    var id$ = item.recId;
    this.portfolioFundService.delete(id$).subscribe((data) => {
      this.clientcurrentFunds.forEach((pData: any, index: any) => {
        if (pData.recId == id$) {
          this.clientcurrentFunds.splice(index, 1);

        }

      });
    });
  }
  addcurrentFund(currentFund: any) {
    this.portfolioFundService.createFund(this.clientcurrentFundDetails, this.selectedcurrentProduct, this.totalCurrentPercentage).subscribe((data: any) => {
      data.isEditable = false;
      this.clientcurrentFunds.push(data);

      this.clientcurrentFundDetails = {
        apircode: "",
        value: 0,
        percentage: 0
      };


    });
  }
  editCurrentFund(currentFund: any) {
    this.portfolioFundService.createFund(currentFund, this.selectedcurrentProduct, this.totalCurrentPercentage).subscribe((data: any) => {
      var id$ = data.recId;
      if (this.clientcurrentFunds.length > 0) {
        this.clientcurrentFunds.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.clientcurrentFunds.splice(index, 1, data);
            //this.currentFunds.push(data);

          }

        });
      }

      this.clientcurrentFundDetails = {
        apircode: "",
        value: 0,
        percentage: 0
      };
    });
  }



  calculateCurrentPercentage(val: any, i: number) {

    var tempTotal = 0;
    var tempValue = 0;
    if (i == 0) {
      this.clientcurrentFunds.forEach((pData: any) => {
        tempTotal += parseFloat(pData.percentage);
        tempValue += parseFloat(pData.value);
      });
    }
    else {
      this.clientcurrentFunds.forEach((pData: any) => {
        if (pData.recId != i) {
          tempTotal += parseFloat(pData.percentage);
          tempValue += parseFloat(pData.value);
        }

      });
    }


    var percentage = (parseFloat(val) / parseFloat(this.currentProductDetails.value)) * 100;
    var tempPercentage = Math.round(tempTotal + percentage);

    if (tempPercentage > 100 || (tempTotal == 100 && percentage > 0) || ((tempValue + parseFloat(val)) > this.currentProductDetails.value)) {
      if (i == 0) {
        this.clientcurrentFundDetails.value = 0;
        this.clientcurrentFundDetails.percentage = 0;
      }
      else {
        var obj = $.grep(this.clientcurrentFunds, function (obj: any) {
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

        this.clientcurrentFundDetails.percentage = percentage.toFixed(2);
      }
      else {
        var obj = $.grep(this.clientcurrentFunds, function (obj: any) {
          return obj.recId == i;
        });
        obj[0].percentage = percentage.toFixed(2);
      }

    }
  }
  calculateCurrentValue(val: any, i: number) {
    var tempTotal = 0;
    var tempPerc = 0;
    if (i == 0) {
      this.clientcurrentFunds.forEach((pData: any) => {
        tempTotal += parseFloat(pData.value);
        tempPerc += parseFloat(pData.percentage);
      });
    }
    else {
      this.clientcurrentFunds.forEach((pData: any) => {
        if (pData.recId != i) {
          tempTotal += parseFloat(pData.value);
          tempPerc += parseFloat(pData.percentage);
        }

      });
    }
    var value = (parseFloat(val) * parseFloat(this.currentProductDetails.value)) / 100;
    var tempValue = Math.round(tempTotal + value);
    if (tempValue > parseFloat(this.currentProductDetails.value) || (tempTotal == this.currentProductDetails.value && value > 0) || ((tempPerc + parseFloat(val)) > 100)) {
            if (i == 0) {
                this.clientcurrentFundDetails.value = 0;
                this.clientcurrentFundDetails.percentage = 0;
            }
            else {
                var obj = $.grep(this.clientcurrentFunds, function (obj: any) {
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

                this.clientcurrentFundDetails.value = value.toFixed(2);
            }
            else {
                var obj = $.grep(this.clientcurrentFunds, function (obj: any) {
                    return obj.recId == i;
                });
                obj[0].value = value.toFixed(2);
            }


        }
    }

    //Proposed Fund
    deleteProposedFund(item: any, index: any) {
        var id$ = item.recId;
        this.productSwitchingService.delete(id$).subscribe((data) => {
            this.proposedFund.forEach((pData: any, index: any) => {
                if (pData.recId == id$) {
                    this.proposedFund.splice(index, 1);

                }

            });
          this.calculateFee(this.selectedProduct, 'P', this.selectedProductDetails.productId);
          this.generateFeeDetails();
        });
    }
    addProposedFund(proposedFund: any) {
        this.productSwitchingService.createFund(this.proposedFundDetails, this.selectedProduct, this.selectedClientProduct, this.totalPercentage).subscribe((data: any) => {
          this.proposedFund.push(data);
            this.proposedFundDetails = {
                apircode: "",
                value: 0,
                percentage: 0
            };

 
          this.calculateFee(this.selectedProduct, 'P', this.selectedProductDetails.productId);
          this.generateFeeDetails();
        });
    }
    editProposedFund(proposedFund: any) {
        this.productSwitchingService.createFund(proposedFund, this.selectedProduct, this.selectedClientProduct, this.totalPercentage).subscribe((data: any) => {
            var id$ = data.recId;
            if (this.proposedFund.length > 0) {
                this.proposedFund.forEach((pData: any, index: any) => {
                    if (pData.recId == id$) {
                        this.proposedFund.splice(index, 1, data);
                        // this.proposedFund.push(data);
                    }

                });
              this.calculateFee(this.selectedProduct, 'P', this.selectedProductDetails.productId);
              this.generateFeeDetails();
            }

            this.proposedFundDetails = {
                apircode: "",
                value: 0,
                percentage: 0
            };
        });
    }
    calculateValue(val: any, i: number) {
      var tempTotal = 0;
      var tempPerc = 0;
        if (i == 0) {
            this.proposedFund.forEach((pData: any) => {
              tempTotal += parseFloat(pData.value);
              tempPerc += parseFloat(pData.percentage);
            });
        }
        else {
            this.proposedFund.forEach((pData: any) => {
                if (pData.recId != i) {
                  tempTotal += parseFloat(pData.value);
                  tempPerc += parseFloat(pData.percentage);
                }
            });
        }
        var value = (parseFloat(val) * this.selectedProductDetails.value) / 100;
      var tempValue = Math.round(tempTotal + value);
      if (tempValue > this.selectedProductDetails.value || (tempTotal == this.selectedProductDetails.value && value > 0) || ((tempPerc + parseFloat(val)) > 100)) {
            if (i == 0) {
                this.proposedFundDetails.value = 0;
                this.proposedFundDetails.percentage = 0;
            }
            else {
                var obj = $.grep(this.proposedFund, function (obj: any) {
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
                this.proposedFundDetails.value = value;
            }
            else {
                var obj = $.grep(this.proposedFund, function (obj: any) {
                    return obj.recId == i;
                });
                obj[0].value = value;
            }


        }
    }
    calculatePercentage(val: any, i: number) {
      var tempTotal = 0;
      var tempValue = 0;
        if (i == 0) {
            this.proposedFund.forEach((pData: any) => {
              tempTotal += parseFloat(pData.percentage);
              tempValue += parseFloat(pData.value);
            });
        }
        else {
            this.proposedFund.forEach((pData: any) => {
                if (pData.recId != i) {
                  tempTotal += parseFloat(pData.percentage);
                  tempValue += parseFloat(pData.value);
                }

            });
        }

        var percentage = (parseFloat(val) / this.selectedProductDetails.value) * 100;
      var tempPercentage = Math.round(tempTotal + percentage);
      if (tempPercentage > 100 || (tempTotal == 100 && percentage > 0) || ((tempValue + parseFloat(val)) > this.selectedProductDetails.value)) {
            if (i == 0) {
                this.proposedFundDetails.value = 0;
                this.proposedFundDetails.percentage = 0;
            }
            else {
                var obj = $.grep(this.proposedFund, function (obj: any) {
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

                this.proposedFundDetails.percentage = percentage;
            }
            else {
                var obj = $.grep(this.proposedFund, function (obj: any) {
                    return obj.recId == i;
                });
                obj[0].percentage = percentage;
            }

        }
    }

  //Alternative Fund
  deleteAlternativeFund(item: any, index: any, productId: any) {
        var id$ = item.recId;
        this.alternativeProductService.delete(id$).subscribe((data) => {
            this.alternativeProducts.forEach((pData: any, index: any) => {
                pData.data.forEach((pD: any, index: any) => {
                    if (pD.recId == id$) {
                        pData.data.splice(index, 1);

                    }
                });

            });
          this.calculateFee(item.headerId, "A", productId);
          this.generateFeeDetails();
        });
  }
  addAlternativeFund(altFund: any, altProduct: any, productId: any) {

    this.currentAltFund.splice(this.currentAltFund.findIndex(v => v.apircode === altFund.apircode), 1);

     this.alternativeProductService.createFund(altFund, altProduct, this.selectedClientProduct).subscribe((data: any) => {
            // this.alternativeFund.push(data);
            var id$ = data.headerId;

            if (this.alternativeProducts.length > 0) {
                this.alternativeProducts.forEach((pData: any, index: any) => {
                    if (pData.id == id$) {
                        pData.data.push(data);
                        pData.fundDetails = {
                            apircode: "",
                            value: 0,
                            percentage: 0
                        };

                    }



                });
              this.calculateFee(id$, "A", productId);
                this.generateFeeDetails();
            }

        });
    }
  editAlternativeFund(altFund: any, altProduct: any, productId : any) {
        this.alternativeProductService.createFund(altFund, altProduct, this.selectedClientProduct).subscribe((data: any) => {
            var id$ = data.recId;

            if (this.alternativeProducts.length > 0) {
                this.alternativeProducts.forEach((pData: any, index: any) => {
                    if (pData.data.length > 0) {
                        pData.data.forEach((pD: any, index: any) => {
                            if (pD.recId == id$) {
                                pData.data.splice(index, 1, data);

                            }
                        });
                    }
                    pData.fundDetails = {
                        apircode: "",
                        value: 0,
                        percentage: 0
                    };

                });
              this.calculateFee(altFund.headerId, "A", productId);
              this.generateFeeDetails();
            }

        });
    }
    calculateAlternativeValue(val: any, i: number, pId: number) {
      var tempTotal = 0;
      var tempPerc = 0;
        if (i == 0) {
            this.alternativeProducts.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                      tempTotal += parseFloat(data.value);
                      tempPerc += parseFloat(data.percentage);
                    });
                }
            });
        }
        else {
            this.alternativeProducts.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                        if (data.recId != i) {
                          tempTotal += parseFloat(data.value);
                          tempPerc += parseFloat(data.percentage);
                        }
                    });
                }

            });
        }

        this.alternativeProductDetails = $.grep(this.alternativeProducts, function (obj: any) {
            return obj.id == pId;
        });

        var value = (parseFloat(val) * this.alternativeProductDetails[0].value) / 100;
        var tempValue = Math.round(tempTotal + value);
      if (tempValue > this.alternativeProductDetails[0].value || (tempTotal == this.alternativeProductDetails[0].value && value > 0) || ((tempPerc + parseFloat(val)) > 100)) {
            if (i == 0) {

                this.alternativeProductDetails[0].fundDetails.value = 0;
                this.alternativeProductDetails[0].fundDetails.percentage = 0
            }
            else {

                this.alternativeProducts.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        obj[0].value = 0;
                        obj[0].percentage = 0;
                    }
                });

                //var obj = $.grep(this.alternativeFund , function (obj: any) {
                //    return obj.recId == i;
                //});
                //obj[0].value = 0;
                //obj[0].percentage = 0;
            }
            alert("Value cannot exceed total ");
            return;
        }
        else {
            if (i == 0) {
                this.alternativeProductDetails[0].fundDetails.value = value;
            }
            else {
                this.alternativeProducts.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        obj[0].value = value;
                    }
                });
                //var obj = $.grep(this.alternativeFund, function (obj: any) {
                //    return obj.recId == i;
                //});
                //obj[0].value = value;
            }


        }
    }
    calculateAlternativePercentage(val: any, i: number, pId: number) {
      var tempTotal = 0;
      var tempValue = 0;
        if (i == 0) {

            this.alternativeProducts.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                      tempTotal += parseFloat(data.percentage);
                      tempValue += parseFloat(data.value);
                    });
                }
            });
        }
        else {
            this.alternativeProducts.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                        if (data.recId != i) {
                          tempTotal += parseFloat(data.percentage);
                          tempValue += parseFloat(data.value);
                        }
                    });
                }

            });
        }

        this.alternativeProductDetails = $.grep(this.alternativeProducts, function (obj: any) {
            return obj.id == pId;
        });

        var percentage = (parseFloat(val) / this.alternativeProductDetails[0].value) * 100;
        var tempPercentage = Math.round(tempTotal + percentage);
        if (tempPercentage > 100 || (tempTotal == 100 && percentage > 0) || ((tempValue + parseFloat(val)) > this.alternativeProductDetails[0].value)) {
            if (i == 0) {

                this.alternativeProductDetails[0].fundDetails.value = 0;
                this.alternativeProductDetails[0].fundDetails.percentage = 0;
            }
            else {
                this.alternativeProducts.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        obj[0].value = 0;
                        obj[0].percentage = 0;
                    }
                });

            }
            alert("Value cannot exceed total ");
            return;
        }
        else {

            if (i == 0) {
                this.alternativeProductDetails[0].fundDetails.percentage = percentage;
            }
            else {
                this.alternativeProducts.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        // obj[0].value = 0;
                        obj[0].percentage = percentage;
                    }
                });

            }

        }
    }

    //ROP Current Fund
  deleteROPFund(item: any, index: any, productId: any) {
        var id$ = item.recId;
        this.ropCurrentService.delete(id$).subscribe((data) => {

            this.currentFund.forEach((pData: any, index: any) => {
                pData.data.forEach((pD: any, index: any) => {
                  if (pD.recId == id$) {
                        pData.data.splice(index, 1);

                    }
                });

            });

          this.calculateFee(item.headerId, "C", productId);
          this.generateFeeDetails();
        });
    }
    addROPFund(ropFund: any, ropProduct: any,productId:any) {
        this.ropCurrentService.createFund(ropFund, ropProduct, this.selectedClientProduct).subscribe((data: any) => {
            // this.alternativeFund.push(data);

            var id$ = data.headerId;

            if (this.currentFund.length > 0) {
                this.currentFund.forEach((pData: any, index: any) => {
                  if (pData.id == id$) {
                        pData.data.push(data);
                        pData.fundDetails = {
                            apircode: "",
                            value: 0,
                            percentage: 0
                        };

                    }



                });
              this.calculateFee(id$, "C", productId);
              this.generateFeeDetails();
            }

        });
    }
    editROPFund(ropFund: any, ropProduct: any, productId: any) {
        this.ropCurrentService.createFund(ropFund, ropProduct, this.selectedClientProduct).subscribe((data: any) => {
            var id$ = data.recId;

            if (this.currentFund.length > 0) {
                this.currentFund.forEach((pData: any, index: any) => {
                    if (pData.data.length > 0) {
                        pData.data.forEach((pD: any, index: any) => {
                            if (pD.recId == id$) {
                                pData.data.splice(index, 1, data);

                            }
                        });
                    }
                    pData.fundDetails = {
                        apircode: "",
                        value: 0,
                        percentage: 0
                    };

                });
              this.calculateFee(ropFund.headerId, "C", productId);
              this.generateFeeDetails();
            }

        });
    }
    calculateROPValue(val: any, i: number, pId: number) {
      var tempTotal = 0;
      var tempPerc = 0;
        if (i == 0) {
            this.currentFund.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                      tempTotal += parseFloat(data.value);
                      tempPerc += parseFloat(data.percentage);
                    });
                }
            });
        }
        else {
            this.currentFund.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                        if (data.recId != i) {
                          tempTotal += parseFloat(data.value);
                          tempPerc += parseFloat(data.percentage);
                        }
                    });
                }

            });
        }

        this.ropCurrentDetails = $.grep(this.currentFund, function (obj: any) {
            return obj.id == pId;
        });

        var value = (parseFloat(val) * this.ropCurrentDetails[0].value) / 100;
        var tempValue = Math.round(tempTotal + value);
      if (tempValue > this.ropCurrentDetails[0].value || (tempTotal == this.ropCurrentDetails[0].value && value > 0) || ((tempPerc + parseFloat(val)) > 100)) {
            if (i == 0) {

                this.ropCurrentDetails[0].fundDetails.value = 0;
                this.ropCurrentDetails[0].fundDetails.percentage = 0
            }
            else {

                this.currentFund.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        obj[0].value = 0;
                        obj[0].percentage = 0;
                    }
                });
            }
            alert("Value cannot exceed total ");
            return;
        }
        else {
            if (i == 0) {
                this.ropCurrentDetails[0].fundDetails.value = value;
            }
            else {
                this.currentFund.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        obj[0].value = value;
                    }
                });
            }


        }
    }
    calculateROPPercentage(val: any, i: number, pId: number) {
      var tempTotal = 0;
      var tempValue = 0;
        if (i == 0) {

            this.currentFund.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                      tempTotal += parseFloat(data.percentage);
                      tempValue += parseFloat(data.value);
                    });
                }
            });
        }
        else {
            this.currentFund.forEach((pData: any) => {
                if (pData.id == pId) {
                    pData.data.forEach((data: any) => {
                        if (data.recId != i) {
                          tempTotal += parseFloat(data.percentage);
                          tempValue += parseFloat(data.value);
                        }
                    });
                }

            });
        }

        this.ropCurrentDetails = $.grep(this.currentFund, function (obj: any) {
            return obj.id == pId;
        });

        var percentage = (parseFloat(val) / this.ropCurrentDetails[0].value) * 100;
        var tempPercentage = Math.round(tempTotal + percentage);
      if (tempPercentage > 100 || (tempTotal == 100 && percentage > 0) || ((tempValue + parseFloat(val)) > this.ropCurrentDetails[0].value)) {
            if (i == 0) {

                this.ropCurrentDetails[0].fundDetails.value = 0;
                this.ropCurrentDetails[0].fundDetails.percentage = 0;
            }
            else {
                this.currentFund.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        obj[0].value = 0;
                        obj[0].percentage = 0;
                    }
                });

            }
            alert("Value cannot exceed total ");
            return;
        }
        else {

            if (i == 0) {
                this.ropCurrentDetails[0].fundDetails.percentage = percentage;
            }
            else {
                this.currentFund.forEach((pData: any) => {
                    if (pData.id == pId) {
                        var obj = $.grep(pData.data, function (obj: any) {
                            return obj.recId == i;
                        });
                        // obj[0].value = 0;
                        obj[0].percentage = percentage;
                    }
                });

            }

        }
    }

    //Add Products
    addAltProduct() {
      if (this.selectedProductDetails.productId > 0) {
        this.alternativeProductService.getAllAlternativeProducts().subscribe((data: any) => {
          this.allAlternativeProducts = data;
          if (this.allAlternativeProducts.length > 0) {
            var t = Math.max.apply(Math, this.allAlternativeProducts.map(function (o) { return o.recId; }))
            this.alternativeProduct = t + 1;

          }
          else {
            this.alternativeProduct = 1;
          }
          this.alternativeDetails.clientId = this.selectedClient;
          this.alternativeDetails.recId = this.alternativeProduct;
          this.alternativeProductService.createNewProduct(this.alternativeDetails, this.selectedProduct).subscribe((data: any) => {
            this.alternativeProductsList.forEach((pData: any, index: any) => {
              if (pData.productId == data.productId) {
                this.alternativeProductsList.splice(index, 1);
              }
            });

            this.getAltProducts(data.recId, data.product, data.owner, data.value, data.productId, 0, this.selectedProduct, 1);
            this.alternativeFundDetails = {
              apircode: "",
              value: 0,
              percentage: 0
            };

            this.alternativeDetails = {
              productId: 0,
              productName: "",
              productType: "",
              owner: "Client",
              value: 0
            };

            this.alternativeDetails.value = this.selectedProductDetails.value;


          });
        });
      }
      else {
        alert("please enter a proposed product");
      }
        

    }
    autoAltFundsSelection(id: number,productId): any {
  
    var tmpProposedFund: any = this.proposedFund;

    tmpProposedFund.forEach((data: any) => {
    
      var obj = $.grep(this.allFunds, function (obj: any) {
        return obj.apircode == data.apircode;
      });

      var altFund: any = {};

      if (obj.length != 0 && this.currentAltFund.length != 0) {
        var apirExist = $.grep(this.currentAltFund, function (fnd: any) {
          return fnd.apircode == obj[0].apircode;
        });

        if (apirExist.length > 0) {
          //altFund = data;
          altFund.value = data.value;
          altFund.percentage = data.percentage;
          altFund.apircode = apirExist[0].apircode;
          this.addAlternativeFund(altFund, id, productId);
        }
        else {

          // step1 - Single/Multiple
          var singleOrMultiple = $.grep(this.currentAltFund, function (fnd: any) {
            return fnd.isSingle == obj[0].isSingle;
          });

          if (singleOrMultiple.length > 0) {

            // step2 - Asset Allocation
            var singleOrMultipleSub: any[] = [];

            //multi sector
            if (obj[0].isSingle == 'M') {

              var domesticEquity = $.grep(singleOrMultiple, function (subFnd: any) {
                return (subFnd.domesticEquity >= (obj[0].domesticEquity - 10) && subFnd.domesticEquity <= (obj[0].domesticEquity + 10));
              });
              var internationalEquity = $.grep(singleOrMultiple, function (subFnd: any) {
                return (subFnd.internationalEquity >= (obj[0].internationalEquity - 10) && subFnd.internationalEquity <= (obj[0].internationalEquity + 10));
              });
              var property = $.grep(singleOrMultiple, function (subFnd: any) {
                return ((subFnd.domesticProperty + subFnd.internationalProperty) >= ((obj[0].domesticProperty + obj[0].internationalProperty) - 10) && (subFnd.domesticProperty + subFnd.internationalProperty) <= ((obj[0].domesticProperty + obj[0].internationalProperty) + 10));
              });
              var fixedInterest = $.grep(singleOrMultiple, function (subFnd: any) {
                return ((subFnd.domesticFixedInterest + subFnd.internationalFixedInterest) >= ((obj[0].domesticFixedInterest + obj[0].internationalFixedInterest) - 10) && (subFnd.domesticFixedInterest + subFnd.internationalFixedInterest) <= ((obj[0].domesticFixedInterest + obj[0].internationalFixedInterest) + 10));
              });
              var cash = $.grep(singleOrMultiple, function (subFnd: any) {
                return ((subFnd.domesticCash + subFnd.internationalCash) >= ((obj[0].domesticCash + obj[0].internationalCash) - 10) && (subFnd.domesticCash + subFnd.internationalCash) <= ((obj[0].domesticCash + obj[0].internationalCash) + 10));
              });
              var growth = $.grep(singleOrMultiple, function (subFnd: any) {
                return ((subFnd.growthAlternatives + subFnd.otherGrowth) >= ((obj[0].growthAlternatives + obj[0].otherGrowth) - 10) && (subFnd.growthAlternatives + subFnd.otherGrowth) <= ((obj[0].growthAlternatives + obj[0].otherGrowth) + 10));
              });
              var defensive = $.grep(singleOrMultiple, function (subFnd: any) {
                return (subFnd.defensiveAlternatives >= (obj[0].defensiveAlternatives - 10) && subFnd.defensiveAlternatives <= (obj[0].defensiveAlternatives + 10));
              });

              var total = domesticEquity.concat(internationalEquity, property, fixedInterest, cash, growth, defensive);

              var assetAllocation: any[] = []

              if (obj[0].subType == "Hedged") {
                var hedged = $.grep(total, function (subFnd: any) {
                  return subFnd.subType == "Hedged";
                });

                if (hedged.length > 0) {

                  var hedgedAA = $.grep(hedged, function (hgdFnd: any) {
                    return hgdFnd.investorProfile == obj[0].investorProfile;
                  });

                  if (hedgedAA.length > 0) {
                    assetAllocation = hedgedAA;
                  }
                  else {
                    assetAllocation = $.grep(total, function (fnd: any) {
                      return fnd.investorProfile == obj[0].investorProfile;
                    });
                  }

                }
                else {
                  assetAllocation = $.grep(total, function (fnd: any) {
                    return fnd.investorProfile == obj[0].investorProfile;
                  });
                }
              }
              else {
                assetAllocation = $.grep(total, function (fnd: any) {
                  return fnd.investorProfile == obj[0].investorProfile;
                });
              }
              
              if (assetAllocation.length > 0) {
                var activeOrPassive = $.grep(assetAllocation, function (fnd: any) {
                  return fnd.isActive == obj[0].isActive;
                });
                if (activeOrPassive.length > 0) {

                  var currentFundByICR = $.grep(activeOrPassive, function (fnd: any) {
                    return fnd.icr > obj[0].icr;
                  });

                  if (currentFundByICR.length > 0) {

                    currentFundByICR.sort(function (a, b) {
                      return (b.icr - a.icr);
                    });

                    var tagArray = _.pluck(currentFundByICR, 'apircode');
                    var mostCommonTag = _.chain(tagArray).countBy().pairs().max(_.last).head().value();


                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = mostCommonTag
                    this.addAlternativeFund(altFund, id, productId);
                  }
                  else {
                    var tagArray = _.pluck(activeOrPassive, 'apircode');
                    var mostCommonTag = _.chain(tagArray).countBy().pairs().max(_.last).head().value();


                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = mostCommonTag;
                    this.addAlternativeFund(altFund, id, productId);
                  }
                }
                else {

                  var currentFundByICR = $.grep(assetAllocation, function (fnd: any) {
                    return fnd.icr > obj[0].icr;
                  });

                  if (currentFundByICR.length > 0) {

                    currentFundByICR.sort(function (a, b) {
                      return (b.icr - a.icr);
                    });

                    var tagArray = _.pluck(currentFundByICR, 'apircode');
                    var mostCommonTag = _.chain(tagArray).countBy().pairs().max(_.last).head().value();


                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = mostCommonTag;
                    this.addAlternativeFund(altFund, id, productId);
                  }
                  else {


                    var tagArray = _.pluck(assetAllocation, 'apircode');
                    var mostCommonTag = _.chain(tagArray).countBy().pairs().max(_.last).head().value();


                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = mostCommonTag;
                    this.addAlternativeFund(altFund, id, productId);
                  }
                }
              }
              else {

                var currentFundByICR = $.grep(total, function (fnd: any) {
                  return fnd.icr > obj[0].icr;
                });

                if (currentFundByICR.length > 0) {

                  currentFundByICR.sort(function (a, b) {
                    return (b.icr - a.icr);
                  });

                  var tagArray = _.pluck(currentFundByICR, 'apircode');
                  var mostCommonTag = _.chain(tagArray).countBy().pairs().max(_.last).head().value();


                  altFund.value = data.value;
                  altFund.percentage = data.percentage;
                  altFund.apircode = mostCommonTag
                  this.addAlternativeFund(altFund, id, productId);
                }
                else {

                  var tagArray = _.pluck(total, 'apircode');
                  var mostCommonTag = _.chain(tagArray).countBy().pairs().max(_.last).head().value();


                  altFund.value = data.value;
                  altFund.percentage = data.percentage;
                  altFund.apircode = mostCommonTag
                  this.addAlternativeFund(altFund, id, productId);
                }
              }
            }
            //single sector
            else {
              var assetAllocation = $.grep(singleOrMultiple, function (fnd: any) {
                if (obj[0].subType == "Hedged") {

                  var hedged = $.grep(singleOrMultiple, function (subFnd: any) {
                    return subFnd.subType == "Hedged";
                  });

                  if (hedged.length > 0) {

                    var hedgedAA = $.grep(hedged, function (hgdFnd: any) {
                      return hgdFnd.investorProfile == obj[0].investorProfile;
                    });

                    if (hedgedAA.length > 0) {
                      return hedgedAA[0];
                    }
                    else {
                      return fnd.investorProfile == obj[0].investorProfile;
                    }

                  }
                  else {
                    return fnd.investorProfile == obj[0].investorProfile;
                  }

                }
                else {
                  return fnd.investorProfile == obj[0].investorProfile;
                }


              });

              if (assetAllocation.length > 0) {

                // step3 - Active/Passive
                var activeOrPassive = $.grep(assetAllocation, function (fnd: any) {
                  return fnd.isActive == obj[0].isActive;
                });

                if (activeOrPassive.length > 0) {

                  var currentFundByICR = $.grep(activeOrPassive, function (fnd: any) {
                    return fnd.icr > obj[0].icr;
                  });

                  if (currentFundByICR.length > 0) {

                    currentFundByICR.sort(function (a, b) {
                      return (b.icr - a.icr);
                    });

                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = currentFundByICR[0].apircode;
                    this.addAlternativeFund(altFund, id, productId);
                  }
                  else {
                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = activeOrPassive[0].apircode;
                    this.addAlternativeFund(altFund, id, productId);
                  }
                }
                else {

                  var currentFundByICR = $.grep(assetAllocation, function (fnd: any) {
                    return fnd.icr > obj[0].icr;
                  });

                  if (currentFundByICR.length > 0) {

                    currentFundByICR.sort(function (a, b) {
                      return (b.icr - a.icr);
                    });

                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = currentFundByICR[0].apircode;
                    this.addAlternativeFund(altFund, id, productId);
                  }
                  else {
                    altFund.value = data.value;
                    altFund.percentage = data.percentage;
                    altFund.apircode = assetAllocation[0].apircode;
                    this.addAlternativeFund(altFund, id, productId);
                  }
                }


              }
              else {

                var currentFundByICR = $.grep(singleOrMultiple, function (fnd: any) {
                  return fnd.icr > obj[0].icr;
                });

                if (currentFundByICR.length > 0) {

                  currentFundByICR.sort(function (a, b) {
                    return (b.icr - a.icr);
                  });

                  altFund.value = data.value;
                  altFund.percentage = data.percentage;
                  altFund.apircode = currentFundByICR[0].apircode;
                  this.addAlternativeFund(altFund, id, productId);
                }
                else {
                  altFund.value = data.value;
                  altFund.percentage = data.percentage;
                  altFund.apircode = singleOrMultiple[0].apircode;
                  this.addAlternativeFund(altFund, id, productId);
                }
              }
            }

          
          }
          else {

            var currentFundByICR = $.grep(this.currentAltFund, function (fnd: any) {
              return fnd.icr > obj[0].icr;
            });         

            if (currentFundByICR.length > 0) {

              currentFundByICR.sort(function (a, b) {
                return (b.icr - a.icr);
              });

              altFund.value = data.value;
              altFund.percentage = data.percentage;
              altFund.apircode = currentFundByICR[0].apircode;
              this.addAlternativeFund(altFund, id, productId);
            }
            else {
              altFund.value = data.value;
              altFund.percentage = data.percentage;
              altFund.apircode = this.currentAltFund[0].apircode;
              this.addAlternativeFund(altFund, id, productId);
            }
          }

        }

      };
     

    });


    }
    editAltProduct(altProduct: any, id: number) {
        altProduct.recId = id;
        altProduct.clientId = this.selectedClient;
        this.alternativeProductService.createNewProduct(altProduct, this.selectedProduct).subscribe((data: any) => {
            this.getAltProducts(data.recId, data.product, data.owner, data.value, data.productId, 2, this.selectedProduct,0);
        }); 
    }
    deleteAltProduct(id: number, productId: number) {
      this.alternativeProductService.deleteProduct(id, this.selectedClient).subscribe((data: any) => {

        var obj = $.grep(this.allProducts, function (prd: any) {
          return prd.productId == productId;
        });

        this.alternativeProductsList.push(obj[0]);

        this.alternativeProducts.forEach((pData: any, index: any) => {
                if (pData.id == id) {
                    this.alternativeProducts.splice(index, 1);

                }
         });
        });
    }
    addROPCurrentProduct() {
        this.ropCurrentService.getAllROPCurrent().subscribe((data: any) => {
            this.allCurrentFund = data;
            if (this.allCurrentFund.length > 0) {
                var t = Math.max.apply(Math, this.allCurrentFund.map(function (o) { return o.recId; }))
                this.ropCurrent = t + 1;
            }
            else {
                this.ropCurrent = 1;
            }
            this.ropDetails.clientId = this.selectedClient;
            this.ropDetails.recId = this.ropCurrent;

            this.ropCurrentService.createNewProduct(this.ropDetails, this.selectedProduct).subscribe((data: any) => {
                this.getROPCurrent(data.recId, data.product, data.owner, data.value, data.productId, 0, this.selectedProduct);

                this.ropFundDetails = {
                    apircode: "",
                    value: 0,
                    percentage: 0
                };

                this.ropDetails = {
                    productId: 0,
                    productName: "",
                    productType: "",
                    owner: "Client",
                    value: 0
                };

                
            });
        });

    }
    editROPCurrentProduct(currentProduct: any, id: number) {
        currentProduct.recId = id;
        currentProduct.clientId = this.selectedClient;
        this.ropCurrentService.createNewProduct(currentProduct, this.selectedProduct).subscribe((data: any) => {
            this.getROPCurrent(data.recId, data.product, data.owner, data.value, data.productId, 2, this.selectedProduct);
        });
    }
    deleteROPCurrentProduct(id: number) {
        this.ropCurrentService.deleteProduct(id, this.selectedClient).subscribe((data: any) => {
            this.currentFund.forEach((pData: any, index: any) => {
                if (pData.id == id) {
                    this.currentFund.splice(index, 1);

                }


            });
        });
    }
    addcurrentProduct(currentProduct: any) {
        this.portfolioService.createProduct(this.currentProductDetails, this.selectedClient, this.currentProductDetails.owner).subscribe((data) => {

            this.currentInvestments.push(data);

            var Currenttotal = 0;
            this.currentInvestments.forEach(function (obj) {
                Currenttotal += obj.value;
            });

            this.totalCurrentInvestments = Currenttotal;

            this.currentProductDetails = data;
            this.selectedcurrentProduct = this.currentProductDetails.recId;

            this.portfolioService.getCurrentFunds(this.selectedcurrentProduct).subscribe(
                cf => {
                    this.clientcurrentFunds = cf;
                    this.clientcurrentFunds.forEach((pData: any, index: any) => {
                        pData.isEditable = false;

                    });
                }
            );

            this.fundService.getSelectedFunds(this.currentProductDetails.productId).subscribe(
                fnds => {
                  this.funds = fnds;
                  this.originalFunds = fnds;  
                }
            );
        });
    }
    addproposedProduct(newProduct: any) {
        this.productSwitchingService.getAllProposedProducts().subscribe((data: any) => {
            this.rebalanceOption = "block";
            this.allProposedInvestments = data;
            if (this.allProposedInvestments.length > 0) {
                var t = Math.max.apply(Math, this.allProposedInvestments.map(function (o) { return o.recId; }))
                this.selectedProduct = t + 1;

            }
            else {
                this.selectedProduct = 1;
            }

            this.selectedProductDetails.clientId = this.selectedClient;
            this.selectedProductDetails.recId = this.selectedProduct;
            this.newProductService.createNewProduct(this.selectedProductDetails, this.selectedClient).subscribe((data: any) => {
              this.proposedInvestments.push(data);
            //  this.proposedProduct.push(data); // todo - verify ? 
                this.proposedProductSum();
                this.selectedProductDetails = data;
                this.selectedProduct = this.selectedProductDetails.recId;

              this.productSwitchingService.getUnderlyingFundsProposed(this.selectedProduct, this.selectedProductDetails.productId).subscribe(
                    pf => {
                        this.proposedFund = pf;
                        this.proposedFund.forEach((pData: any, index: any) => {
                            pData.isEditable = false;

                        });
                    }
              );

              this.getProposedProducts(this.selectedProduct, data.product, this.selectedProductDetails.productId, data.value, 0, this.selectedProductDetails.owner, 0, this.selectedProductDetails.platformId);

                this.fundService.getSelectedFunds(this.selectedProductDetails.productId).subscribe(
                    fnds => {
                      this.funds = fnds;
                      this.originalFunds = fnds;  
                    }
                );

              var t = this.selectedProduct;
              var obj = $.grep(this.proposedInvestments, function (obj: any) {
                return obj.recId == t;
              });
              var productType = $.grep(this.allProducts, function (prd: any) {
                return prd.productId == obj[0].productId;
              });
              var filteredProducts = $.grep(this.allProducts, function (prd: any) {
                return (prd.productType == productType[0].productType) && (prd.productId != productType[0].productId);
              });

              this.alternativeProductsList = filteredProducts;

            });
        });

    }
    editproposedProduct(newProduct: any) {
        this.newProductService.createNewProduct(this.selectedProductDetails, this.selectedClient).subscribe((data: any) => {
            var id$ = data.recId;
            if (this.proposedInvestments.length > 0) {
                this.proposedInvestments.forEach((pData: any, index: any) => {
                    if (pData.recId == id$) {
                        this.proposedInvestments.splice(index, 1, data);
                    }

                });
            }
            this.proposedProductSum();
            this.selectedProductDetails = data;
            this.selectedProduct = this.selectedProductDetails.recId;
        });


    }
    deleteproposedProduct(productId: any) {
        this.newProductService.delete(productId).subscribe((data: any) => {
            this.proposedInvestments.forEach((pData: any, index: any) => {
                if (pData.recId == productId) {
                    this.proposedInvestments.splice(index, 1);
                    this.proposedProductSum();
                }
            });
        });
    }


    public onClick(fund: any): void {
        var obj = $.grep(this.allFunds, function (obj: any) {
            return obj.apircode == fund;
        });

        var totGrowth = 0;
        var totDefensive = 0;
        //for (var key in obj[0]) {
        //    if (key != "amount" && key != "apircode" && key != "buySpread" && key != "fundName" && key != "icr" && key != "total" && key != "totalDefensive" && key != "totalGrowth") {
        //        tot += parseFloat(obj[0][key]);
        //    }
        //}
        for (var key in obj[0]) {
            if (key == "domesticEquity" || key == "internationalEquity" || key == "domesticProperty" || key == "internationalProperty" || key == "growthAlternatives" || key == "otherGrowth") {
                totGrowth += parseFloat(obj[0][key]);
            }
        }
        for (var key in obj[0]) {
            if (key == "domesticCash" || key == "internationalCash" || key == "domesticFixedInterest" || key == "internationalFixedInterest" || key == "defensiveAlternatives") {
                totDefensive += parseFloat(obj[0][key]);
            }
        }
        obj[0].total = (totGrowth + totDefensive).toFixed(2);
        obj[0].totalGrowth = totGrowth.toFixed(2);
        obj[0].totalDefensive = totDefensive.toFixed(2);
        this.investmentDetails = obj[0];
        this.fundDisplay = 'block';
        this.rebalanceOption = 'none';
    }
    getRP(id: any, type: any) {
        this.riskProfile = 'block';
        this.rebalanceOption = 'none';

        var product: any = {};
        if (type == "current") {
            product = $.grep(this.currentFund, function (obj: any) {
                return obj.id == id;
            });
        }
        else if (type == "proposed") {
            product = $.grep(this.proposedProduct, function (obj: any) {
                return obj.id == id;
            });
        }
        else if (type == "alternative") {
            product = $.grep(this.alternativeProducts, function (obj: any) {
                return obj.id == id;
            });
        }

        this.riskProfileMatchSummary = [];
        var t = this;
        this.riskProfiles.forEach(function (rp) {
            var obj: any = {};
            obj.id = id;
            obj.type = type;
            obj.RiskProfile = rp.riskProfile1;
            obj.Growth = rp.growth;
            obj.Variance = (Math.abs(parseFloat(product[0].totGrowth) - parseFloat(rp.growth))).toFixed(2);
            obj.Match = (100 - parseFloat(obj.Variance)).toFixed(2);

            if (obj.RiskProfile == product[0].riskProfile.riskProfile1) {
                obj.checked = true;
            }
            else {
                obj.checked = false;
            } 


         
            t.riskProfileMatchSummary.push(obj);
        });

        t.riskProfileMatchSummary.sort(function (a, b) {
            return b.Match - a.Match ;
        })



    }
    onRPChange(product: any) {
        if (product.type == 'current') {
            var cf = this.currentFund.find(function (o) { return o.id == product.id });

            var RpObj = this.riskProfiles.find(function (o) { return o.riskProfile1 == product.RiskProfile });
            RpObj.total = parseFloat(RpObj.growth) + parseFloat(RpObj.defensive);
            cf.riskProfile = RpObj;

            var variance: any = {};
            variance.domesticEquity = (Math.abs(parseFloat(cf.domesticEquity) - parseFloat(cf.riskProfile.domesticEquity))).toFixed(2);
            variance.internationalEquity = (Math.abs(parseFloat(cf.internationalEquity) - parseFloat(cf.riskProfile.internationalEquity))).toFixed(2);
            variance.domesticProperty = (Math.abs(parseFloat(cf.domesticProperty) - parseFloat(cf.riskProfile.domesticProperty))).toFixed(2);
            variance.internationalProperty = (Math.abs(parseFloat(cf.internationalProperty) - parseFloat(cf.riskProfile.internationalProperty))).toFixed(2);
            variance.growthAlternatives = (Math.abs(parseFloat(cf.growthAlternatives) - parseFloat(cf.riskProfile.growthAlternatives))).toFixed(2);
            variance.otherGrowth = (Math.abs(parseFloat(cf.otherGrowth) - parseFloat(cf.riskProfile.otherGrowth))).toFixed(2);
            variance.domesticCash = (Math.abs(parseFloat(cf.domesticCash) - parseFloat(cf.riskProfile.domesticCash))).toFixed(2);
            variance.internationalCash = (Math.abs(parseFloat(cf.internationalCash) - parseFloat(cf.riskProfile.internationalCash))).toFixed(2);
            variance.domesticFixedInterest = (Math.abs(parseFloat(cf.domesticFixedInterest) - parseFloat(cf.riskProfile.domesticFixedInterest))).toFixed(2);
            variance.internationalFixedInterest = (Math.abs(parseFloat(cf.internationalFixedInterest) - parseFloat(cf.riskProfile.internationalFixedInterest))).toFixed(2);
            variance.defensiveAlternatives = (Math.abs(parseFloat(cf.defensiveAlternatives) - parseFloat(cf.riskProfile.defensiveAlternatives))).toFixed(2);
            variance.totGrowth = (Math.abs(parseFloat(cf.totGrowth) - parseFloat(cf.riskProfile.growth))).toFixed(2);
            variance.totDefensive = (Math.abs(parseFloat(cf.totDefensive) - parseFloat(cf.riskProfile.defensive))).toFixed(2);
            variance.total = (Math.abs(parseFloat(cf.total) - parseFloat(cf.riskProfile.total))).toFixed(2);

            cf.variance = variance;
        }
        else if (product.type == 'proposed') {
            var pf = this.proposedProduct.find(function (o) { return o.id == product.id });

            var RpObj = this.riskProfiles.find(function (o) { return o.riskProfile1 == product.RiskProfile });
            RpObj.total = parseFloat(RpObj.growth) + parseFloat(RpObj.defensive);
            pf.riskProfile = RpObj;

            var variance: any = {};
            variance.domesticEquity = (Math.abs(parseFloat(pf.domesticEquity) - parseFloat(pf.riskProfile.domesticEquity))).toFixed(2);
            variance.internationalEquity = (Math.abs(parseFloat(pf.internationalEquity) - parseFloat(pf.riskProfile.internationalEquity))).toFixed(2);
            variance.domesticProperty = (Math.abs(parseFloat(pf.domesticProperty) - parseFloat(pf.riskProfile.domesticProperty))).toFixed(2);
            variance.internationalProperty = (Math.abs(parseFloat(pf.internationalProperty) - parseFloat(pf.riskProfile.internationalProperty))).toFixed(2);
            variance.growthAlternatives = (Math.abs(parseFloat(pf.growthAlternatives) - parseFloat(pf.riskProfile.growthAlternatives))).toFixed(2);
            variance.otherGrowth = (Math.abs(parseFloat(pf.otherGrowth) - parseFloat(pf.riskProfile.otherGrowth))).toFixed(2);
            variance.domesticCash = (Math.abs(parseFloat(pf.domesticCash) - parseFloat(pf.riskProfile.domesticCash))).toFixed(2);
            variance.internationalCash = (Math.abs(parseFloat(pf.internationalCash) - parseFloat(pf.riskProfile.internationalCash))).toFixed(2);
            variance.domesticFixedInterest = (Math.abs(parseFloat(pf.domesticFixedInterest) - parseFloat(pf.riskProfile.domesticFixedInterest))).toFixed(2);
            variance.internationalFixedInterest = (Math.abs(parseFloat(pf.internationalFixedInterest) - parseFloat(pf.riskProfile.internationalFixedInterest))).toFixed(2);
            variance.defensiveAlternatives = (Math.abs(parseFloat(pf.defensiveAlternatives) - parseFloat(pf.riskProfile.defensiveAlternatives))).toFixed(2);
            variance.totGrowth = (Math.abs(parseFloat(pf.totGrowth) - parseFloat(pf.riskProfile.growth))).toFixed(2);
            variance.totDefensive = (Math.abs(parseFloat(pf.totDefensive) - parseFloat(pf.riskProfile.defensive))).toFixed(2);
            variance.total = (Math.abs(parseFloat(pf.total) - parseFloat(pf.riskProfile.total))).toFixed(2);

            pf.variance = variance;
        }
        else if (product.type == 'alternative') {
            var af = this.alternativeProducts.find(function (o) { return o.id == product.id });

            var RpObj = this.riskProfiles.find(function (o) { return o.riskProfile1 == product.RiskProfile });
            RpObj.total = parseFloat(RpObj.growth) + parseFloat(RpObj.defensive);
            af.riskProfile = RpObj;

            var variance: any = {};
            variance.domesticEquity = (Math.abs(parseFloat(af.domesticEquity) - parseFloat(af.riskProfile.domesticEquity))).toFixed(2);
            variance.internationalEquity = (Math.abs(parseFloat(af.internationalEquity) - parseFloat(af.riskProfile.internationalEquity))).toFixed(2);
            variance.domesticProperty = (Math.abs(parseFloat(af.domesticProperty) - parseFloat(af.riskProfile.domesticProperty))).toFixed(2);
            variance.internationalProperty = (Math.abs(parseFloat(af.internationalProperty) - parseFloat(af.riskProfile.internationalProperty))).toFixed(2);
            variance.growthAlternatives = (Math.abs(parseFloat(af.growthAlternatives) - parseFloat(af.riskProfile.growthAlternatives))).toFixed(2);
            variance.otherGrowth = (Math.abs(parseFloat(af.otherGrowth) - parseFloat(af.riskProfile.otherGrowth))).toFixed(2);
            variance.domesticCash = (Math.abs(parseFloat(af.domesticCash) - parseFloat(af.riskProfile.domesticCash))).toFixed(2);
            variance.internationalCash = (Math.abs(parseFloat(af.internationalCash) - parseFloat(af.riskProfile.internationalCash))).toFixed(2);
            variance.domesticFixedInterest = (Math.abs(parseFloat(af.domesticFixedInterest) - parseFloat(af.riskProfile.domesticFixedInterest))).toFixed(2);
            variance.internationalFixedInterest = (Math.abs(parseFloat(af.internationalFixedInterest) - parseFloat(af.riskProfile.internationalFixedInterest))).toFixed(2);
            variance.defensiveAlternatives = (Math.abs(parseFloat(af.defensiveAlternatives) - parseFloat(af.riskProfile.defensiveAlternatives))).toFixed(2);
            variance.totGrowth = (Math.abs(parseFloat(af.totGrowth) - parseFloat(af.riskProfile.growth))).toFixed(2);
            variance.totDefensive = (Math.abs(parseFloat(af.totDefensive) - parseFloat(af.riskProfile.defensive))).toFixed(2);
            variance.total = (Math.abs(parseFloat(af.total) - parseFloat(af.riskProfile.total))).toFixed(2);

            af.variance = variance;
        }
      
      
      
    }
    setCurrentFundAA() {
        this.currentFund.forEach((cf: any, index: any) => {
            cf.data.forEach((data: any, index: any) => {
                var obj = $.grep(this.allFunds, function(obj: any) {
                    return obj.apircode == data.apircode;
                });
                data.domesticEquity = ((parseFloat(obj[0].domesticEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalEquity = ((parseFloat(obj[0].internationalEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticProperty = ((parseFloat(obj[0].domesticProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalProperty = ((parseFloat(obj[0].internationalProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.growthAlternatives = ((parseFloat(obj[0].growthAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.otherGrowth = ((parseFloat(obj[0].otherGrowth) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticCash = ((parseFloat(obj[0].domesticCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalCash = ((parseFloat(obj[0].internationalCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticFixedInterest = ((parseFloat(obj[0].domesticFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalFixedInterest = ((parseFloat(obj[0].internationalFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.defensiveAlternatives = ((parseFloat(obj[0].defensiveAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.totGrowth = (parseFloat(data.domesticEquity) + parseFloat(data.internationalEquity) + parseFloat(data.domesticProperty) + parseFloat(data.internationalProperty) + parseFloat(data.growthAlternatives) + parseFloat(data.otherGrowth));
                data.totDefensive = (parseFloat(data.domesticCash) + parseFloat(data.internationalCash) + parseFloat(data.domesticFixedInterest) + parseFloat(data.internationalFixedInterest) + parseFloat(data.defensiveAlternatives));
                data.total = (parseFloat(data.totGrowth) + parseFloat(data.totDefensive));
            });


            cf.domesticEquity = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalEquity = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.domesticProperty = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalProperty = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.growthAlternatives = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.otherGrowth = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.domesticCash = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalCash = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.domesticFixedInterest = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalFixedInterest = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.defensiveAlternatives = (cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) > 0 ? (((cf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.totGrowth = (parseFloat(cf.domesticEquity) + parseFloat(cf.internationalEquity) + parseFloat(cf.domesticProperty) + parseFloat(cf.internationalProperty) + parseFloat(cf.growthAlternatives) + parseFloat(cf.otherGrowth)).toFixed(2);
            cf.totDefensive = (parseFloat(cf.domesticCash) + parseFloat(cf.internationalCash) + parseFloat(cf.domesticFixedInterest) + parseFloat(cf.internationalFixedInterest) + parseFloat(cf.defensiveAlternatives)).toFixed(2);
            cf.total = (parseFloat(cf.totGrowth) + parseFloat(cf.totDefensive)).toFixed(2);

            this.riskProfileSummary = [];
            var t = this;
            this.riskProfiles.forEach(function(rp) {
                var obj: any = {};
                obj.RiskProfile = rp.riskProfile1;
                obj.Growth = rp.growth;
                obj.Variance = (Math.abs(parseFloat(cf.totGrowth) - parseFloat(rp.growth))).toFixed(2);
                obj.Match = (100 - parseFloat(obj.Variance)).toFixed(2);
                t.riskProfileSummary.push(obj);
            });


            var res = Math.max.apply(Math, this.riskProfileSummary.map(function(o) { return parseFloat(o.Match) }));
            var maxRP = this.riskProfileSummary.find(function(o) { return o.Match == res });
            var RpObj = this.riskProfiles.find(function(o) { return o.riskProfile1 == maxRP.RiskProfile });
            RpObj.total = parseFloat(RpObj.growth) + parseFloat(RpObj.defensive);
            cf.riskProfile = RpObj;

            var variance: any = {};
            variance.domesticEquity = (Math.abs(parseFloat(cf.domesticEquity) - parseFloat(cf.riskProfile.domesticEquity))).toFixed(2);
            variance.internationalEquity = (Math.abs(parseFloat(cf.internationalEquity) - parseFloat(cf.riskProfile.internationalEquity))).toFixed(2);
            variance.domesticProperty = (Math.abs(parseFloat(cf.domesticProperty) - parseFloat(cf.riskProfile.domesticProperty))).toFixed(2);
            variance.internationalProperty = (Math.abs(parseFloat(cf.internationalProperty) - parseFloat(cf.riskProfile.internationalProperty))).toFixed(2);
            variance.growthAlternatives = (Math.abs(parseFloat(cf.growthAlternatives) - parseFloat(cf.riskProfile.growthAlternatives))).toFixed(2);
            variance.otherGrowth = (Math.abs(parseFloat(cf.otherGrowth) - parseFloat(cf.riskProfile.otherGrowth))).toFixed(2);
            variance.domesticCash = (Math.abs(parseFloat(cf.domesticCash) - parseFloat(cf.riskProfile.domesticCash))).toFixed(2);
            variance.internationalCash = (Math.abs(parseFloat(cf.internationalCash) - parseFloat(cf.riskProfile.internationalCash))).toFixed(2);
            variance.domesticFixedInterest = (Math.abs(parseFloat(cf.domesticFixedInterest) - parseFloat(cf.riskProfile.domesticFixedInterest))).toFixed(2);
            variance.internationalFixedInterest = (Math.abs(parseFloat(cf.internationalFixedInterest) - parseFloat(cf.riskProfile.internationalFixedInterest))).toFixed(2);
            variance.defensiveAlternatives = (Math.abs(parseFloat(cf.defensiveAlternatives) - parseFloat(cf.riskProfile.defensiveAlternatives))).toFixed(2);
            variance.totGrowth = (Math.abs(parseFloat(cf.totGrowth) - parseFloat(cf.riskProfile.growth))).toFixed(2);
            variance.totDefensive = (Math.abs(parseFloat(cf.totDefensive) - parseFloat(cf.riskProfile.defensive))).toFixed(2);
            variance.total = (Math.abs(parseFloat(cf.total) - parseFloat(cf.riskProfile.total))).toFixed(2);

            cf.variance = variance;

            


        });

       
    };
    setAllCurrentFundAA() {

      
        this.allCurrentFund.forEach((cf: any, index: any) => {
            cf.data.forEach((data: any, index: any) => {
                var obj = $.grep(this.allFunds, function (obj: any) {
                    return obj.apircode == data.apircode;
                });
                data.domesticEquity = ((parseFloat(obj[0].domesticEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalEquity = ((parseFloat(obj[0].internationalEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticProperty = ((parseFloat(obj[0].domesticProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalProperty = ((parseFloat(obj[0].internationalProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.growthAlternatives = ((parseFloat(obj[0].growthAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.otherGrowth = ((parseFloat(obj[0].otherGrowth) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticCash = ((parseFloat(obj[0].domesticCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalCash = ((parseFloat(obj[0].internationalCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticFixedInterest = ((parseFloat(obj[0].domesticFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalFixedInterest = ((parseFloat(obj[0].internationalFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.defensiveAlternatives = ((parseFloat(obj[0].defensiveAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.totGrowth = (parseFloat(data.domesticEquity) + parseFloat(data.internationalEquity) + parseFloat(data.domesticProperty) + parseFloat(data.internationalProperty) + parseFloat(data.growthAlternatives) + parseFloat(data.otherGrowth));
                data.totDefensive = (parseFloat(data.domesticCash) + parseFloat(data.internationalCash) + parseFloat(data.domesticFixedInterest) + parseFloat(data.internationalFixedInterest) + parseFloat(data.defensiveAlternatives));
                data.total = (parseFloat(data.totGrowth) + parseFloat(data.totDefensive));
            });


            cf.domesticEquity = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalEquity = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.domesticProperty = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalProperty = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.growthAlternatives = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.otherGrowth = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.domesticCash = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalCash = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.domesticFixedInterest = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.internationalFixedInterest = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.defensiveAlternatives = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) > 0 ? (((cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) / cf.value) * 100).toFixed(2) : 0.00;
            cf.totGrowth = (parseFloat(cf.domesticEquity) + parseFloat(cf.internationalEquity) + parseFloat(cf.domesticProperty) + parseFloat(cf.internationalProperty) + parseFloat(cf.growthAlternatives) + parseFloat(cf.otherGrowth)).toFixed(2);
            cf.totDefensive = (parseFloat(cf.domesticCash) + parseFloat(cf.internationalCash) + parseFloat(cf.domesticFixedInterest) + parseFloat(cf.internationalFixedInterest) + parseFloat(cf.defensiveAlternatives)).toFixed(2);
            cf.total = (parseFloat(cf.totGrowth) + parseFloat(cf.totDefensive)).toFixed(2);

        });
    
    };
    setProposedFundAA() {
  
            this.proposedProduct.forEach((pf: any, index: any) => {
                pf.data.forEach((data: any, index: any) => {
                    var obj = $.grep(this.allFunds, function(obj: any) {
                        return obj.apircode == data.apircode;
                    });
                    data.domesticEquity = ((parseFloat(obj[0].domesticEquity) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalEquity = ((parseFloat(obj[0].internationalEquity) * parseFloat(data.value)) / 100).toFixed(2);
                    data.domesticProperty = ((parseFloat(obj[0].domesticProperty) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalProperty = ((parseFloat(obj[0].internationalProperty) * parseFloat(data.value)) / 100).toFixed(2);
                    data.growthAlternatives = ((parseFloat(obj[0].growthAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                    data.otherGrowth = ((parseFloat(obj[0].otherGrowth) * parseFloat(data.value)) / 100).toFixed(2);
                    data.domesticCash = ((parseFloat(obj[0].domesticCash) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalCash = ((parseFloat(obj[0].internationalCash) * parseFloat(data.value)) / 100).toFixed(2);
                    data.domesticFixedInterest = ((parseFloat(obj[0].domesticFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalFixedInterest = ((parseFloat(obj[0].internationalFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                    data.defensiveAlternatives = ((parseFloat(obj[0].defensiveAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                    data.totGrowth = (parseFloat(data.domesticEquity) + parseFloat(data.internationalEquity) + parseFloat(data.domesticProperty) + parseFloat(data.internationalProperty) + parseFloat(data.growthAlternatives) + parseFloat(data.otherGrowth));
                    data.totDefensive = (parseFloat(data.domesticCash) + parseFloat(data.internationalCash) + parseFloat(data.domesticFixedInterest) + parseFloat(data.internationalFixedInterest) + parseFloat(data.defensiveAlternatives));
                    data.total = (parseFloat(data.totGrowth) + parseFloat(data.totDefensive));
                });

                pf.domesticEquity = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.internationalEquity = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.domesticProperty = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.internationalProperty = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.growthAlternatives = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.otherGrowth = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.domesticCash = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.internationalCash = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.domesticFixedInterest = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.internationalFixedInterest = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.defensiveAlternatives = (pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) > 0 ? (((pf.data.reduce(function(a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
                pf.totGrowth = (parseFloat(pf.domesticEquity) + parseFloat(pf.internationalEquity) + parseFloat(pf.domesticProperty) + parseFloat(pf.internationalProperty) + parseFloat(pf.growthAlternatives) + parseFloat(pf.otherGrowth)).toFixed(2);
                pf.totDefensive = (parseFloat(pf.domesticCash) + parseFloat(pf.internationalCash) + parseFloat(pf.domesticFixedInterest) + parseFloat(pf.internationalFixedInterest) + parseFloat(pf.defensiveAlternatives)).toFixed(2);
                pf.total = (parseFloat(pf.totGrowth) + parseFloat(pf.totDefensive)).toFixed(2);


                this.riskProfileSummary = [];
                var t = this;
                this.riskProfiles.forEach(function(rp) {
                    var obj: any = {};
                    obj.RiskProfile = rp.riskProfile1;
                    obj.Growth = rp.growth;
                    obj.Variance = (Math.abs(parseFloat(pf.totGrowth) - parseFloat(rp.growth))).toFixed(2);
                    obj.Match = (100 - parseFloat(obj.Variance)).toFixed(2);
                    t.riskProfileSummary.push(obj);
                });


                var res = Math.max.apply(Math, this.riskProfileSummary.map(function(o) { return parseFloat(o.Match) }));
                var maxRP = this.riskProfileSummary.find(function(o) { return o.Match == res });
                var RpObj = this.riskProfiles.find(function(o) { return o.riskProfile1 == maxRP.RiskProfile });
                RpObj.total = parseFloat(RpObj.growth) + parseFloat(RpObj.defensive);
                pf.riskProfile = RpObj;

                var variance: any = {};
                variance.domesticEquity = (Math.abs(parseFloat(pf.domesticEquity) - parseFloat(pf.riskProfile.domesticEquity))).toFixed(2);
                variance.internationalEquity = (Math.abs(parseFloat(pf.internationalEquity) - parseFloat(pf.riskProfile.internationalEquity))).toFixed(2);
                variance.domesticProperty = (Math.abs(parseFloat(pf.domesticProperty) - parseFloat(pf.riskProfile.domesticProperty))).toFixed(2);
                variance.internationalProperty = (Math.abs(parseFloat(pf.internationalProperty) - parseFloat(pf.riskProfile.internationalProperty))).toFixed(2);
                variance.growthAlternatives = (Math.abs(parseFloat(pf.growthAlternatives) - parseFloat(pf.riskProfile.growthAlternatives))).toFixed(2);
                variance.otherGrowth = (Math.abs(parseFloat(pf.otherGrowth) - parseFloat(pf.riskProfile.otherGrowth))).toFixed(2);
                variance.domesticCash = (Math.abs(parseFloat(pf.domesticCash) - parseFloat(pf.riskProfile.domesticCash))).toFixed(2);
                variance.internationalCash = (Math.abs(parseFloat(pf.internationalCash) - parseFloat(pf.riskProfile.internationalCash))).toFixed(2);
                variance.domesticFixedInterest = (Math.abs(parseFloat(pf.domesticFixedInterest) - parseFloat(pf.riskProfile.domesticFixedInterest))).toFixed(2);
                variance.internationalFixedInterest = (Math.abs(parseFloat(pf.internationalFixedInterest) - parseFloat(pf.riskProfile.internationalFixedInterest))).toFixed(2);
                variance.defensiveAlternatives = (Math.abs(parseFloat(pf.defensiveAlternatives) - parseFloat(pf.riskProfile.defensiveAlternatives))).toFixed(2);
                variance.totGrowth = (Math.abs(parseFloat(pf.totGrowth) - parseFloat(pf.riskProfile.growth))).toFixed(2);
                variance.totDefensive = (Math.abs(parseFloat(pf.totDefensive) - parseFloat(pf.riskProfile.defensive))).toFixed(2);
                variance.total = (Math.abs(parseFloat(pf.total) - parseFloat(pf.riskProfile.total))).toFixed(2);

                pf.variance = variance;
            });
    };
    setAllProposedFundAA() {
       
        this.allProposedFund.forEach((pf: any, index: any) => {
            pf.data.forEach((data: any, index: any) => {
                var obj = $.grep(this.allFunds, function (obj: any) {
                    return obj.apircode == data.apircode;
                });
                data.domesticEquity = ((parseFloat(obj[0].domesticEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalEquity = ((parseFloat(obj[0].internationalEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticProperty = ((parseFloat(obj[0].domesticProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalProperty = ((parseFloat(obj[0].internationalProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.growthAlternatives = ((parseFloat(obj[0].growthAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.otherGrowth = ((parseFloat(obj[0].otherGrowth) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticCash = ((parseFloat(obj[0].domesticCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalCash = ((parseFloat(obj[0].internationalCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticFixedInterest = ((parseFloat(obj[0].domesticFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalFixedInterest = ((parseFloat(obj[0].internationalFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.defensiveAlternatives = ((parseFloat(obj[0].defensiveAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.totGrowth = (parseFloat(data.domesticEquity) + parseFloat(data.internationalEquity) + parseFloat(data.domesticProperty) + parseFloat(data.internationalProperty) + parseFloat(data.growthAlternatives) + parseFloat(data.otherGrowth));
                data.totDefensive = (parseFloat(data.domesticCash) + parseFloat(data.internationalCash) + parseFloat(data.domesticFixedInterest) + parseFloat(data.internationalFixedInterest) + parseFloat(data.defensiveAlternatives));
                data.total = (parseFloat(data.totGrowth) + parseFloat(data.totDefensive));
            });

            pf.domesticEquity = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.internationalEquity = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.domesticProperty = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.internationalProperty = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.growthAlternatives = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.otherGrowth = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.domesticCash = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.internationalCash = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.domesticFixedInterest = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticFixedInterest); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.internationalFixedInterest = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.defensiveAlternatives = (pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) > 0 ? (((pf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) / pf.value) * 100).toFixed(2) : 0.00;
            pf.totGrowth = (parseFloat(pf.domesticEquity) + parseFloat(pf.internationalEquity) + parseFloat(pf.domesticProperty) + parseFloat(pf.internationalProperty) + parseFloat(pf.growthAlternatives) + parseFloat(pf.otherGrowth)).toFixed(2);
            pf.totDefensive = (parseFloat(pf.domesticCash) + parseFloat(pf.internationalCash) + parseFloat(pf.domesticFixedInterest) + parseFloat(pf.internationalFixedInterest) + parseFloat(pf.defensiveAlternatives)).toFixed(2);
            pf.total = (parseFloat(pf.totGrowth) + parseFloat(pf.totDefensive)).toFixed(2);

          
        });
  
    };
    setAlternativeFundAA() {
 
            this.alternativeProducts.forEach((af: any, index: any) => {
                af.data.forEach((data: any, index: any) => {
                    var obj = $.grep(this.allFunds, function(obj: any) {
                        return obj.apircode == data.apircode;
                    });
                    data.domesticEquity = ((parseFloat(obj[0].domesticEquity) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalEquity = ((parseFloat(obj[0].internationalEquity) * parseFloat(data.value)) / 100).toFixed(2);
                    data.domesticProperty = ((parseFloat(obj[0].domesticProperty) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalProperty = ((parseFloat(obj[0].internationalProperty) * parseFloat(data.value)) / 100).toFixed(2);
                    data.growthAlternatives = ((parseFloat(obj[0].growthAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                    data.otherGrowth = ((parseFloat(obj[0].otherGrowth) * parseFloat(data.value)) / 100).toFixed(2);
                    data.domesticCash = ((parseFloat(obj[0].domesticCash) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalCash = ((parseFloat(obj[0].internationalCash) * parseFloat(data.value)) / 100).toFixed(2);
                    data.domesticFixedInterest = ((parseFloat(obj[0].domesticFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                    data.internationalFixedInterest = ((parseFloat(obj[0].internationalFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                    data.defensiveAlternatives = ((parseFloat(obj[0].defensiveAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                    data.totGrowth = (parseFloat(data.domesticEquity) + parseFloat(data.internationalEquity) + parseFloat(data.domesticProperty) + parseFloat(data.internationalProperty) + parseFloat(data.growthAlternatives) + parseFloat(data.otherGrowth));
                    data.totDefensive = (parseFloat(data.domesticCash) + parseFloat(data.internationalCash) + parseFloat(data.domesticFixedInterest) + parseFloat(data.internationalFixedInterest) + parseFloat(data.defensiveAlternatives));
                    data.total = (parseFloat(data.totGrowth) + parseFloat(data.totDefensive));
                });

                af.domesticEquity = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.internationalEquity = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.domesticProperty = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.internationalProperty = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.growthAlternatives = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.otherGrowth = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.domesticCash = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.internationalCash = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.domesticFixedInterest = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domestiafixedInterest); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.domestiafixedInterest); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.internationalFixedInterest = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.defensiveAlternatives = (af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) > 0 ? (((af.data.reduce(function(a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
                af.totGrowth = (parseFloat(af.domesticEquity) + parseFloat(af.internationalEquity) + parseFloat(af.domesticProperty) + parseFloat(af.internationalProperty) + parseFloat(af.growthAlternatives) + parseFloat(af.otherGrowth)).toFixed(2);
                af.totDefensive = (parseFloat(af.domesticCash) + parseFloat(af.internationalCash) + parseFloat(af.domesticFixedInterest) + parseFloat(af.internationalFixedInterest) + parseFloat(af.defensiveAlternatives)).toFixed(2);
                af.total = (parseFloat(af.totGrowth) + parseFloat(af.totDefensive)).toFixed(2);

                this.riskProfileSummary = [];
                var t = this;
                this.riskProfiles.forEach(function(rp) {
                    var obj: any = {};
                    obj.RiskProfile = rp.riskProfile1;
                    obj.Growth = rp.growth;
                    obj.Variance = (Math.abs(parseFloat(af.totGrowth) - parseFloat(rp.growth))).toFixed(2);
                    obj.Match = (100 - parseFloat(obj.Variance)).toFixed(2);
                    t.riskProfileSummary.push(obj);
                });


                var res = Math.max.apply(Math, this.riskProfileSummary.map(function(o) { return parseFloat(o.Match) }));
                var maxRP = this.riskProfileSummary.find(function(o) { return o.Match == res });
                var RpObj = this.riskProfiles.find(function(o) { return o.riskProfile1 == maxRP.RiskProfile });
                RpObj.total = parseFloat(RpObj.growth) + parseFloat(RpObj.defensive);
                af.riskProfile = RpObj;

                var variance: any = {};
                variance.domesticEquity = (Math.abs(parseFloat(af.domesticEquity) - parseFloat(af.riskProfile.domesticEquity))).toFixed(2);
                variance.internationalEquity = (Math.abs(parseFloat(af.internationalEquity) - parseFloat(af.riskProfile.internationalEquity))).toFixed(2);
                variance.domesticProperty = (Math.abs(parseFloat(af.domesticProperty) - parseFloat(af.riskProfile.domesticProperty))).toFixed(2);
                variance.internationalProperty = (Math.abs(parseFloat(af.internationalProperty) - parseFloat(af.riskProfile.internationalProperty))).toFixed(2);
                variance.growthAlternatives = (Math.abs(parseFloat(af.growthAlternatives) - parseFloat(af.riskProfile.growthAlternatives))).toFixed(2);
                variance.otherGrowth = (Math.abs(parseFloat(af.otherGrowth) - parseFloat(af.riskProfile.otherGrowth))).toFixed(2);
                variance.domesticCash = (Math.abs(parseFloat(af.domesticCash) - parseFloat(af.riskProfile.domesticCash))).toFixed(2);
                variance.internationalCash = (Math.abs(parseFloat(af.internationalCash) - parseFloat(af.riskProfile.internationalCash))).toFixed(2);
                variance.domesticFixedInterest = (Math.abs(parseFloat(af.domesticFixedInterest) - parseFloat(af.riskProfile.domesticFixedInterest))).toFixed(2);
                variance.internationalFixedInterest = (Math.abs(parseFloat(af.internationalFixedInterest) - parseFloat(af.riskProfile.internationalFixedInterest))).toFixed(2);
                variance.defensiveAlternatives = (Math.abs(parseFloat(af.defensiveAlternatives) - parseFloat(af.riskProfile.defensiveAlternatives))).toFixed(2);
                variance.totGrowth = (Math.abs(parseFloat(af.totGrowth) - parseFloat(af.riskProfile.growth))).toFixed(2);
                variance.totDefensive = (Math.abs(parseFloat(af.totDefensive) - parseFloat(af.riskProfile.defensive))).toFixed(2);
                variance.total = (Math.abs(parseFloat(af.total) - parseFloat(af.riskProfile.total))).toFixed(2);

                af.variance = variance;


            });
            
           
    };
    setAllAlternativeFundAA() {
        this.totalAlternativeProducts.forEach((af: any, index: any) => {
            af.data.forEach((data: any, index: any) => {
                var obj = $.grep(this.allFunds, function (obj: any) {
                    return obj.apircode == data.apircode;
                });
                data.domesticEquity = ((parseFloat(obj[0].domesticEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalEquity = ((parseFloat(obj[0].internationalEquity) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticProperty = ((parseFloat(obj[0].domesticProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalProperty = ((parseFloat(obj[0].internationalProperty) * parseFloat(data.value)) / 100).toFixed(2);
                data.growthAlternatives = ((parseFloat(obj[0].growthAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.otherGrowth = ((parseFloat(obj[0].otherGrowth) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticCash = ((parseFloat(obj[0].domesticCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalCash = ((parseFloat(obj[0].internationalCash) * parseFloat(data.value)) / 100).toFixed(2);
                data.domesticFixedInterest = ((parseFloat(obj[0].domesticFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.internationalFixedInterest = ((parseFloat(obj[0].internationalFixedInterest) * parseFloat(data.value)) / 100).toFixed(2);
                data.defensiveAlternatives = ((parseFloat(obj[0].defensiveAlternatives) * parseFloat(data.value)) / 100).toFixed(2);
                data.totGrowth = (parseFloat(data.domesticEquity) + parseFloat(data.internationalEquity) + parseFloat(data.domesticProperty) + parseFloat(data.internationalProperty) + parseFloat(data.growthAlternatives) + parseFloat(data.otherGrowth));
                data.totDefensive = (parseFloat(data.domesticCash) + parseFloat(data.internationalCash) + parseFloat(data.domesticFixedInterest) + parseFloat(data.internationalFixedInterest) + parseFloat(data.defensiveAlternatives));
                data.total = (parseFloat(data.totGrowth) + parseFloat(data.totDefensive));
            });

            af.domesticEquity = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticEquity); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.internationalEquity = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalEquity); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.domesticProperty = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticProperty); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.internationalProperty = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalProperty); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.growthAlternatives = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.growthAlternatives); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.otherGrowth = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.otherGrowth); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.domesticCash = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domesticCash); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.internationalCash = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalCash); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.domesticFixedInterest = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domestiafixedInterest); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.domestiafixedInterest); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.internationalFixedInterest = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.internationalFixedInterest); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.defensiveAlternatives = (af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) > 0 ? (((af.data.reduce(function (a: any, b: any) { return a + parseFloat(b.defensiveAlternatives); }, 0)) / af.value) * 100).toFixed(2) : 0.00;
            af.totGrowth = (parseFloat(af.domesticEquity) + parseFloat(af.internationalEquity) + parseFloat(af.domesticProperty) + parseFloat(af.internationalProperty) + parseFloat(af.growthAlternatives) + parseFloat(af.otherGrowth)).toFixed(2);
            af.totDefensive = (parseFloat(af.domesticCash) + parseFloat(af.internationalCash) + parseFloat(af.domesticFixedInterest) + parseFloat(af.internationalFixedInterest) + parseFloat(af.defensiveAlternatives)).toFixed(2);
            af.total = (parseFloat(af.totGrowth) + parseFloat(af.totDefensive)).toFixed(2);

        });
     
    };
   
    fundPercentageChange(val: any) {
        var tempTotal = 0;
        this.clientcurrentFunds.forEach((pData: any) => {
            tempTotal += parseFloat(pData.percentage);
        });
        this.totalCurrentPercentage = tempTotal + parseFloat(val);
        if (this.totalCurrentPercentage > 100) {
            this.totalCurrentPercentage = tempTotal;
            alert("Percentage cannot be greater than 100 ");
            return;
        }
        this.clientcurrentFunds.forEach((pData: any) => {
            if (pData.recId == this.selectedcurrentProduct) {
                pData.percentage = this.totalCurrentPercentage;
            }

        });

    }
    generateWordDocument() {
        this.showLoadingIndicator = true;
        this.dropOptionMain = 'block';
        var fileName = this.document[0].clientDetails.familyName.trim() + "," + this.document[0].clientDetails.clientName.trim() + ".docx";
        this.productSwitchingService.generateWord(this.document).subscribe(blob => {
            this.showLoadingIndicator = false;
            this.dropOptionMain = 'none';
            saveAs(blob, fileName);


        });   
    }

    doesContain(arr: any, val: any) {

        var isExist = arr.feeDisplay.filter((item: any) => { return item.name.trim() == val.trim() });
        if (isExist.length >= 0) {
            return true;
        }
        else {
            return false;
        }
     
    }

    getFundName(arr: any) {
        return arr.data[0].fundName;
    }
    getFeeValue(arr: any, val: any) {
        var isExist = arr.feeDisplay.filter((item: any) => { return item.name.trim() == val.trim() });
        if (isExist.length > 0) {
            return Math.round(isExist[0].val);
        }
        else {
            return "0";
        }
    }
    getFeePercentage(arr: any, val: any) {

        var isExist = arr.feeDisplay.filter((item: any) => { return item.name.trim() == val.trim() });
        if (isExist.length > 0) {
            if (isExist[0].percentage == 0 && isExist[0].val == 0) {
                return parseFloat(isExist[0].percentage).toFixed(2) + "%";
            }
            else if (isExist[0].percentage == 0) {
                return "Fixed Fee";           
            }
            else if (isExist[0].percentage != 0) {
                return parseFloat(isExist[0].percentage).toFixed(2) + "%";
            }
        }
        else {
            return "0.00";
        }
    }

    async displayPreview() {
     
        this.document = [];
        var names: any = {};
        var clientCurrentWeights: any = {};
        var partnerCurrentWeights: any = {};
        var jointCurrentWeights: any = {};
        var clientProposedWeights: any = {};
        var partnerProposedWeights: any = {};
        var jointProposedWeights: any = {};
        var clientTargetWeights: any = {};
        var clientTargetMinWeights: any = {};
        var clientTargetMaxWeights: any = {};
        var partnerTargetWeights: any = {};
        var partnerTargetMinWeights: any = {};
        var partnerTargetMaxWeights: any = {};

        var clientWeights: any[] = [];
        var partnerWeights: any[] = [];
        var jointWeights: any[] = [];

        var AA: any = {};
        var test: any[] = [];
       

        var clientCurrentSum = 0;
        var partnerCurrentSum = 0;
        var jointCurrentSum = 0;
        var clientCurrentOriginalSum = 0;
        var partnerCurrentOriginalSum = 0;
        var jointCurrentOriginalSum = 0;
        var clientProposedSum = 0;
        var partnerProposedSum = 0;
        var jointProposedSum = 0;

       
     

        AA.clientCurrent = $.grep(this.allCurrentFund, function (el: any) {
            return el.owner === 'Client';
        });
        clientCurrentSum = AA.clientCurrent.reduce(function (a: any, b: any) { return a + parseFloat(b.value) }, 0);

        AA.partnerCurrent = $.grep(this.allCurrentFund, function (el: any) {
            return el.owner === 'Partner';
        });
        partnerCurrentSum = AA.partnerCurrent.reduce(function (a: any, b: any) { return a + parseFloat(b.value) }, 0);

        AA.jointCurrent = $.grep(this.allCurrentFund, function (el: any) {
            return el.owner === 'Joint';
        });
        jointCurrentSum = AA.jointCurrent.reduce(function (a: any, b: any) { return a + parseFloat(b.value) }, 0);

        AA.clientProposed = $.grep(this.allProposedFund, function (el: any) {
            return el.owner === 'Client';
        });
        clientProposedSum = AA.clientProposed.reduce(function (a: any, b: any) { return a + parseFloat(b.value) }, 0);

        AA.partnerProposed = $.grep(this.allProposedFund, function (el: any) {
            return el.owner === 'Partner';
        });
        partnerProposedSum = AA.partnerProposed.reduce(function (a: any, b: any) { return a + parseFloat(b.value) }, 0);

        AA.jointProposed = $.grep(this.allProposedFund, function (el: any) {
            return el.owner === 'Joint';
        });
        jointProposedSum = AA.jointProposed.reduce(function (a: any, b: any) { return a + parseFloat(b.value) }, 0);

        test.push(AA);


        test.forEach((cf: any, index: any) => { 
            names.defensiveAlternatives = "Defensive Alternatives";
            names.domesticCash = "Domestic Cash";
            names.domesticFixedInterest = "Domestic Fixed Interest";
            names.internationalCash = "International Cash";
            names.internationalFixedInterest = "International Fixed Interest";
            names.totDefensive = "Total Defensive";

            names.growthAlternatives = "Growth Alternatives";
            names.domesticEquity = "Domestic Equity";
            names.domesticProperty = "Domestic Property";
            names.internationalEquity = "International Equity";
            names.internationalProperty = "International Property";
            names.otherGrowth = "Other Growth";
            names.totGrowth = "Total Growth";

            var $t = this;
            var clientRiskProfile = $.grep(this.riskProfiles, function (el: any) {
                return el.riskProfile1.trim() === $t.clientDetails.clientRiskProfile.trim();
            });

            var partnerRiskProfile = $.grep(this.riskProfiles, function (el: any) {
                return el.riskProfile1.trim() === $t.clientDetails.partnerRiskProfile.trim();
            });

            var jointRiskProfile = $.grep(this.riskProfiles, function (el: any) {
                return el.riskProfile1.trim() === $t.clientDetails.jointRiskProfile.trim();
            });

            clientCurrentWeights.domesticEquity = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.internationalEquity = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.domesticProperty = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.internationalProperty = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.growthAlternatives = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.otherGrowth = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.domesticCash = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.internationalCash = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.domesticFixedInterest = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.internationalFixedInterest = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.defensiveAlternatives = (cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) / clientCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            clientCurrentWeights.totGrowth = (parseFloat(clientCurrentWeights.domesticEquity) + parseFloat(clientCurrentWeights.internationalEquity) + parseFloat(clientCurrentWeights.domesticProperty) + parseFloat(clientCurrentWeights.internationalProperty) + parseFloat(clientCurrentWeights.growthAlternatives) + parseFloat(clientCurrentWeights.otherGrowth)).toFixed(2);
            clientCurrentWeights.totDefensive = (parseFloat(clientCurrentWeights.domesticCash) + parseFloat(clientCurrentWeights.internationalCash) + parseFloat(clientCurrentWeights.domesticFixedInterest) + parseFloat(clientCurrentWeights.internationalFixedInterest) + parseFloat(clientCurrentWeights.defensiveAlternatives)).toFixed(2);

            clientProposedWeights.domesticEquity = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.internationalEquity = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.domesticProperty = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.internationalProperty = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.growthAlternatives = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.otherGrowth = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.domesticCash = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.internationalCash = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.domesticFixedInterest = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.internationalFixedInterest = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.defensiveAlternatives = (cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.clientProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) / clientProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            clientProposedWeights.totGrowth = (parseFloat(clientProposedWeights.domesticEquity) + parseFloat(clientProposedWeights.internationalEquity) + parseFloat(clientProposedWeights.domesticProperty) + parseFloat(clientProposedWeights.internationalProperty) + parseFloat(clientProposedWeights.growthAlternatives) + parseFloat(clientProposedWeights.otherGrowth)).toFixed(2);
            clientProposedWeights.totDefensive = (parseFloat(clientProposedWeights.domesticCash) + parseFloat(clientProposedWeights.internationalCash) + parseFloat(clientProposedWeights.domesticFixedInterest) + parseFloat(clientProposedWeights.internationalFixedInterest) + parseFloat(clientProposedWeights.defensiveAlternatives)).toFixed(2);

            partnerCurrentWeights.domesticEquity = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.internationalEquity = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.domesticProperty = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.internationalProperty = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.growthAlternatives = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.otherGrowth = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.domesticCash = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.internationalCash = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.domesticFixedInterest = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.internationalFixedInterest = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.defensiveAlternatives = (cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) / partnerCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerCurrentWeights.totGrowth = (parseFloat(partnerCurrentWeights.domesticEquity) + parseFloat(partnerCurrentWeights.internationalEquity) + parseFloat(partnerCurrentWeights.domesticProperty) + parseFloat(partnerCurrentWeights.internationalProperty) + parseFloat(partnerCurrentWeights.growthAlternatives) + parseFloat(partnerCurrentWeights.otherGrowth)).toFixed(2);
            partnerCurrentWeights.totDefensive = (parseFloat(partnerCurrentWeights.domesticCash) + parseFloat(partnerCurrentWeights.internationalCash) + parseFloat(partnerCurrentWeights.domesticFixedInterest) + parseFloat(partnerCurrentWeights.internationalFixedInterest) + parseFloat(partnerCurrentWeights.defensiveAlternatives)).toFixed(2);

            partnerProposedWeights.domesticEquity = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.internationalEquity = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.domesticProperty = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.internationalProperty = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.growthAlternatives = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.otherGrowth = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.domesticCash = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.internationalCash = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.domesticFixedInterest = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.internationalFixedInterest = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.defensiveAlternatives = (cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.partnerProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) / partnerProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            partnerProposedWeights.totGrowth = (parseFloat(partnerProposedWeights.domesticEquity) + parseFloat(partnerProposedWeights.internationalEquity) + parseFloat(partnerProposedWeights.domesticProperty) + parseFloat(partnerProposedWeights.internationalProperty) + parseFloat(partnerProposedWeights.growthAlternatives) + parseFloat(partnerProposedWeights.otherGrowth)).toFixed(2);
            partnerProposedWeights.totDefensive = (parseFloat(partnerProposedWeights.domesticCash) + parseFloat(partnerProposedWeights.internationalCash) + parseFloat(partnerProposedWeights.domesticFixedInterest) + parseFloat(partnerProposedWeights.internationalFixedInterest) + parseFloat(partnerProposedWeights.defensiveAlternatives)).toFixed(2);

            jointCurrentWeights.domesticEquity = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.internationalEquity = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.domesticProperty = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.internationalProperty = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.growthAlternatives = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.otherGrowth = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.domesticCash = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.internationalCash = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.domesticFixedInterest = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.internationalFixedInterest = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.defensiveAlternatives = (cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointCurrent.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) / jointCurrentSum) * 100).toFixed(2) : (0).toFixed(2);
            jointCurrentWeights.totGrowth = (parseFloat(jointCurrentWeights.domesticEquity) + parseFloat(jointCurrentWeights.internationalEquity) + parseFloat(jointCurrentWeights.domesticProperty) + parseFloat(jointCurrentWeights.internationalProperty) + parseFloat(jointCurrentWeights.growthAlternatives) + parseFloat(jointCurrentWeights.otherGrowth)).toFixed(2);
            jointCurrentWeights.totDefensive = (parseFloat(jointCurrentWeights.domesticCash) + parseFloat(jointCurrentWeights.internationalCash) + parseFloat(jointCurrentWeights.domesticFixedInterest) + parseFloat(jointCurrentWeights.internationalFixedInterest) + parseFloat(jointCurrentWeights.defensiveAlternatives)).toFixed(2);


            jointProposedWeights.domesticEquity = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticEquity) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.internationalEquity = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalEquity) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.domesticProperty = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticProperty) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.internationalProperty = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalProperty) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.growthAlternatives = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.growthAlternatives) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.otherGrowth = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.otherGrowth) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.domesticCash = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticCash) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.internationalCash = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalCash) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.domesticFixedInterest = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.domesticFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.internationalFixedInterest = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.internationalFixedInterest) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.defensiveAlternatives = (cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) > 0 ? (((cf.jointProposed.reduce(function (a: any, b: any) { return a + ((parseFloat(b.defensiveAlternatives) * parseFloat(b.value)) / 100); }, 0)) / jointProposedSum) * 100).toFixed(2) : (0).toFixed(2);
            jointProposedWeights.totGrowth = (parseFloat(jointProposedWeights.domesticEquity) + parseFloat(jointProposedWeights.internationalEquity) + parseFloat(jointProposedWeights.domesticProperty) + parseFloat(jointProposedWeights.internationalProperty) + parseFloat(jointProposedWeights.growthAlternatives) + parseFloat(jointProposedWeights.otherGrowth)).toFixed(2);
            jointProposedWeights.totDefensive = (parseFloat(jointProposedWeights.domesticCash) + parseFloat(jointProposedWeights.internationalCash) + parseFloat(jointProposedWeights.domesticFixedInterest) + parseFloat(jointProposedWeights.internationalFixedInterest) + parseFloat(jointProposedWeights.defensiveAlternatives)).toFixed(2);


            if (clientRiskProfile.length > 0) {
                clientRiskProfile[0].totGrowth = parseFloat(clientRiskProfile[0].growth).toFixed(2);
                clientRiskProfile[0].totDefensive = parseFloat(clientRiskProfile[0].defensive).toFixed(2);
                clientRiskProfile[0].totGrowthMin = parseFloat(clientRiskProfile[0].growthMin).toFixed(2);
                clientRiskProfile[0].totDefensiveMin = parseFloat(clientRiskProfile[0].defensiveMin).toFixed(2);
                clientRiskProfile[0].totGrowthMax = parseFloat(clientRiskProfile[0].growthMax).toFixed(2);
                clientRiskProfile[0].totDefensiveMax = parseFloat(clientRiskProfile[0].defensiveMax).toFixed(2);
            }

            if (partnerRiskProfile.length > 0) {
                partnerRiskProfile[0].totGrowth = parseFloat(partnerRiskProfile[0].growth).toFixed(2);
                partnerRiskProfile[0].totDefensive = parseFloat(partnerRiskProfile[0].defensive).toFixed(2);
                partnerRiskProfile[0].totGrowthMin = parseFloat(partnerRiskProfile[0].growthMin).toFixed(2);
                partnerRiskProfile[0].totDefensiveMin = parseFloat(partnerRiskProfile[0].defensiveMin).toFixed(2);
                partnerRiskProfile[0].totGrowthMax = parseFloat(partnerRiskProfile[0].growthMax).toFixed(2);
                partnerRiskProfile[0].totDefensiveMax = parseFloat(partnerRiskProfile[0].defensiveMax).toFixed(2);
            }

            if (cf.clientCurrent.length > 0 || cf.clientProposed.length) {
                for (var key in names) {
                    var obj: any = {};
                    obj.name = names[key];
                    obj.current = clientCurrentWeights[key];
                    obj.proposed = clientProposedWeights[key];
                    obj.target = (parseFloat(clientRiskProfile[0][key])).toFixed(2);
                    obj.targetmin = (parseFloat(clientRiskProfile[0][key + "Min"])).toFixed(2);
                    obj.targetmax = (parseFloat(clientRiskProfile[0][key + "Max"])).toFixed(2);
                    clientWeights.push(obj);
                }
            }
            if (cf.partnerCurrent.length > 0 || cf.partnerProposed.length) {
                for (var key in names) {
                    var obj: any = {};
                    obj.name = names[key];
                    obj.current = partnerCurrentWeights[key];
                    obj.proposed = partnerProposedWeights[key];
                    obj.target = (parseFloat(partnerRiskProfile[0][key])).toFixed(2);
                    obj.targetmin = (parseFloat(partnerRiskProfile[0][key + "Min"])).toFixed(2);
                    obj.targetmax = (parseFloat(partnerRiskProfile[0][key + "Max"])).toFixed(2);
                    partnerWeights.push(obj);
                }
            }
            if (cf.jointCurrent.length > 0 || cf.jointProposed.length) {
                for (var key in names) {
                    var obj: any = {};
                    obj.name = names[key];
                    obj.current = jointCurrentWeights[key];
                    obj.proposed = jointProposedWeights[key];
                    obj.target = 0;
                    obj.targetmin = 0;
                    obj.targetmax = 0;
                    jointWeights.push(obj);
                }
            }
        });


        this.proposedAssetAllocation.clientWeights = clientWeights;
        this.proposedAssetAllocation.partnerWeights = partnerWeights;
        this.proposedAssetAllocation.jointWeights = jointWeights;
        this.proposedAssetAllocation.clientRiskProfile = this.clientDetails.clientRiskProfile;
        this.proposedAssetAllocation.partnerRiskProfile = this.clientDetails.partnerRiskProfile;
        this.proposedAssetAllocation.jointRiskProfile = this.clientDetails.jointRiskProfile;

        this.proposedAssetAllocation.clientDetails = this.clientDetails;
        this.proposedAssetAllocation.cashFlowIncome = this.cashFlowIncome;
        this.proposedAssetAllocation.cashFlowExpenses = this.cashFlowExpenses;
        this.proposedAssetAllocation.lifestyleAssets = this.lifestyleAssets;
        this.proposedAssetAllocation.currentOriginalAssests = this.currentInvestments;
        this.proposedAssetAllocation.currentAssests = this.allCurrentFund;
        this.proposedAssetAllocation.proposedAssets = this.allProposedFund;
        this.proposedAssetAllocation.alternativeAssets = this.totalAlternativeProducts;
        this.proposedAssetAllocation.liabilities = this.liabilities;

        this.document.push(this.proposedAssetAllocation);

        //Preview

        this.clientCurrentFund = $.grep(this.allCurrentFund, function (el: any) {
            return el.owner === 'Client';
        });

        this.partnerCurrentFund = $.grep(this.allCurrentFund, function (el: any) {
            return el.owner === 'Partner';
        });

        this.jointCurrentFund = $.grep(this.allCurrentFund, function (el: any) {
            return el.owner === 'Joint';
        });

        this.clientProposedFund = $.grep(this.allProposedFund, function (el: any) {
            return el.owner === 'Client';
        });


        this.partnerProposedFund = $.grep(this.allProposedFund, function (el: any) {
            return el.owner === 'Partner';
        });

        this.jointProposedFund = $.grep(this.allProposedFund, function (el: any) {
            return el.owner === 'Joint';
        });

        //Original

        this.clientCurrentOriginalFund = $.grep(this.currentInvestments, function (el: any) {
            return el.owner === 'Client';
        });

        this.partnerCurrentOriginalFund = $.grep(this.currentInvestments, function (el: any) {
            return el.owner === 'Partner';
        });

        this.jointCurrentOriginalFund = $.grep(this.currentInvestments, function (el: any) {
            return el.owner === 'Joint';
        });

        //Recommended Portfolio


        //client
        if (this.clientCurrentOriginalFund.length > 0 || this.clientProposedFund.length > 0) {

            var newClient: any[] = [];
            var newClientTotal: any[] = [];

            var allItems = this.clientCurrentOriginalFund.map(a => a.productId).concat(this.clientProposedFund.map(a => a.productId));
            var unique = allItems.filter(function (elem, index, self) {
                return index === self.indexOf(elem);
            });

            var t = this;
            var currentTotal = 0;
            var proposedTotal = 0;

          

            unique.forEach(function (obj) {
                var newObj: any = {};

                var current: any = t.clientCurrentOriginalFund.filter(item => { return item.productId == obj });
                var proposed: any = t.clientProposedFund.filter(item => { return item.productId == obj });

               

                if (current != undefined && current.length > 0) {
                    newObj.product = current[0].product;
                }
                else {
                    if (proposed != undefined && proposed.length > 0) {
                        newObj.product = proposed[0].product;
                    }

                }



                var commonFund: any[] = [];
                if ((Array.isArray(current) && current.length > 0) || (Array.isArray(proposed) && proposed.length > 0)) {
                    if ((Array.isArray(current) && current.length > 0) && (Array.isArray(proposed) && proposed.length > 0)) {
                        var allAPIR = current[0].data.map((a: any) => a.apircode).concat(proposed[0].data.map((a: any) => a.apircode));
                        commonFund = allAPIR.filter(function (elem: any, index: any, self: any) {
                            return index === self.indexOf(elem);
                        });

                    }
                    else if ((Array.isArray(proposed) && proposed.length > 0)) {
                        commonFund = proposed[0].data.map((a: any) => a.apircode);
                    }
                    else {
                        if (current[0].data) {
                            commonFund = current[0].data.map((a: any) => a.apircode);
                        }
                    }



                    var data: any[] = [];
                    var subTotal: any[] = [];
                    var sub: any = {};

                    commonFund.forEach(function (obj1) {

                        if (current == undefined) {
                            current = [];
                        }
                        if (proposed == undefined) {
                            proposed = [];
                        }

                        var currentFund: any[] = [];
                        var proposedFund: any[] = [];
                        if ((Array.isArray(current) && current.length > 0)) {
                            currentFund = current[0].data.filter((r: any) => { return r.apircode == obj1 });
                        }
                        if ((Array.isArray(proposed) && proposed.length > 0)) {
                            proposedFund = proposed[0].data.filter((r: any) => { return r.apircode == obj1 });
                        }
                        var fundData: any = {};
                        if ((Array.isArray(currentFund) && currentFund.length > 0)) {
                            fundData.fundName = currentFund[0].fundName;
                            fundData.current = currentFund[0].value;
                            fundData.proposed = (proposedFund.length <= 0) ? 0 : proposedFund[0].value;
                            fundData.change = ((proposedFund.length <= 0) ? 0 : proposedFund[0].value) - currentFund[0].value;
                            data.push(fundData);
                        }
                        else {
                            fundData.fundName = proposedFund[0].fundName;
                            fundData.proposed = proposedFund[0].value;
                            fundData.current = (currentFund.length <= 0) ? 0 : currentFund[0].value;
                            fundData.change = proposedFund[0].value - ((currentFund.length <= 0) ? 0 : currentFund[0].value);
                            data.push(fundData);
                        }
                    });


                    //SubTotal
                    sub.fundName = "Sub Total";
                   
                        sub.current = (current != undefined && current.length > 0 && current[0].data) ? (current[0].data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0)) : 0;
                   
                        sub.proposed = (proposed != undefined && proposed.length > 0 && proposed[0].data) ? (proposed[0].data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0)) : 0;
                  

                    sub.change = sub.proposed - sub.current;
                    subTotal.push(sub);

                    currentTotal += sub.current;
                    proposedTotal += sub.proposed;

                    newObj.fundData = data;
                    newObj.subTotal = subTotal;


                }
                newClient.push(newObj);
            });


            if (newClient.length > 0) {
                var newObj: any = {};

                newObj.proposed = proposedTotal;
                newObj.current = currentTotal;
                newObj.change = proposedTotal - currentTotal;
                newClientTotal.push(newObj);
            }


            this.commonClient = newClient;
            this.totalClient = newClientTotal;
        }
        //partner
        if (this.partnerCurrentOriginalFund.length > 0 || this.partnerProposedFund.length > 0) {

            var newPartner: any[] = [];
            var newPartnerTotal: any[] = [];

            var allItems = this.partnerCurrentOriginalFund.map(a => a.productId).concat(this.partnerProposedFund.map(a => a.productId));
            var unique = allItems.filter(function (elem, index, self) {
                return index === self.indexOf(elem);
            });

            var t = this;
            var currentTotal = 0;
            var proposedTotal = 0;
            unique.forEach(function (obj) {
                var newObj: any = {};

                var current: any = t.partnerCurrentOriginalFund.filter(item => { return item.productId == obj });
                var proposed: any = t.partnerProposedFund.filter(item => { return item.productId == obj });

                if (current != undefined && current.length > 0) {
                    newObj.product = current[0].product;
                }
                else {
                    if (proposed != undefined && proposed.length > 0) {
                        newObj.product = proposed[0].product;
                    }

                }



                var commonFund: any[] = [];
                if ((Array.isArray(current) && current.length > 0) || (Array.isArray(proposed) && proposed.length > 0)) {
                    if ((Array.isArray(current) && current.length > 0) && (Array.isArray(proposed) && proposed.length > 0)) {
                        var allAPIR = current[0].data.map((a: any) => a.apircode).concat(proposed[0].data.map((a: any) => a.apircode));
                        commonFund = allAPIR.filter(function (elem: any, index: any, self: any) {
                            return index === self.indexOf(elem);
                        });

                    }
                    else if ((Array.isArray(proposed) && proposed.length > 0)) {
                        commonFund = proposed[0].data.map((a: any) => a.apircode);
                    }
                    else {
                        commonFund = current[0].data.map((a: any) => a.apircode);
                    }



                    var data: any[] = [];
                    var subTotal: any[] = [];
                    var sub: any = {};

                    commonFund.forEach(function (obj1) {

                        if (current == undefined) {
                            current = [];
                        }
                        if (proposed == undefined) {
                            proposed = [];
                        }

                        var currentFund: any[] = [];
                        var proposedFund: any[] = [];
                        if ((Array.isArray(current) && current.length > 0)) {
                            currentFund = current[0].data.filter((r: any) => { return r.apircode == obj1 });
                        }
                        if ((Array.isArray(proposed) && proposed.length > 0)) {
                            proposedFund = proposed[0].data.filter((r: any) => { return r.apircode == obj1 });
                        }
                        var fundData: any = {};
                        if ((Array.isArray(currentFund) && currentFund.length > 0)) {
                            fundData.fundName = currentFund[0].fundName;
                            fundData.current = currentFund[0].value;
                            fundData.proposed = (proposedFund.length <= 0) ? 0 : proposedFund[0].value;
                            fundData.change = ((proposedFund.length <= 0) ? 0 : proposedFund[0].value) - currentFund[0].value;
                            data.push(fundData);
                        }
                        else {
                            fundData.fundName = proposedFund[0].fundName;
                            fundData.proposed = proposedFund[0].value;
                            fundData.current = (currentFund.length <= 0) ? 0 : currentFund[0].value;
                            fundData.change = proposedFund[0].value - ((currentFund.length <= 0) ? 0 : currentFund[0].value);
                            data.push(fundData);
                        }
                    });


                    //SubTotal
                    sub.fundName = "Sub Total";
                    sub.current = (current != undefined && current.length > 0) ? (current[0].data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0)) : 0;
                    sub.proposed = (proposed != undefined && proposed.length > 0) ? (proposed[0].data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0)) : 0;
                    sub.change = sub.proposed - sub.current;
                    subTotal.push(sub);

                    currentTotal += sub.current;
                    proposedTotal += sub.proposed;

                    newObj.fundData = data;
                    newObj.subTotal = subTotal;


                }
                newPartner.push(newObj);
            });


            if (newPartner.length > 0) {
                var newObj: any = {};

                newObj.proposed = proposedTotal;
                newObj.current = currentTotal;
                newObj.change = proposedTotal - currentTotal;
                newPartnerTotal.push(newObj);
            }


            this.commonPartner = newPartner;
            this.totalPartner = newPartnerTotal;
        }
        //joint
        if (this.jointCurrentOriginalFund.length > 0 || this.jointProposedFund.length > 0) {

            var newJoint: any[] = [];
            var newJointTotal: any[] = [];

            var allItems = this.jointCurrentOriginalFund.map(a => a.productId).concat(this.jointProposedFund.map(a => a.productId));
            var unique = allItems.filter(function (elem, index, self) {
                return index === self.indexOf(elem);
            });

            var t = this;
            var currentTotal = 0;
            var proposedTotal = 0;
            unique.forEach(function (obj) {
                var newObj: any = {};

                var current: any = t.jointCurrentOriginalFund.filter(item => { return item.productId == obj });
                var proposed: any = t.jointProposedFund.filter(item => { return item.productId == obj });

                if (current != undefined && current.length > 0) {
                    newObj.product = current[0].product;
                }
                else {
                    if (proposed != undefined && proposed.length > 0) {
                        newObj.product = proposed[0].product;
                    }

                }



                var commonFund: any[] = [];
                if ((Array.isArray(current) && current.length > 0) || (Array.isArray(proposed) && proposed.length > 0)) {
                    if ((Array.isArray(current) && current.length > 0) && (Array.isArray(proposed) && proposed.length > 0)) {
                        var allAPIR = current[0].data.map((a: any) => a.apircode).concat(proposed[0].data.map((a: any) => a.apircode));
                        commonFund = allAPIR.filter(function (elem: any, index: any, self: any) {
                            return index === self.indexOf(elem);
                        });

                    }
                    else if ((Array.isArray(proposed) && proposed.length > 0)) {
                        commonFund = proposed[0].data.map((a: any) => a.apircode);
                    }
                    else {
                        commonFund = current[0].data.map((a: any) => a.apircode);
                    }



                    var data: any[] = [];
                    var subTotal: any[] = [];
                    var sub: any = {};

                    commonFund.forEach(function (obj1) {

                        if (current == undefined) {
                            current = [];
                        }
                        if (proposed == undefined) {
                            proposed = [];
                        }

                        var currentFund: any[] = [];
                        var proposedFund: any[] = [];
                        if ((Array.isArray(current) && current.length > 0)) {
                            currentFund = current[0].data.filter((r: any) => { return r.apircode == obj1 });
                        }
                        if ((Array.isArray(proposed) && proposed.length > 0)) {
                            proposedFund = proposed[0].data.filter((r: any) => { return r.apircode == obj1 });
                        }
                        var fundData: any = {};
                        if ((Array.isArray(currentFund) && currentFund.length > 0)) {
                            fundData.fundName = currentFund[0].fundName;
                            fundData.current = currentFund[0].value;
                            fundData.proposed = (proposedFund.length <= 0) ? 0 : proposedFund[0].value;
                            fundData.change = ((proposedFund.length <= 0) ? 0 : proposedFund[0].value) - currentFund[0].value;
                            data.push(fundData);
                        }
                        else {
                            fundData.fundName = proposedFund[0].fundName;
                            fundData.proposed = proposedFund[0].value;
                            fundData.current = (currentFund.length <= 0) ? 0 : currentFund[0].value;
                            fundData.change = proposedFund[0].value - ((currentFund.length <= 0) ? 0 : currentFund[0].value);
                            data.push(fundData);
                        }
                    });


                    //SubTotal
                    sub.fundName = "Sub Total";
                    sub.current = (current != undefined && current.length > 0) ? (current[0].data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0)) : 0;
                    sub.proposed = (proposed != undefined && proposed.length > 0) ? (proposed[0].data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0)) : 0;
                    sub.change = sub.proposed - sub.current;
                    subTotal.push(sub);

                    currentTotal += sub.current;
                    proposedTotal += sub.proposed;

                    newObj.fundData = data;
                    newObj.subTotal = subTotal;


                }
                newJoint.push(newObj);
            });


            if (newJoint.length > 0) {
                var newObj: any = {};

                newObj.proposed = proposedTotal;
                newObj.current = currentTotal;
                newObj.change = proposedTotal - currentTotal;
                newJointTotal.push(newObj);
            }


            this.commonJoint = newJoint;
            this.totalJoint = newJointTotal;
        }


        ////Replacement of Product

        ////client
        if (this.clientProposedFund != undefined && this.clientProposedFund.length > 0) {
          
            var proposedProducts = this.clientProposedFund;
            var t = this;
            for (let product of proposedProducts) {
                var feeDisplay: any[] = [];
                var tempProposedProducts: any[] = [];
                var existingProducts: any[] = [];
                var alternativeProducts: any[] = [];

                tempProposedProducts.push(product);

                var alternative = t.totalAlternativeProducts.filter(item => { return (item.proposedId == product.id) });
                alternative.forEach(function (alt) {
                    alternativeProducts.push(alt);
                });
                   
                var fund = t.allCurrentFund.filter(item => { return (item.proposedId == product.id) });
                   
                    fund.forEach(function (fnd) {
                        existingProducts.push(fnd);
                    });


                var isExisting = existingProducts.length > 0 ? existingProducts.filter(item => { return (item.owner != "Client") }) : [];
                var isAlternate = alternativeProducts.length > 0 ? alternativeProducts.filter(item => { return (item.owner != "Client") }) : [];
                var notClient: boolean = false;
                if (isExisting.length > 0 || isAlternate.length > 0) {
                    notClient = true;
                }


                if (product != null || existingProducts.length > 0 || alternativeProducts.length > 0) {

                    var newObj: any = {};

                    existingProducts.forEach(function (obj: any) {
                        var sum = 0;
                        obj.feeDisplay.forEach(function (obj1: any) {
                            if ((obj1.val != 0 || obj1.percentage != 0) && obj1.feeType.trim() == "ongoing") {
                                var isExist = feeDisplay.indexOf(obj1.name.trim());
                                if (isExist < 0) {
                                    feeDisplay.push(obj1.name.trim());

                                }
                                sum += parseFloat(obj1.val);
                            }
                           
                        });

                        if (notClient) {
                            var name = "";
                            if (obj.owner == "Client") {
                                name = t.clientDetails.clientName;
                            }
                            else if (obj.owner == "Partner") {
                                name = t.clientDetails.partnerName;
                            }
                            else {
                                name = "Joint";
                            }

                            obj.displayName = obj.product + " - " + name;

                        }
                        else {
                            obj.displayName = obj.product;
                        }

                        obj.lessTotalRebates = 0;
                        obj.total = Math.round(sum);

                    });

                    for (let obj of tempProposedProducts) {

                       
                        var sum = 0;
                        obj.feeDisplay.forEach(function (obj1: any) {
                            if ((obj1.val != 0 || obj1.percentage != 0) && obj1.feeType.trim() == "ongoing") {
                                var isExist = feeDisplay.indexOf(obj1.name.trim());
                                if (isExist < 0) {
                                    feeDisplay.push(obj1.name.trim());

                                }
                                sum += parseFloat(obj1.val);
                            }
                           
                        });

                        if (notClient) {
                            var name = "";
                            if (obj.owner == "Client") {
                                name = t.clientDetails.clientName;
                            }
                            else if (obj.owner == "Partner") {
                                name = t.clientDetails.partnerName;
                            }
                            else {
                                name = "Joint";
                            }

                            obj.displayName = obj.product + " - " + name;

                        }
                        else {
                            obj.displayName = obj.product;
                        }

                        obj.lessTotalRebates = 0;
                        obj.total = Math.round(sum);


                        var sumOfDiff = 0;
                       
                        //var rep = t.allProductReplacement.filter(item => { return (item.proposedId == obj.id) });

                        //rep.forEach(function (d: any) {
                        var fund = t.allCurrentFund.filter(item => { return (item.proposedId == obj.id) });
                        if (fund.length > 0) {
                            sumOfDiff += parseFloat(fund[0].feeDisplay.reduce(function (a: any, b: any) { return a + parseFloat(b.val); }, 0));
                        }
                            //});


                        var display = "";
                        var value = Math.round(sum) - Math.round(sumOfDiff);
                        if (value > 0) {
                            display = " Additional $" + Math.abs(value);
                        }
                        else if (value < 0) {
                            display = " Less $" + Math.abs(value);
                        }
                        else {
                            display = "$0";
                        }

                        obj.difference = display;

                    }

                    tempProposedProducts.forEach(function (obj: any) {

                        var alt = alternativeProducts.filter(item => { return (item.proposedId == obj.id) });
                        alt.forEach(function (obj1: any) {
                            var sum = 0;
                            obj1.feeDisplay.forEach(function (obj2: any) {
                                if ((obj2.val != 0 || obj2.percentage != 0) && obj2.feeType.trim() == "ongoing") {
                                    var isExist = feeDisplay.indexOf(obj2.name.trim());
                                    if (isExist < 0) {
                                        feeDisplay.push(obj2.name.trim());

                                    }
                                    sum += parseFloat(obj2.val);
                                }
                               
                            });

                            if (notClient) {
                                var name = "";
                                if (obj1.owner == "Client") {
                                    name = t.clientDetails.clientName;
                                }
                                else if (obj1.owner == "Partner") {
                                    name = t.clientDetails.partnerName;
                                }
                                else {
                                    name = "Joint";
                                }

                                obj1.displayName = obj1.product + " - " + name;

                            }
                            else {
                                obj1.displayName = obj1.product;
                            }
                            obj1.lessTotalRebates = 0;
                            obj1.total = Math.round(sum);

                            var sumOfDiff = 0;
                            //var rep = t.allProductReplacement.filter(item => { return (item.proposedId == obj1.proposedId) });

                            //rep.forEach(function (d: any) {
                            var fund = t.allCurrentFund.filter(item => { return (item.proposedId == obj1.proposedId) });
                            if (fund.length > 0) {
                                sumOfDiff += parseFloat(fund[0].feeDisplay.reduce(function (a: any, b: any) { return a + parseFloat(b.val); }, 0));
                            }
                                //});


                            var display = "";
                            var value = Math.round(sum) - Math.round(sumOfDiff);
                            if (value > 0) {
                                display = " Additional $" + Math.abs(value);
                            }
                            else if (value < 0) {
                                display = " Less $" + Math.abs(value);
                            }
                            else {
                                display = "$0";
                            }

                            obj1.difference = display;
                        });
                    });

                    newObj.proposedProducts = tempProposedProducts;
                    newObj.existingProducts = existingProducts;
                    newObj.alternativeProducts = alternativeProducts;
                    newObj.feeDisplay = feeDisplay;

                    if (newObj.existingProducts.length > 0 || newObj.alternativeProducts.length > 0) {
                        this.clientProductReplacement.push(newObj);
                    }
                }

            }   

        }
        ////partner
        if (this.partnerProposedFund != undefined && this.partnerProposedFund.length > 0) {
            var proposedProducts = this.partnerProposedFund;
         
            var t = this;
            for (let product of proposedProducts) {

                var feeDisplay: any[] = [];
                var tempProposedProducts: any[] = [];
                var existingProducts: any[] = [];
                var alternativeProducts: any[] = [];

                tempProposedProducts.push(product);

                var alternative = t.totalAlternativeProducts.filter(item => { return (item.proposedId == product.id) });
                alternative.forEach(function (alt) {
                    alternativeProducts.push(alt);
                });
            
                var fund = t.allCurrentFund.filter(item => { return (item.proposedId == product.id) });

                    fund.forEach(function (fnd) {
                        existingProducts.push(fnd);
                    });

            

            var isExisting = existingProducts.length > 0 ? existingProducts.filter(item => { return (item.owner != "Partner") }) : [];
            var isAlternate = alternativeProducts.length > 0 ? alternativeProducts.filter(item => { return (item.owner != "Partner") }) : [];
            var notClient: boolean = false;
            if (isExisting.length > 0 || isAlternate.length > 0) {
                notClient = true;
            }

                if (tempProposedProducts.length > 0 || existingProducts.length > 0 || alternativeProducts.length > 0) {

                    var newObj: any = {};

                    existingProducts.forEach(function (obj: any) {
                        var sum = 0;
                        obj.feeDisplay.forEach(function (obj1: any) {
                            if ((obj1.val != 0 || obj1.percentage != 0) && obj1.feeType.trim() == "ongoing") {
                                var isExist = feeDisplay.indexOf(obj1.name.trim());
                                if (isExist < 0) {
                                    feeDisplay.push(obj1.name.trim());

                                }
                                sum += parseFloat(obj1.val);
                            }
                           
                        });

                        if (notClient) {
                            var name = "";
                            if (obj.owner == "Client") {
                                name = t.clientDetails.clientName;
                            }
                            else if (obj.owner == "Partner") {
                                name = t.clientDetails.partnerName;
                            }
                            else {
                                name = "Joint";
                            }

                            obj.displayName = obj.product + " - " + name;

                        }
                        else {
                            obj.displayName = obj.product;
                        }

                        obj.lessTotalRebates = 0;
                        obj.total = Math.round(sum);

                    });

                    //  proposedProducts.forEach(function (obj: any) {
                    for (let obj of tempProposedProducts) {
                        var sum = 0;
                        obj.feeDisplay.forEach(function (obj1: any) {
                            if ((obj1.val != 0 || obj1.percentage != 0) && obj1.feeType.trim() == "ongoing") {
                                var isExist = feeDisplay.indexOf(obj1.name.trim());
                                if (isExist < 0) {
                                    feeDisplay.push(obj1.name.trim());

                                }
                                sum += parseFloat(obj1.val);
                            }
                           
                        });

                        if (notClient) {
                            var name = "";
                            if (obj.owner == "Client") {
                                name = t.clientDetails.clientName;
                            }
                            else if (obj.owner == "Partner") {
                                name = t.clientDetails.partnerName;
                            }
                            else {
                                name = "Joint";
                            }

                            obj.displayName = obj.product + " - " + name;

                        }
                        else {
                            obj.displayName = obj.product;
                        }

                        obj.lessTotalRebates = 0;
                        obj.total = Math.round(sum);


                        var sumOfDiff = 0;
                        
                        var fund = t.allCurrentFund.filter(item => { return (item.proposedId == obj.id) });
                        if (fund.length > 0) {
                            sumOfDiff += parseFloat(fund[0].feeDisplay.reduce(function (a: any, b: any) { return a + parseFloat(b.val); }, 0));
                        }


                        var display = "";
                        var value = Math.round(sum) - Math.round(sumOfDiff);
                        if (value > 0) {
                            display = " Additional $" + Math.abs(value);
                        }
                        else if (value < 0) {
                            display = " Less $" + Math.abs(value);
                        }
                        else {
                            display = "$0";
                        }

                        obj.difference = display;

                    }

                    tempProposedProducts.forEach(function (obj: any) {

                        var alt = alternativeProducts.filter(item => { return (item.proposedId == obj.id) });
                        alt.forEach(function (obj1: any) {
                            var sum = 0;
                            obj1.feeDisplay.forEach(function (obj2: any) {
                                if ((obj2.val != 0 || obj2.percentage != 0) && obj2.feeType.trim() == "ongoing") {
                                    var isExist = feeDisplay.indexOf(obj2.name.trim());
                                    if (isExist < 0) {
                                        feeDisplay.push(obj2.name.trim());

                                    }
                                    sum += parseFloat(obj2.val);
                                }
                               
                            });

                            if (notClient) {
                                var name = "";
                                if (obj1.owner == "Client") {
                                    name = t.clientDetails.clientName;
                                }
                                else if (obj1.owner == "Partner") {
                                    name = t.clientDetails.partnerName;
                                }
                                else {
                                    name = "Joint";
                                }

                                obj1.displayName = obj1.product + " - " + name;

                            }
                            else {
                                obj1.displayName = obj1.product;
                            }
                            obj1.lessTotalRebates = 0;
                            obj1.total = Math.round(sum);

                            var sumOfDiff = 0;
                           
                            var fund = t.allCurrentFund.filter(item => { return (item.proposedId == obj1.proposedId) });
                            if (fund.length > 0) {
                                sumOfDiff += parseFloat(fund[0].feeDisplay.reduce(function (a: any, b: any) { return a + parseFloat(b.val); }, 0));
                            }


                            var display = "";
                            var value = Math.round(sum) - Math.round(sumOfDiff);
                            if (value > 0) {
                                display = " Additional $" + Math.abs(value);
                            }
                            else if (value < 0) {
                                display = " Less $" + Math.abs(value);
                            }
                            else {
                                display = "$0";
                            }

                            obj1.difference = display;
                        });
                    });

                    newObj.proposedProducts = tempProposedProducts;
                    newObj.existingProducts = existingProducts;
                    newObj.alternativeProducts = alternativeProducts;
                    newObj.feeDisplay = feeDisplay;

                    if (newObj.existingProducts.length > 0 || newObj.alternativeProducts.length > 0) {
                        this.partnerProductReplacement.push(newObj);
                    }
                }
            }

           
        }
        ////joint
        if (this.jointProposedFund != undefined && this.jointProposedFund.length > 0) {
          
            var proposedProducts = this.jointProposedFund;
           
            var t = this;
            for (let product of proposedProducts) {
                var feeDisplay: any[] = [];
                var tempProposedProducts: any[] = [];
                var existingProducts: any[] = [];
                var alternativeProducts: any[] = [];

                tempProposedProducts.push(product);

                var alternative = t.totalAlternativeProducts.filter(item => { return (item.proposedId == product.id) });
                alternative.forEach(function (alt) {
                    alternativeProducts.push(alt);
                });

             

                var fund = t.allCurrentFund.filter(item => { return (item.proposedId == product.id) });

                    fund.forEach(function (fnd) {
                        existingProducts.push(fnd);
                    });

               

            

            var isExisting = existingProducts.length > 0 ? existingProducts.filter(item => { return (item.owner != "Joint") }) : [];
            var isAlternate = alternativeProducts.length > 0 ? alternativeProducts.filter(item => { return (item.owner != "Joint") }) : [];
            var notClient: boolean = false;
            if (isExisting.length > 0 || isAlternate.length > 0) {
                notClient = true;
            }

                if (tempProposedProducts.length > 0 || existingProducts.length > 0 || alternativeProducts.length > 0) {

                    var newObj: any = {};

                    existingProducts.forEach(function (obj: any) {
                        var sum = 0;
                        obj.feeDisplay.forEach(function (obj1: any) {
                            if ((obj1.val != 0 || obj1.percentage != 0) && obj1.feeType.trim() == "ongoing") {
                                var isExist = feeDisplay.indexOf(obj1.name.trim());
                                if (isExist < 0) {
                                    feeDisplay.push(obj1.name.trim());

                                }
                                sum += parseFloat(obj1.val);
                            }
                         
                        });

                        if (notClient) {
                            var name = "";
                            if (obj.owner == "Client") {
                                name = t.clientDetails.clientName;
                            }
                            else if (obj.owner == "Partner") {
                                name = t.clientDetails.partnerName;
                            }
                            else {
                                name = "Joint";
                            }

                            obj.displayName = obj.product + " - " + name;

                        }
                        else {
                            obj.displayName = obj.product;
                        }

                        obj.lessTotalRebates = 0;
                        obj.total = Math.round(sum);

                    });

                    //  proposedProducts.forEach(function (obj: any) {
                    for (let obj of tempProposedProducts) {
                        var sum = 0;
                        obj.feeDisplay.forEach(function (obj1: any) {
                            if ((obj1.val != 0 || obj1.percentage != 0) && obj1.feeType.trim() == "ongoing") {
                                var isExist = feeDisplay.indexOf(obj1.name.trim());
                                if (isExist < 0) {
                                    feeDisplay.push(obj1.name.trim());

                                }
                                sum += parseFloat(obj1.val);
                            }
                           
                        });

                        if (notClient) {
                            var name = "";
                            if (obj.owner == "Client") {
                                name = t.clientDetails.clientName;
                            }
                            else if (obj.owner == "Partner") {
                                name = t.clientDetails.partnerName;
                            }
                            else {
                                name = "Joint";
                            }

                            obj.displayName = obj.product + " - " + name;

                        }
                        else {
                            obj.displayName = obj.product;
                        }

                        obj.lessTotalRebates = 0;
                        obj.total = Math.round(sum);


                        var sumOfDiff = 0;
                        
                        var fund = t.allCurrentFund.filter(item => { return (item.proposedId == obj.id)  });
                        if (fund.length > 0) {
                            sumOfDiff += parseFloat(fund[0].feeDisplay.reduce(function (a: any, b: any) { return a + parseFloat(b.val); }, 0));
                        }


                        var display = "";
                        var value = Math.round(sum) - Math.round(sumOfDiff);
                        if (value > 0) {
                            display = " Additional $" + Math.abs(value);
                        }
                        else if (value < 0) {
                            display = " Less $" + Math.abs(value);
                        }
                        else {
                            display = "$0";
                        }

                        obj.difference = display;

                    }

                    tempProposedProducts.forEach(function (obj: any) {

                        var alt = alternativeProducts.filter(item => { return (item.proposedId == obj.id) });
                        alt.forEach(function (obj1: any) {
                            var sum = 0;
                            obj1.feeDisplay.forEach(function (obj2: any) {
                                if ((obj2.val != 0 || obj2.percentage != 0) && obj2.feeType.trim() == "ongoing") {
                                    var isExist = feeDisplay.indexOf(obj2.name.trim());
                                    if (isExist < 0) {
                                        feeDisplay.push(obj2.name.trim());

                                    }
                                    sum += parseFloat(obj2.val);
                                }
                               
                            });

                            if (notClient) {
                                var name = "";
                                if (obj1.owner == "Client") {
                                    name = t.clientDetails.clientName;
                                }
                                else if (obj1.owner == "Partner") {
                                    name = t.clientDetails.partnerName;
                                }
                                else {
                                    name = "Joint";
                                }

                                obj1.displayName = obj1.product + " - " + name;

                            }
                            else {
                                obj1.displayName = obj1.product;
                            }
                            obj1.lessTotalRebates = 0;
                            obj1.total = Math.round(sum);

                            var sumOfDiff = 0;

                            var fund = t.allCurrentFund.filter(item => { return (item.proposedId == obj1.proposedId) });
                            if (fund.length > 0) {
                            sumOfDiff += parseFloat(fund[0].feeDisplay.reduce(function (a: any, b: any) { return a + parseFloat(b.val); }, 0));
                        }


                            var display = "";
                            var value = Math.round(sum) - Math.round(sumOfDiff);
                            if (value > 0) {
                                display = " Additional $" + Math.abs(value);
                            }
                            else if (value < 0) {
                                display = " Less $" + Math.abs(value);
                            }
                            else {
                                display = "$0";
                            }

                            obj1.difference = display;
                        });
                    });

                    newObj.proposedProducts = tempProposedProducts;
                    newObj.existingProducts = existingProducts;
                    newObj.alternativeProducts = alternativeProducts;
                    newObj.feeDisplay = feeDisplay;

                    if (newObj.existingProducts.length > 0 || newObj.alternativeProducts.length > 0) {
                        this.jointProductReplacement.push(newObj);
                    }
                }
            }


        }

    }
    proposedProductSum() {
        var Proposedtotal = 0;
        this.proposedInvestments.forEach(function (obj) {
            Proposedtotal += obj.value;
        });

        this.totalProposedInvestments = Proposedtotal;
    }
    setFilteredInvestment() {
        var filtered = $.grep(this.currentInvestments, function (obj: any) {
            return obj.unutilizedValue > 0;
        });
        this.filteredInvestments = filtered;
    }
  
   
  //fee
    async generateFeeDetails() {
    this.currentFund.forEach((cf: any) => {
      cf.feeDisplay = [];
      var gross = 0;
      this.productFeeDetails.forEach((pfd: any) => {

        if (pfd.feeName != "Gross On-going Costs") {
          if (pfd.feeName == "Buy/Sell Costs") {
            var buySell = $.grep(cf.feeDisplay, function (el: any) {
              return el.feeName === "Buy/Sell Costs";
            });
            if (undefined !== buySell && buySell.length <= 0) {
              var obj: any = {};
              obj.name = "Buy/Sell Costs";
              obj.percentage = 0.00;

              var sum = 0;
              cf.data.forEach((cf: any) => {

                var obj1 = $.grep(this.allFunds, function (obj2: any) {
                  return obj2.apircode == cf.apircode;
                });
                sum += ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value) / 100) : 0;
              });

              obj.val = sum.toFixed(2);
              cf.feeDisplay.push(obj);
            }
          }
          else if (pfd.feeName == "Indirect Cost Ratio (ICR)") {
            var icr = $.grep(cf.feeDisplay, function (el: any) {
              return el.feeName === "Indirect Cost Ratio (ICR)";
            });
            if (undefined !== icr && icr.length <= 0) {
              var obj: any = {};
              obj.name = "Indirect Cost Ratio (ICR)";
              //  obj.percentage = 0.00;

              var sum = 0;
              cf.data.forEach((cf: any) => {

                var obj1 = $.grep(this.allFunds, function (obj2: any) {
                  return obj2.apircode == cf.apircode;
                });
                sum += (parseFloat(obj1[0].icr) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].icr) * parseFloat(cf.value)) / 100) : 0;
              });

              var fundSum = (cf.data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0));


              obj.val = sum.toFixed(2);
             var perc = 0;
              if (fundSum != 0) {
                perc = (sum / fundSum) * 100;
              }
              obj.percentage = perc.toFixed(2);
              cf.feeDisplay.push(obj);

              gross += sum;
            }
          }
          else {


            var exist = $.grep(cf.feeDetails, function (el: any) {
              return el.feeName === pfd.feeName;
            });

            var final = $.grep(cf.feeDisplay, function (el: any) {
              return el.feeName === pfd.feeName;
            });

            if (undefined !== final && final.length <= 0) {
              var obj: any = {};

              if (exist.length > 0) {

                obj.name = pfd.feeName;

                var temp = $.grep(this.individualProductFeeDetails, function (el: any) {
                  return (el.feeName === pfd.feeName && el.productId === cf.productId);
                });

                if (undefined !== temp && temp.length <= 0) {
                  obj.percentage = 0.00;
                  obj.val = 0.00;
                }
                else {
                  if (temp[0].feeType === 'P') {
                    obj.percentage = ((parseFloat(exist[0].amount) / cf.value) * 100).toFixed(2);
                      obj.val = (exist[0].amount).toFixed(2);

                    if (temp[0].costType === "ongoing") {
                      gross += parseFloat(obj.val);
                    }

                  }
                  else {
                    obj.percentage = 0.00;
                    obj.val = (exist[0].amount).toFixed(2);

                    if (temp[0].costType === "ongoing") {
                      gross += parseFloat(obj.val);
                    }
                  }
                }

              }
              else {
                obj.name = pfd.feeName;
                obj.percentage = 0.00;
                obj.val = 0.00;



              }
              cf.feeDisplay.push(obj);
            }
          }
        }
      });

      cf.gross = gross.toFixed(2);

    });

    this.proposedProduct.forEach((pp: any) => {
      pp.feeDisplay = [];
      var gross = 0;
      this.productFeeDetails.forEach((pfd: any) => {


        if (pfd.feeName == "Buy/Sell Costs") {
          var buySell = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Buy/Sell Costs";
          });
          if (undefined !== buySell && buySell.length <= 0) {
            var obj: any = {};
            obj.name = "Buy/Sell Costs";
            obj.percentage = 0.00;

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value) / 100) : 0;
            });

            obj.val = sum.toFixed(2);
            pp.feeDisplay.push(obj);

          }
        }
        else if (pfd.feeName == "Indirect Cost Ratio (ICR)") {
          var icr = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Indirect Cost Ratio (ICR)";
          });
          if (undefined !== icr && icr.length <= 0) {
            var obj: any = {};
            obj.name = "Indirect Cost Ratio (ICR)";
            //  obj.percentage = 0.00;

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += (parseFloat(obj1[0].icr) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].icr) * parseFloat(cf.value)) / 100) : 0;
            });

            var fundSum = (pp.data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0));

            obj.val = sum.toFixed(2);
            var perc = 0;
            if (fundSum != 0) {
              perc = (sum / fundSum) * 100;
            }
            obj.percentage = perc.toFixed(2);
            pp.feeDisplay.push(obj);
            gross += sum;
          }
        }
        else {

          var exist = $.grep(pp.feeDetails, function (el: any) {
            return el.feeName === pfd.feeName;
          });
          var final = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === pfd.feeName;
          });

          if (undefined !== final && final.length <= 0) {
            var obj: any = {};
            if (exist.length > 0) {
              obj.name = pfd.feeName;
              var temp = $.grep(this.individualProductFeeDetails, function (el: any) {
                return (el.feeName === pfd.feeName && el.productId === pp.productId);
              });

              if (undefined !== temp && temp.length <= 0) {
                obj.percentage = 0.00;
                obj.val = 0.00;
              }
              else {
                if (temp[0].feeType === 'P') {
                  obj.percentage = ((parseFloat(exist[0].amount) / pp.value) * 100).toFixed(2);
                  obj.val = (exist[0].amount).toFixed(2);

                  //obj.percentage = (exist[0].amount).toFixed(2);
                  //obj.val = ((pp.value * parseFloat(exist[0].amount)) / 100).toFixed(2);

                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
                else {
                  obj.percentage = 0.00;
                  obj.val = (exist[0].amount).toFixed(2);

                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
              }
            }
            else {
              obj.name = pfd.feeName;
              obj.percentage = 0.00;
              obj.val = 0.00;
            }
            pp.feeDisplay.push(obj);
          }
        }
      });
      pp.gross = gross.toFixed(2);

    });


    this.alternativeProducts.forEach((ap: any) => {
      ap.feeDisplay = [];
      var gross = 0;
      this.productFeeDetails.forEach((pfd: any) => {

        if (pfd.feeName == "Buy/Sell Costs") {
          var buySell = $.grep(ap.feeDisplay, function (el: any) {
            return el.feeName === "Buy/Sell Costs";
          });
          if (undefined !== buySell && buySell.length <= 0) {
            var obj: any = {};
            obj.name = "Buy/Sell Costs";
            obj.percentage = 0.00;

            var sum = 0;
            ap.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value) / 100) : 0;
            });

            obj.val = sum.toFixed(2);
            ap.feeDisplay.push(obj);
          }
        }
        else if (pfd.feeName == "Indirect Cost Ratio (ICR)") {
          var icr = $.grep(ap.feeDisplay, function (el: any) {
            return el.feeName === "Indirect Cost Ratio (ICR)";
          });
          if (undefined !== icr && icr.length <= 0) {
            var obj: any = {};
            obj.name = "Indirect Cost Ratio (ICR)";
            //  obj.percentage = 0.00;

            var sum = 0;
            ap.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += (parseFloat(obj1[0].icr) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].icr) * parseFloat(cf.value)) / 100) : 0;
            });

            var fundSum = (ap.data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0));

            obj.val = sum.toFixed(2);
            var perc = 0;
            if (fundSum != 0) {
              perc = (sum / fundSum) * 100;
            }
          
            obj.percentage = perc.toFixed(2);
            ap.feeDisplay.push(obj);
            gross += sum;
          }
        }
        else {
          var exist = $.grep(ap.feeDetails, function (el: any) {
            return el.feeName === pfd.feeName;
          });
          var final = $.grep(ap.feeDisplay, function (el: any) {
            return el.feeName === pfd.feeName;
          });

          if (undefined !== final && final.length <= 0) {
            var obj: any = {};
            if (exist.length > 0) {
              obj.name = pfd.feeName;
              var temp = $.grep(this.individualProductFeeDetails, function (el: any) {
                return (el.feeName === pfd.feeName && el.productId === ap.productId);
              });

              if (undefined !== temp && temp.length <= 0) {
                obj.percentage = 0.00;
                obj.val = 0.00;
              }
              else {
                if (temp[0].feeType === 'P') {
                  //obj.percentage = (exist[0].amount).toFixed(2);
                  //obj.val = ((ap.value * parseFloat(exist[0].amount)) / 100).toFixed(2);

                  obj.percentage = ((parseFloat(exist[0].amount) / ap.value) * 100).toFixed(2);
                  obj.val = (exist[0].amount).toFixed(2);


                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
                else {
                  obj.percentage = 0.00;
                  obj.val = (exist[0].amount).toFixed(2);

                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
              }
            }
            else {
              obj.name = pfd.feeName;
              obj.percentage = 0.00;
              obj.val = 0.00;
            }
            ap.feeDisplay.push(obj);
          }
        }
      });

      ap.gross = gross.toFixed(2);
    });



    this.ongoing = $.grep(this.productFeeDetails, function (el: any) {
      return el.feeCost === 'ongoing' && el.feeName != "Gross On-going Costs";
    });





    this.transactional = $.grep(this.productFeeDetails, function (el: any) {
      return el.feeCost === 'transactional';
    });

  }
    async generateAllFeeDetails() {

    var icr = $.grep(this.productFeeDetails, function (obj: any) {
      return obj.feeName == "Indirect Cost Ratio (ICR)";
    });
    if (icr.length <= 0) {
      var icrVal: any = { 'feeName': "Indirect Cost Ratio (ICR)", 'feeType': "P", 'feeCost': "ongoing" };
      this.productFeeDetails.push(icrVal);
    }


    var buySell = $.grep(this.productFeeDetails, function (obj: any) {
      return obj.feeName == "Buy/Sell Costs";
    });
    if (buySell.length <= 0) {
      var bSell: any = { 'feeName': "Buy/Sell Costs", 'feeType': 'F', 'feeCost': "transactional" };
      this.productFeeDetails.push(bSell);
    }

    this.allProposedFund.forEach((pp: any) => {
      pp.feeDisplay = [];
      var gross = 0;
      this.productFeeDetails.forEach((pfd: any) => {

        if (pfd.feeName == "Buy/Sell Costs") {
          var buySell = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Buy/Sell Costs";
          });
          if (undefined !== buySell && buySell.length <= 0) {
            var obj: any = {};
            obj.name = "Buy/Sell Costs";
            obj.percentage = 0.00;

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value) / 100) : 0;
            });

            obj.val = sum.toFixed(2);
            obj.feeType = pfd.feeCost;
            pp.feeDisplay.push(obj);

          }
        }
        else if (pfd.feeName == "Indirect Cost Ratio (ICR)") {
          var icr = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Indirect Cost Ratio (ICR)";
          });
          if (undefined !== icr && icr.length <= 0) {
            var obj: any = {};
            obj.name = "Indirect Cost Ratio (ICR)";

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += (parseFloat(obj1[0].icr) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].icr) * parseFloat(cf.value)) / 100) : 0;
            });

            var fundSum = (pp.data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0));

            obj.val = sum.toFixed(2);
            var perc = 0;
            if (fundSum != 0) {
              perc = (sum / fundSum) * 100;
            }
            obj.percentage = perc.toFixed(2);
            obj.feeType = pfd.feeCost;
            pp.feeDisplay.push(obj);
            gross += sum;


          }
        }
        else {

          var exist = $.grep(pp.feeDetails, function (el: any) {
            return el.feeName === pfd.feeName;
          });
          var final = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === pfd.feeName;
          });

          if (undefined !== final && final.length <= 0) {
            var obj: any = {};
            if (exist.length > 0) {
              obj.name = pfd.feeName;
              obj.feeType = pfd.feeCost;
              var temp = $.grep(this.individualProductFeeDetails, function (el: any) {
                return (el.feeName === pfd.feeName && el.productId === pp.productId);
              });
              if (undefined !== temp && temp.length <= 0) {
                obj.percentage = 0.00;
                obj.val = 0.00;
              }
              else {
                if (temp[0].feeType === 'P') {
                  //obj.percentage = (exist[0].amount).toFixed(2);
                  //obj.val = ((pp.value * parseFloat(exist[0].amount)) / 100).toFixed(2);
                  obj.percentage = ((parseFloat(exist[0].amount) / pp.value) * 100).toFixed(2);
                  obj.val = (exist[0].amount).toFixed(2);


                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
                else {
                  obj.percentage = 0.00;
                  obj.val = (exist[0].amount).toFixed(2);

                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
              }
            }
            else {
              obj.name = pfd.feeName;
              obj.percentage = 0.00;
              obj.val = 0.00;
              obj.feeType = pfd.feeCost;
            }
            pp.feeDisplay.push(obj);
          }
        }

      });
      pp.gross = gross.toFixed(2);

    });

    this.allCurrentFund.forEach((pp: any) => {
      pp.feeDisplay = [];
      var gross = 0;
      this.productFeeDetails.forEach((pfd: any) => {


        if (pfd.feeName == "Buy/Sell Costs") {
          var buySell = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Buy/Sell Costs";
          });
          if (undefined !== buySell && buySell.length <= 0) {
            var obj: any = {};
            obj.name = "Buy/Sell Costs";
            obj.percentage = 0.00;

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });


              sum += ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value) / 100) : 0;
            });

            obj.val = sum.toFixed(2);
            obj.feeType = pfd.feeCost;
            pp.feeDisplay.push(obj);



          }
        }
        else if (pfd.feeName == "Indirect Cost Ratio (ICR)") {
          var icr = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Indirect Cost Ratio (ICR)";
          });
          if (undefined !== icr && icr.length <= 0) {
            var obj: any = {};
            obj.name = "Indirect Cost Ratio (ICR)";

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });

              sum += (parseFloat(obj1[0].icr) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].icr) * parseFloat(cf.value)) / 100) : 0;
            });

            var fundSum = (pp.data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0));

            obj.val = sum.toFixed(2);
            var perc = 0;
            if (fundSum != 0) {
              perc = (sum / fundSum) * 100;
            }
            obj.percentage = perc.toFixed(2);
            obj.feeType = pfd.feeCost;
            pp.feeDisplay.push(obj);
            gross += sum;

          }
        }
        else {

          var exist = $.grep(pp.feeDetails, function (el: any) {
            return el.feeName === pfd.feeName;
          });
          var final = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === pfd.feeName;
          });

          if (undefined !== final && final.length <= 0) {
            var obj: any = {};
            if (exist.length > 0) {
              obj.name = pfd.feeName;
              obj.feeType = pfd.feeCost;
              var temp = $.grep(this.individualProductFeeDetails, function (el: any) {
                return (el.feeName === pfd.feeName && el.productId === pp.productId);
              });
              if (undefined !== temp && temp.length <= 0) {
                obj.percentage = 0.00;
                obj.val = 0.00;
              }
              else {

                if (temp[0].feeType === 'P') {
                  //obj.percentage = (exist[0].amount).toFixed(2);
                  //obj.val = ((pp.value * parseFloat(exist[0].amount)) / 100).toFixed(2);
                  obj.percentage = ((parseFloat(exist[0].amount) / pp.value) * 100).toFixed(2);
                  obj.val = (exist[0].amount).toFixed(2);


                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
                else {
                  obj.percentage = 0.00;
                  obj.val = (exist[0].amount).toFixed(2);

                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
              }
            }
            else {
              obj.name = pfd.feeName;
              obj.percentage = 0.00;
              obj.val = 0.00;
              obj.feeType = pfd.feeCost;
            }
            pp.feeDisplay.push(obj);
          }
        }
      });
      pp.gross = gross.toFixed(2);

    });

    this.totalAlternativeProducts.forEach((pp: any) => {
      pp.feeDisplay = [];
      var gross = 0;
      this.productFeeDetails.forEach((pfd: any) => {


        if (pfd.feeName == "Buy/Sell Costs") {
          var buySell = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Buy/Sell Costs";
          });
          if (undefined !== buySell && buySell.length <= 0) {
            var obj: any = {};
            obj.name = "Buy/Sell Costs";
            obj.percentage = 0.00;

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].buySpread) * 2) * parseFloat(cf.value) / 100) : 0;
            });

            obj.val = sum.toFixed(2);
            obj.feeType = pfd.feeCost;
            pp.feeDisplay.push(obj);

          }
        }
        else if (pfd.feeName == "Indirect Cost Ratio (ICR)") {
          var icr = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === "Indirect Cost Ratio (ICR)";
          });
          if (undefined !== icr && icr.length <= 0) {
            var obj: any = {};
            obj.name = "Indirect Cost Ratio (ICR)";

            var sum = 0;
            pp.data.forEach((cf: any) => {

              var obj1 = $.grep(this.allFunds, function (obj2: any) {
                return obj2.apircode == cf.apircode;
              });
              sum += (parseFloat(obj1[0].icr) * parseFloat(cf.value)) != 0 ? ((parseFloat(obj1[0].icr) * parseFloat(cf.value)) / 100) : 0;
            });

            var fundSum = (pp.data.reduce(function (a: any, b: any) { return a + parseFloat(b.value); }, 0));

            obj.val = sum.toFixed(2);
            var perc = 0;
            if (fundSum != 0) {
              perc = (sum / fundSum) * 100;
            }
            obj.percentage = perc.toFixed(2);
            obj.feeType = pfd.feeCost;
            pp.feeDisplay.push(obj);
            gross += sum;
          }
        }
        else {

          var exist = $.grep(pp.feeDetails, function (el: any) {
            return el.feeName === pfd.feeName;
          });
          var final = $.grep(pp.feeDisplay, function (el: any) {
            return el.feeName === pfd.feeName;
          });

          if (undefined !== final && final.length <= 0) {
            var obj: any = {};
            if (exist.length > 0) {
              obj.name = pfd.feeName;
              obj.feeType = pfd.feeCost;
              var temp = $.grep(this.individualProductFeeDetails, function (el: any) {
                return (el.feeName === pfd.feeName && el.productId === pp.productId);
              });
              if (undefined !== temp && temp.length <= 0) {
                obj.percentage = 0.00;
                obj.val = 0.00;
              }
              else {
                if (temp[0].feeType === 'P') {
                  //obj.percentage = (exist[0].amount).toFixed(2);
                  //obj.val = ((pp.value * parseFloat(exist[0].amount)) / 100).toFixed(2);
                  obj.percentage = ((parseFloat(exist[0].amount) / pp.value) * 100).toFixed(2);
                  obj.val = (exist[0].amount).toFixed(2);



                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
                else {
                  obj.percentage = 0.00;
                  obj.val = (exist[0].amount).toFixed(2);

                  if (temp[0].costType === "ongoing") {
                    gross += parseFloat(obj.val);
                  }

                }
              }
            }
            else {
              obj.name = pfd.feeName;
              obj.percentage = 0.00;
              obj.val = 0.00;
              obj.feeType = pfd.feeCost;
            }
            pp.feeDisplay.push(obj);
          }
        }
      });
      pp.gross = gross.toFixed(2);

    });
  }

  //product fees

  calculateFee(headerId: number, headerType: string, productId :number) {

    var fund: any;
    var selectedProduct: any;
    if (headerType == "P") {
      fund = this.proposedFund;
      selectedProduct = this.selectedProductDetails;
    }
    else if (headerType == "C") {
      var product = $.grep(this.currentFund, function (obj: any) {
        return obj.id == headerId;
      });
      if (product.length > 0) {
        selectedProduct = product[0];
        fund = product[0].data;
      }
       
      }
    else if (headerType == "A") {
      var product = $.grep(this.alternativeProducts, function (obj: any) {
        return obj.id == headerId;
      });
      if (product.length > 0) {
        selectedProduct = product[0];
        fund = product[0].data;
      }
    }



    var platform = $.grep(this.platformDetails, function (obj: any) {
      return obj.productId == productId;
    });

    if (platform.length > 0) {
      if (platform[0].platformName == "GrowWrap") {

        var productFees: any[] = [];
        var portfolioVal = 0;
        portfolioVal = selectedProduct.value > 750000 ? 750000 : this.selectedProductDetails.value;

        //Admin Fees
        var adminFee = {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Administration Fee",
          feeType: "P",
          amount: 0
        };
        var adminExpenseRecoveryFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Expense Recovery Fee",
          feeType: "P",
          amount: 0
        }
        var asxShareTradingFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "ASX Share Trading",
          feeType: "P",
          amount: 0
        }
        var brokerageFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Brokerage Fee",
          feeType: "P",
          amount: 0
        }
        var exitFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Exit/Withdrawal fees",
          feeType: "P",
          amount: 0
        }
        var otherFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Other transaction costs",
          feeType: "P",
          amount: 0
        }
        var managedFundFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Managed fund transactions",
          feeType: "P",
          amount: 0
        }

        var SMAAdminFee = {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "SMA Administration Access Fee",
          feeType: "P",
          amount: 0
        };

        var essentialsVal = 0;
        var emfVal = 0;
        var smaVal = 0;
        var asxVal = 0;

        if (headerType == "P") {
          for (var obj of fund) {
            if (obj.apircode != "Cash") {
              managedFundFee.amount += 20.5;
            }
           }
        }


        //Essentials
        essentialsVal = portfolioVal * 0.0043;

        //Extended Managed Funds
        var emf = $.grep(fund, function (obj: any) {
          return obj.feeLabel1 == "EMF";
        });
        if (emf.length > 0) {
          emfVal = portfolioVal * 0.0007;
        }

        //SMA
        var sma = $.grep(fund, function (obj: any) {
          return obj.feeLabel2 == "SMA";
        });
        if (sma.length > 0) {
          smaVal = portfolioVal * 0.0005;

          SMAAdminFee.amount = 300; //Added to admin  ?
        }

        //ASX
        var asx = $.grep(fund, function (obj: any) {
          return obj.feeLabel3 == "ASX";
        });
        if (asx.length > 0) {
          var asxSum = 0;
          asx.forEach((obj: any) => {
            asxSum += obj.val;
          });

          if (asxSum <= 750000) {
            asxVal = portfolioVal * 0.0005;
          }

          brokerageFee.amount = asxSum * 0.0012;
          if (brokerageFee.amount < 30) {
            brokerageFee.amount = 30;
          }
          else if (brokerageFee.amount > 100) {
            brokerageFee.amount = 100;
          }
        }

        //TODO - Verify Family Discount (Current/Proposed Investments)
        var fdIsEligible = 0;

        for (var obj of this.proposedInvestments) {
          if (obj.platformName == "GrowWrap" && (obj.recId != headerId)) {
            fdIsEligible = 1;
            break;
          }
        }

        if (fdIsEligible == 1) {
          adminFee.amount = ((essentialsVal + emfVal) * 0.8) + smaVal;
        }
        else {
          adminFee.amount = (essentialsVal + emfVal + smaVal);
        }

        if (adminFee.amount < 214.92) {
          adminFee.amount = 214.92;
        }

        //ASX Share
        asxShareTradingFee.amount = asxVal;

        //Admin Expense Recovery
        adminExpenseRecoveryFee.amount = portfolioVal * 0.0002;

        productFees.push(adminFee, adminExpenseRecoveryFee, asxShareTradingFee, brokerageFee, otherFee, managedFundFee, exitFee, SMAAdminFee);
        this.addProductFee(productFees, headerId, headerType);

      }
      else if (platform[0].platformName == "MyNorth") {

        var productFees: any[] = [];
        var portfolioVal = 0;
        portfolioVal = selectedProduct.value;

        //Admin Fees
        var adminFee = {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Administration Fee",
          feeType: "P",
          amount: 0
        };
        var membershipFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Membership Fee",
          feeType: "F",
          amount: 90.96
        }

        var brokerageFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Brokerage Fee",
          feeType: "P",
          amount: 0
        }
        var exitFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Exit/Withdrawal fees",
          feeType: "P",
          amount: 0
        }
        var otherFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Other transaction costs",
          feeType: "P",
          amount: 0
        }
        var managedFundFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Managed fund transactions",
          feeType: "P",
          amount: 0
        }

        var selectVal = 0;
        var choiceVal = 0;


        //Select
        var select = $.grep(fund, function (obj: any) {
          return obj.feeLabel1 == "Select";
        });
        if (select.length > 0) {
          var selectSum = 0;
          select.forEach((obj: any) => {
            selectSum += obj.value;
          });


          if (selectSum >= 0 && selectSum <= 149999) {
            selectVal = portfolioVal * 0.003;
          }
          else if (selectSum >= 150000 && selectSum <= 249999) {
            selectVal = portfolioVal * 0.003;
          }
          else if (selectSum >= 250000 && selectSum <= 399999) {
            selectVal = portfolioVal * 0.003;
          }
          else if (selectSum >= 400000 && selectSum <= 749999) {
            selectVal = portfolioVal * 0.0025;
          }
          else if (selectSum >= 750000) {
            selectVal = portfolioVal * 0.002;
          }
        }

        //Choice
        var choice = $.grep(fund, function (obj: any) {
          return obj.feeLabel1 == "Choice";
        });
        if (choice.length > 0) {
          var choiceSum = 0;
          choice.forEach((obj: any) => {
            choiceSum += obj.value;
          });


          if (choiceSum >= 0 && choiceSum <= 149999) {
            choiceVal = portfolioVal * 0.0065;
          }
          else if (choiceSum >= 150000 && choiceSum <= 249999) {
            choiceVal = portfolioVal * 0.006;
          }
          else if (choiceSum >= 250000 && choiceSum <= 399999) {
            choiceVal = portfolioVal * 0.0055;
          }
          else if (choiceSum >= 400000 && choiceSum <= 749999) {
            choiceVal = portfolioVal * 0.0045;
          }
          else if (choiceSum >= 750000) {
            choiceVal = portfolioVal * 0.003;
          }

        }


        //ASX
        var asx = $.grep(fund, function (obj: any) {
          return obj.feeLabel3 == "ASX";
        });
        if (asx.length > 0) {
          var asxSum = 0;
          asx.forEach((obj: any) => {
            asxSum += obj.value;
          });


          brokerageFee.amount = asxSum * 0.0011;
          if (brokerageFee.amount < 34) {
            brokerageFee.amount = 34;
          }
        }

        //TODO : Family Discount

        adminFee.amount = selectVal + choiceVal;

        productFees.push(adminFee, brokerageFee, membershipFee, otherFee, managedFundFee, exitFee);
        this.addProductFee(productFees, headerId, headerType);

      }
      else if (platform[0].platformName == "BTPanorama") {

        var productFees: any[] = [];
        var portfolioVal = 0;
        var fdPortfolioVal = 0;
        portfolioVal = selectedProduct.value;
       

        //Admin Fees
        var adminFee = {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Administration Fee",
          feeType: "P",
          amount: 0
        };

        var membershipFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Membership Fee",
          feeType: "F",
          amount: 0
        }

        var brokerageFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Brokerage Fee",
          feeType: "P",
          amount: 0
        }
        var exitFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Exit/Withdrawal fees",
          feeType: "P",
          amount: 0
        }
        var otherFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Other transaction costs",
          feeType: "P",
          amount: 0
        }
        var managedFundFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Managed fund transactions",
          feeType: "P",
          amount: 0
        }
        var operationalFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Operational Risk Levy Fee",
          feeType: "P",
          amount: 0
        }

        var adminExpenseRecoveryFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Expense Recovery Fee",
          feeType: "F",
          amount: 95
        }


        // TODO - Verify Brokerage Fee
        var asx = $.grep(fund, function (obj: any) {
          return obj.feeLabel3 == "ASX";
        });
        if (asx.length > 0) {
          var asxSum = 0;
          asx.forEach((obj: any) => {
            asxSum += obj.value;
          });


          brokerageFee.amount = asxSum * 0.0011;
          if (brokerageFee.amount < 12.50) {
            brokerageFee.amount = 12.50;
          }
        }

        // TODO - Family Discount

        var currentProd = $.grep(this.allProducts, function (obj: any) {
          return obj.productId == productId;
        });

        if (currentProd.length > 0) {
          if (currentProd[0].subType == "Compact") {
            membershipFee.amount = 180;
          }
          else {
            membershipFee.amount = 540;
          }
        }

        for (var obj of this.proposedInvestments) {
          if (obj.platformName == "BTPanorama") {
            fdPortfolioVal += obj.value;
          }
        }

        if (fdPortfolioVal! = 0) {
          if (fdPortfolioVal > 1000000) {
            adminFee.amount = 0;
          }
          else {
            adminFee.amount = fdPortfolioVal * 0.0015;
          }
        }
        else {
          if (portfolioVal > 1000000) {
            adminFee.amount = 0;
          }
          else {
            adminFee.amount = portfolioVal * 0.0015;
          }
        }
      

        operationalFee.amount = 0.0003 * portfolioVal;   

        productFees.push(adminFee, membershipFee, operationalFee, adminExpenseRecoveryFee, brokerageFee ,otherFee, managedFundFee, exitFee);
        this.addProductFee(productFees, headerId, headerType);

      }
      else if (platform[0].platformName == "Summit") {

        var productFees: any[] = [];
        var portfolioVal = 0;
        portfolioVal = selectedProduct.value;

        //Admin Fees
        var adminFee = {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Administration Fee",
          feeType: "P",
          amount: 0
        };

        var brokerageFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Brokerage Fee",
          feeType: "P",
          amount: 0
        }
        var trusteeFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Trustee Fee",
          feeType: "P",
          amount: 0
        }


        var exitFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Exit/Withdrawal fees",
          feeType: "P",
          amount: 0
        }
        var otherFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Other transaction costs",
          feeType: "P",
          amount: 0
        }
        var managedFundFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Managed fund transactions",
          feeType: "P",
          amount: 0
        }

        var InvestorVal = 0;
        var NorthVal = 0;


        //North
        var north = $.grep(fund, function (obj: any) {
          return obj.feeLabel1 == "North";
        });
      
        if (north.length > 0) {
          var northSum = 0;
          north.forEach((obj: any) => {
            northSum += obj.value;
          });


          if (northSum <= 125000) {
            NorthVal = northSum * 0.007375;
          }
          else {
            NorthVal += 125000 * 0.007375;
            if (northSum <= 250000) {
              NorthVal += (northSum - 125000) * 0.002875;
            }
            else {
              NorthVal += 125000 * 0.002875;
              if (northSum <= 500000) {
                NorthVal += (northSum - 250000) * 0.002875;
              }
              else {
                NorthVal += 250000 * 0.002875;
                if (northSum <= 1000000) {
                  NorthVal += (northSum - 500000) * 0.002875;
                }
                else {
                  NorthVal += 500000 * 0.002875;
                  NorthVal += (northSum - 1000000) * 0.001125;

                }

              }

            }

          }
        }


        //Choice
        var investor = $.grep(fund, function (obj: any) {
          return obj.feeLabel2 == "Investor" || obj.apircode == "Cash";
        });
        if (investor.length > 0) {
          var investorSum = 0;
          investor.forEach((obj: any) => {
            investorSum += obj.value;
          });

          if (investorSum <= 125000) {
            InvestorVal = investorSum * 0.009475;
          }
          else {
            InvestorVal += 125000 * 0.009475;
            if (investorSum <= 250000) {
              InvestorVal += (investorSum - 125000) * 0.005475;
            }
            else {
              InvestorVal += 125000 * 0.005475;
              if (investorSum <= 500000) {
                InvestorVal += (investorSum - 250000) * 0.002975;
              }
              else {
                InvestorVal += 250000 * 0.002975;
                if (investorSum <= 1000000) {
                  InvestorVal += (investorSum - 500000) * 0.002975;
                }
                else {
                  InvestorVal += 500000 * 0.002975;
                  InvestorVal += (investorSum - 1000000) * 0.001125;

                }

              }

            }

          }

        }



      

        trusteeFee.amount = 0.001 * portfolioVal;


        adminFee.amount = InvestorVal + NorthVal;

        productFees.push(adminFee, trusteeFee, otherFee, managedFundFee, exitFee, brokerageFee);
        this.addProductFee(productFees, headerId, headerType);

      }
      else if (platform[0].platformName == "North") {

        var productFees: any[] = [];
        var portfolioVal = 0;
        var fdPortfolioVal = 0;
        portfolioVal = selectedProduct.value;

        //Admin Fees
        var adminFee = {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Administration Fee",
          feeType: "P",
          amount: 0
        };

        var membershipFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Membership Fee",
          feeType: "F",
          amount: 90.96
        }

        var brokerageFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Brokerage Fee",
          feeType: "P",
          amount: 0
        }
  
        var trusteeFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Trustee Fee",
          feeType: "P",
          amount: 0
        }


        var exitFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Exit/Withdrawal fees",
          feeType: "P",
          amount: 0
        }
        var otherFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Other transaction costs",
          feeType: "P",
          amount: 0
        }
        var managedFundFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "transactional",
          feeName: "Managed fund transactions",
          feeType: "P",
          amount: 0
        }
        var adminExpenseRecoveryFee =
        {
          feeId: 0,
          headerType: headerType,
          headerId: headerId,
          productId: productId,
          costType: "ongoing",
          feeName: "Expense Recovery Fee",
          feeType: "P",
          amount: 0
        }


        // TODO - Verify Brokerage Fee
        var asx = $.grep(fund, function (obj: any) {
          return obj.feeLabel3 == "ASX";
        });
        if (asx.length > 0) {
          var asxSum = 0;
          asx.forEach((obj: any) => {
            asxSum += obj.value;
          });


          brokerageFee.amount = asxSum * 0.0011;
          if (brokerageFee.amount < 34) {
            brokerageFee.amount = 34;
          }
        }

        // TODO - Family Discount

        var currentProd = $.grep(this.allProducts, function (obj: any) {
          return obj.productId == productId;
        });

        var DiscountedVal = 0;
        var StandardVal = 0;
        if (currentProd.length > 0) {
          if (currentProd[0].subType == "Excludes Guarantee") {
           


            //Standard
            var disc = $.grep(fund, function (obj: any) {
              return obj.feeLabel1 == "Discounted";
            });

            if (disc.length > 0) {
              var discSum = 0;
              disc.forEach((obj: any) => {
                discSum += obj.value;
              });

              if (portfolioVal <= 149999) {
                DiscountedVal = portfolioVal * 0.003;
              }
              else if (portfolioVal >= 150000 && portfolioVal <= 249999) {
                DiscountedVal = portfolioVal * 0.003;
              }
              else if (portfolioVal >= 250000 && portfolioVal <= 399999) {
                DiscountedVal = portfolioVal * 0.003;
              }
              else if (portfolioVal >= 400000 && portfolioVal <= 749999) {
                DiscountedVal = portfolioVal * 0.0025;
              }
              else if(portfolioVal >= 750000) {
                DiscountedVal = portfolioVal * 0.0020;
              }

       
            }


            //Discounted
            var standard = $.grep(fund, function (obj: any) {
              return obj.feeLabel2 == "Standard";
            });

            if (standard.length > 0) {
              var standardSum = 0;
              standard.forEach((obj: any) => {
                standardSum += obj.value;
              });

              if (portfolioVal <= 149999) {
                StandardVal = portfolioVal * 0.0073;
              }
              else if (portfolioVal >= 150000 && portfolioVal <= 249999) {
                StandardVal = portfolioVal * 0.0073;
              }
              else if (portfolioVal >= 250000 && portfolioVal <= 399999) {
                StandardVal = portfolioVal * 0.006;
              }
              else if (portfolioVal >= 400000 && portfolioVal <= 749999) {
                StandardVal = portfolioVal * 0.0045;
              }
              else if (portfolioVal >= 750000) {
                StandardVal = portfolioVal * 0.003;
              }


            }
          }
          else {

            //Discounted
            var disc = $.grep(fund, function (obj: any) {
              return obj.feeLabel1 == "Discounted";
            });

            if (disc.length > 0) {
              var discSum = 0;
              disc.forEach((obj: any) => {
                discSum += obj.value;
              });

              if (portfolioVal <= 149999) {
                DiscountedVal = portfolioVal * 0.004;
              }
              else if (portfolioVal >= 150000 && portfolioVal <= 249999) {
                DiscountedVal = portfolioVal * 0.004;
              }
              else if (portfolioVal >= 250000 && portfolioVal <= 399999) {
                DiscountedVal = portfolioVal * 0.004;
              }
              else if (portfolioVal >= 400000 && portfolioVal <= 749999) {
                DiscountedVal = portfolioVal * 0.0035;
              }
              else if (portfolioVal >= 750000) {
                DiscountedVal = portfolioVal * 0.003;
              }


            }


            //Standard
            var standard = $.grep(fund, function (obj: any) {
              return obj.feeLabel2 == "Standard";
            });

            if (standard.length > 0) {
              var standardSum = 0;
              standard.forEach((obj: any) => {
                standardSum += obj.value;
              });

              if (portfolioVal <= 149999) {
                StandardVal = portfolioVal * 0.0095;
              }
              else if (portfolioVal >= 150000 && portfolioVal <= 249999) {
                StandardVal = portfolioVal * 0.008;
              }
              else if (portfolioVal >= 250000 && portfolioVal <= 399999) {
                StandardVal = portfolioVal * 0.006;
              }
              else if (portfolioVal >= 400000 && portfolioVal <= 749999) {
                StandardVal = portfolioVal * 0.0045;
              }
              else if (portfolioVal >= 750000) {
                StandardVal = portfolioVal * 0.003;
              }


            }
          }
        }

        adminFee.amount = DiscountedVal + StandardVal;

        adminExpenseRecoveryFee.amount = 0.0003 * portfolioVal;
        productFees.push(adminFee, membershipFee, adminExpenseRecoveryFee, brokerageFee, otherFee, managedFundFee, exitFee);
        this.addProductFee(productFees, headerId, headerType);
      }
    }


  }
  addProductFee(fee: any[],headerId,headerType) {
      this.feeService.createFees(fee, headerId, headerType).subscribe((data: any) => {

        data.forEach((fee: any) => {

          if (fee.amount > 0) {
            this.individualProductFeeDetails.push(fee);

            var obj = $.grep(this.productFeeDetails, function (obj: any) {
              return obj.feeName == fee.feeName;
            });
            if (obj.length > 0) {

            }
            else {
              var feeDetails: any = { 'feeName': fee.feeName, 'feeType': fee.feeType, 'feeCost': fee.costType };
              this.productFeeDetails.push(feeDetails);
            }
          }
        });


  
        if (headerType == "P") {
          var product = $.grep(this.proposedProduct, function (obj: any) {
            return obj.id == headerId;
          });
          if (product.length > 0) {
            product[0].feeDetails = data;
          }
        }
        else if (headerType == "C") {
          var product = $.grep(this.currentFund, function (obj: any) {
            return obj.id == headerId;
          });
          if (product.length > 0) {
            product[0].feeDetails = data;
          }
        }
        else if (headerType == "A") {
          var product = $.grep(this.alternativeProducts, function (obj: any) {
            return obj.id == headerId;
          });
          if (product.length > 0) {
            product[0].feeDetails = data;
          }
        }

       // this.generateFeeDetails();

      // this.rebalanceProduct();
    });
    }

  switchTabs(event: MatTabChangeEvent) {
    var products = [];
    if (event.index == 3) {
      this.currentFund.forEach((cf: any) => {

        var sum = 0;
        cf.data.forEach((obj: any) => {
          sum += obj.value;
        });

        if (sum != cf.value) {
          products.push("Current - " + cf.product);
        }

      });
      this.proposedProduct.forEach((pp: any) => {

        var sum = 0;
        pp.data.forEach((obj: any) => {
          sum += obj.value;
        });

        if (sum != pp.value) {
          products.push("Proposed - " + pp.product);
        }


      });
      this.alternativeProducts.forEach((ap: any) => {
        var sum = 0;
        ap.data.forEach((obj: any) => {
          sum += obj.value;
        });

        if (sum != ap.value) {
          products.push("Alternative - " + ap.product);
        }
      });

      if (products.length > 0) {
        alert("The value of the following funds and product doesn't add up " + JSON.stringify(products));
      }
    }
  }
    
  updateFund(fund: any) {

    var productFund: any = {};
    productFund.apircode = this.investmentDetails.apircode;
    productFund.productId = this.currentSelectedProduct.productId;
    productFund.feeLabel1 = "";
    productFund.feeLabel2 = "";
    productFund.feeLabel3 = "";
    this.fundService.createFundDetails(this.investmentDetails, productFund).subscribe((data: any) => {
      var t = this;
      if (this.currentSelectedType == "A") {

        var product = $.grep(this.alternativeProducts, function (obj: any) {
          return obj.id == t.currentSelectedProduct.id;
        });
        if (product.length != 0) {
          product[0].alFunds.push(data);
        }
      }
      else if (this.currentSelectedType == "C") {
        var product = $.grep(this.currentFund, function (obj: any) {
          return obj.id == t.currentSelectedProduct.id;
        });
        if (product.length != 0) {
          product[0].ropFunds.push(data);
        }
      }
      else if (this.currentSelectedType == "P") {
        var product = $.grep(this.proposedProduct, function (obj: any) {
          return obj.id == t.currentSelectedProduct.id;
        });
        if (product.length != 0) {
          this.funds.push(data);
          this.originalFunds.push(data);
        }
      }
      else if (this.currentSelectedType == "CP") {
        this.funds.push(data);
        this.originalFunds.push(data);
      }

     
       this.onCloseNewFundDisplay();

    });

     
  }



  applySelectFilter(event, product : any,type:any) {
    if (event == null) {
      this.newfundDisplay = 'block';
      this.rebalanceOption = 'none';
      this.currentSelectedProduct = product;
      this.currentSelectedType = type;
    }
  }
}

