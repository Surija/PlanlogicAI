import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
import * as XLSX from 'xlsx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import { ActivatedRoute, Router } from '@angular/router';
import { DragulaModule, DragulaService } from 'ng2-dragula';
//import { async } from '@angular/core/testing';
//import { BasicDetails } from '../../models/Client';
import { ClientService } from './../../services/client.service';
import { NgxSelectModule, INgxSelectOption } from 'ngx-select-ex'
import { InsuranceSwitchingService } from './../../services/insurance-switching.service';
import { NeedsAnalysisService } from './../../services/needs-analysis.service';

@Component({
    selector: 'app-insurance',
    templateUrl: './insurance.component.html',
    styleUrls: ['./insurance.component.css']
})
export class InsuranceComponent implements OnInit {
  showLoadingIndicator = false;
  generateWord = 'none';
    clientDetails: any;
    selectedClient: any;
    selectedClientProduct: number = 0;
    owner: any = [];
    currentInsurance: any[] = [];
    proposedInsurance: any[] = [];
    selectedProduct: number = 0;
    allProposedInvestments: any[] = [];
    dropOptionMain = 'none';
    currentInsuranceView = 'none';
    proposedInsuranceView = 'none';
    currentLifeCoverView = 'none';
    currentTpdCoverView = 'none';
    currentTraumaCoverView = 'none';
    currentIncomeCoverView = 'none';
    isCurrent = 0;
    existingCurrent: any[] = [];

    //Objects
    feeDetails: any = {
        recId: 0,
        headerId: 0,
        feeType: 'premium',
        amount: 0,
        frequency: 'Yearly',
        specialNotes: ''

    };

    currentInsuranceDetails: any = {
        recId: 0,
        provider: '',
        owner: 'Client',
        feeDetails: [],
        lifeCover: [],
        tpdCover: [],
        traumaCover: [],
        incomeCover: []
    };

    ongoingType: any = {
        recId: 0,
        headerId: 0,
        coaType: "ongoing",
        commission: 0,
        adviser: 0,
        practice: 0,
        riadvice: 0
    };

    implementationType: any = {
        recId: 0,
        headerId: 0,
        coaType: "implementation",
        commission: 0,
        adviser: 0,
        practice: 0,
        riadvice: 0
    };

    proposedInsuranceDetails: any = {
        recId: 0,
        provider: '',
      owner: 'Client',
      replacementId: 0,
        feeDetails: [],
        lifeCover: [],
        tpdCover: [],
        traumaCover: [],
        incomeCover: [],
        //TODO:
        implementation: this.implementationType,
        ongoing: this.ongoingType,
        replacement: []

    };

    frequency = ["Yearly", "Monthly"];
    feeType = [
        { id: "premium", name: "Premium" },
        { id: "policyFee", name: "Policy Fee" },
        { id: "stampDuty", name: "Stamp Duty" },
        { id: "other", name: "Other" }
    ];
    coverType = [
        { id: "default", name: "Select cover type" },
        { id: "life", name: "Life" },
        { id: "tpd", name: "TPD" },
        { id: "trauma", name: "Trauma" },
        { id: "incomeProtection", name: "Income Protection" }
    ];

  coverTypeTemp = [
    { id: "default", name: "Select cover type" },
    { id: "life", name: "Life" },
    { id: "tpd", name: "TPD" },
    { id: "trauma", name: "Trauma" },
    { id: "incomeProtection", name: "Income Protection" }
  ];
    policyOwner = [
        { id: "superFund", name: "Super Fund" },
        { id: "partner", name: "Client" },
        { id: "client", name: "Partner" }
    ];

    coverDetails: any = {
        type: 'default',
        policyOwner: 'superFund',
        benefitAmount: 0
    };

    lifeCoverDetails: any = {
        recId: 0,
        headerId: 0,
        policyOwner: 'superFund',
        benefitAmount: 0,
        premiumType: '',
        withinSuper: 0,
        futureInsurability: 0,
        terminalIllness: 0
    };

    tpdCoverDetails: any = {
        recId: 0,
        headerId: 0,
        policyOwner: 'superFund',
        benefitAmount: 0,
        premiumType: '',

        standaloneOrLinked: '',
        definition: '',
        withinSuper: '',

        disabilityTerm: '',
        doubleTpd: 0,
        waiverOfPremium: 0,
        futureInsurability: 0

    };

    traumaCoverDetails: any = {
        recId: 0,
        headerId: 0,
        policyOwner: 'superFund',
        benefitAmount: 0,
        premiumType: '',
        standaloneOrLinked: '',
        withinSuper: '',
        reinstatement: 0,
        doubleTrauma: 0,
        childTrauma: 0,
        waiverOfPremium: 0,
    };

    incomeCoverDetails: any = {
        recId: 0,
        headerId: 0,
        policyOwner: 'superFund',
        monthlyBenefitAmount: 0,
        premiumType: '',
        withinSuper: 0,
        definition: '',
        waitingPeriod: '',
        benefitPeriod: '',
        claimsIndexation: 0,
        criticalConditionsCover: 0,
        accidentBenefit: 0,

    };

    incomeDefinition = [
        { id: "toAge", name: "to age" },
        { id: "years", name: "years" }
    ];
    benefitPeriod = [
        { id: "agreedValue", name: "Agreed Value" },
        { id: "indemnity", name: "Indemnity" }
    ];

    waitingPeriod = ["days", "months", "years"];
    premiumType = ["Level", "Stepped", "Hybrid"];
    standaloneLinkedType = ["Standalone", "Linked"];
    Definition = ["Any", "Own", "Non-Working", "Super-linked"];
    WithinSuperlinked = ["Yes", "No", "Superlinked"];

    lifeDefault = "life";
    tpdDefault = "tpd";
    traumaDefault = "trauma";
    incomeDefault = "incomeProtection";

    document: any = {};

  constructor(private route: ActivatedRoute, private needsAnalysisService: NeedsAnalysisService, private router: Router, private insuranceSwitchingService: InsuranceSwitchingService, private dragulaService: DragulaService, private clientService: ClientService) {


        dragulaService.setOptions('third-bag', {
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

    }

    private onDropModel(args: any) {

        let [el, target, source] = args;

        this.insuranceSwitchingService.getAllProposedProducts().subscribe((data: any) => {
            this.allProposedInvestments = data;
            if (this.allProposedInvestments.length > 0) {
                var t = Math.max.apply(Math, this.allProposedInvestments.map(function (o) { return o.recId; }))
                this.selectedProduct = t + 1;
            }
            else {
                this.selectedProduct = 1;
            }

            var exist = $.grep(this.proposedInsurance, function (obj: any) {
                return obj.recId == el.id;
            });
            if (exist.length != 0) {
                exist[0].recId = this.selectedProduct;
            }
            this.retainProduct();
        });


        this.selectedClientProduct = el.id;


    }

    ngOnInit() {
    

        //$(document).ready(function () {
        //    (<any>$('[data-toggle="popover"]')).popover({ html: true });
        //});

        //$(document).on('click', function (e) {
        //    $('[data-toggle="popover"],[data-original-title]').each(function () {
        //        if (!$(this).is(<any>(e.target)) && $(this).has(<any>(e.target)).length === 0 && $('.popover').has(<any>(e.target)).length === 0) {
        //            (((<any>$(this)).popover('hide').data('bs.popover') || {}).inState || {}).click = false
        //        }
        //    });
        //})

        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
        this.clientDetails = JSON.parse(localStorage.getItem('ClientDetails') || '{}');

        if (this.clientDetails.maritalStatus == "S") {
            this.owner = ["Client"];
        } else {
            this.owner = ["Client", "Partner", "Joint"];
        }

        var selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');




        var sources = [];

        sources.push(
            this.insuranceSwitchingService.getCurrentInsurance(this.selectedClient),
            this.insuranceSwitchingService.getProposedInsurance(this.selectedClient)
        );

        Observable.forkJoin(sources).subscribe((data: any) => {
            this.currentInsurance = data[0];
            this.proposedInsurance = data[1];
        }, err => {
            if (err.status == 404)
                this.router.navigate(['/home']);
        });
    }

    ngOnDestroy() {
        if (this.dragulaService.find('third-bag') !== undefined) {
            this.dragulaService.destroy('third-bag');
        }
    } 

    openCurrentModal() {
        this.dropOptionMain = 'block';
        this.currentInsuranceView = 'block';
        this.feeDetails = {
            recId: 0,
            headerId: 0,
            feeType: 'premium',
            amount: 0,
            frequency: 'Yearly',
            specialNotes: ''

        };

        this.currentInsuranceDetails = {
            recId: 0,
            provider: '',
            owner: 'Client',
            feeDetails: [],
            lifeCover: [],
            tpdCover: [],
            traumaCover: [],
            incomeCover: []
        };

        this.isCurrent = 0;
    }

    openProposedModal() {
        this.existingCurrent = [];
        this.dropOptionMain = 'block';
        this.proposedInsuranceView = 'block';
        this.feeDetails = {
            recId: 0,
            headerId: 0,
            feeType: 'premium',
            amount: 0,
            frequency: 'Yearly',
            specialNotes: ''

        };

      this.ongoingType = {
        recId: 0,
        headerId: 0,
        coaType: "ongoing",
        commission: 0,
        adviser: 0,
        practice: 0,
        riadvice: 0
      };

      this.implementationType = {
        recId: 0,
        headerId: 0,
        coaType: "implementation",
        commission: 0,
        adviser: 0,
        practice: 0,
        riadvice: 0
      };

        this.proposedInsuranceDetails = {
            recId: 0,
            provider: '',
            owner: 'Client',
            feeDetails: [],
            lifeCover: [],
            tpdCover: [],
            traumaCover: [],
            incomeCover: [],
            //TODO:
            implementation: this.implementationType,
            ongoing: this.ongoingType,
            replacement: []

        };

        this.isCurrent = 1;
        this.currentInsurance.forEach((pData: any, index: any) => {

            var obj = $.grep(this.proposedInsuranceDetails.replacement, function (obj: any) {
                return obj.currentId == pData.recId;
            });

            if (obj.length == 0) {
                var obj1: any = {};
                obj1["currentId"] = pData.recId;
                obj1["name"] = pData.provider;
                this.existingCurrent.push(obj1);
            }
        });
    }

    changeCoverType(covertype: any) {
        if (covertype == "life") {
            this.dropOptionMain = 'block';
            this.currentLifeCoverView = 'block';
            this.lifeCoverDetails = {
                recId: 0,
                headerId: 0,
                policyOwner: 'superFund',
                benefitAmount: 0,
                premiumType: '',
                withinSuper: 0,
                futureInsurability: 0,
                terminalIllness: 0
            };

        }
        else if (covertype == "tpd") {
            this.dropOptionMain = 'block';
            this.currentTpdCoverView = 'block';
            this.tpdCoverDetails = {
                recId: 0,
                headerId: 0,
                policyOwner: 'superFund',
                benefitAmount: 0,
                premiumType: '',
                standaloneOrLinked: '',
                definition: '',
                withinSuper: '',
                disabilityTerm: '',
                doubleTpd: 0,
                waiverOfPremium: 0,
                futureInsurability: 0

            };

        }
        else if (covertype == "trauma") {
            this.dropOptionMain = 'block';
            this.currentTraumaCoverView = 'block';
            this.traumaCoverDetails = {
                recId: 0,
                headerId: 0,
                policyOwner: 'superFund',
                benefitAmount: 0,
                premiumType: '',
                standaloneOrLinked: '',
                withinSuper: '',
                reinstatement: 0,
                doubleTrauma: 0,
                childTrauma: 0,
                waiverOfPremium: 0,
            };

        }
        else if (covertype == "incomeProtection") {
            this.dropOptionMain = 'block';
            this.currentIncomeCoverView = 'block';
            this.incomeCoverDetails = {
                recId: 0,
                headerId: 0,
                policyOwner: 'superFund',
                monthlyBenefitAmount: 0,
                premiumType: '',
                withinSuper: 0,
                definition: '',
                waitingPeriod: '',
                benefitPeriod: '',
                claimsIndexation: 0,
                criticalConditionsCover: 0,
                accidentBenefit: 0,

            };

        }

    }

  setSelectedCurrent(id: number) {
        this.dropOptionMain = 'block';
        this.currentInsuranceView = 'block';
        var obj = $.grep(this.currentInsurance, function (o: any) {
            return o.recId == id;
        });
    this.currentInsuranceDetails = obj[0];
  
      if (this.currentInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "life"), 1);
      }
      if (this.currentInsuranceDetails.tpdCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "tpd"), 1);
      }
    if (this.currentInsuranceDetails.traumaCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "trauma"), 1);
      }
    if (this.currentInsuranceDetails.incomeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "incomeProtection"), 1);
      }

    }

  setSelectedProduct(id: number) {

        this.existingCurrent = [];
        this.dropOptionMain = 'block';
        this.proposedInsuranceView = 'block';
        var obj = $.grep(this.proposedInsurance, function (obj: any) {
            return obj.recId == id;
        });
        var rep: any[] = [];
        obj[0].replacement.forEach((pData: any, index: any) => {
            rep.push(pData.currentId);
        });
        obj[0].replacement = rep;
        this.proposedInsuranceDetails = obj[0];

        this.currentInsurance.forEach((pData: any, index: any) => {

            var obj = $.grep(this.proposedInsuranceDetails.replacement, function (obj: any) {
                return obj.currentId == pData.recId;
            });

            if (obj.length == 0) {
                var obj1: any = {};
                obj1["currentId"] = pData.recId;
                obj1["name"] = pData.provider;
                this.existingCurrent.push(obj1);
            }
      });


    if (this.proposedInsuranceDetails.lifeCover.length == 1) {
      this.coverType.splice(this.coverType.findIndex(v => v.id === "life"), 1);
    }
    if (this.proposedInsuranceDetails.tpdCover.length == 1) {
      this.coverType.splice(this.coverType.findIndex(v => v.id === "tpd"), 1);
    }
    if (this.proposedInsuranceDetails.traumaCover.length == 1) {
      this.coverType.splice(this.coverType.findIndex(v => v.id === "trauma"), 1);
    }
    if (this.proposedInsuranceDetails.incomeCover.length == 1) {
      this.coverType.splice(this.coverType.findIndex(v => v.id === "incomeProtection"), 1);
    }
    }

    deleteproposedProduct(productId: any) {
        this.insuranceSwitchingService.deleteProposed(productId).subscribe((data: any) => {
            this.proposedInsurance.forEach((pData: any, index: any) => {
                if (pData.recId == productId) {
                    this.proposedInsurance.splice(index, 1);
                }
            });
        });
    }
    //Fee_Functions

    AddFeeDetails(data: any, type: number) {
        if (type == 0) {
            this.currentInsuranceDetails.feeDetails.push(data);
        }
        else {
            this.proposedInsuranceDetails.feeDetails.push(data);
        }

        this.feeDetails = {
            recId: 0,
            headerId: 0,
            feeType: 'premium',
            amount: 0,
            frequency: 'Yearly',
            specialNotes: ''

        };

    } 
    DeleteFeeDetails(index: any, type: number) {
        if (type == 0) {
            this.currentInsuranceDetails.feeDetails.splice(index, 1);
        }
        else {
            this.proposedInsuranceDetails.feeDetails.splice(index, 1);
        }
    }

    ////Life
  AddLifeCoverDetails(data: any, type: number) {
    if (type == 0) {
      var isExist = false;
      var id$ = data.recId;
      if (this.currentInsuranceDetails.lifeCover.length > 0) {
        this.currentInsuranceDetails.lifeCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.currentInsuranceDetails.lifeCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.currentInsuranceDetails.lifeCover.length > 0) {
          var t = Math.max.apply(Math, this.currentInsuranceDetails.lifeCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.currentInsuranceDetails.lifeCover.push(data);
      }

      if (this.currentInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "life"), 1);
      }
    }
    else {
      var isExist = false;
      var id$ = data.recId;
      if (this.proposedInsuranceDetails.lifeCover.length > 0) {
        this.proposedInsuranceDetails.lifeCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.proposedInsuranceDetails.lifeCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.proposedInsuranceDetails.lifeCover.length > 0) {
          var t = Math.max.apply(Math, this.proposedInsuranceDetails.lifeCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.proposedInsuranceDetails.lifeCover.push(data);
      }

      if (this.proposedInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "life"), 1);
      }
    }

   
    this.onCloseLifeCover();
  }
    EditLifeCoverDetails(data: any) {
        this.dropOptionMain = 'block';

        this.lifeCoverDetails = data;
        this.currentLifeCoverView = 'block';
    }
    DeleteLifeCoverDetails(index: any, type: number) {
        if (type == 0) {
          this.currentInsuranceDetails.lifeCover.splice(index, 1);

          if (this.currentInsuranceDetails.lifeCover.length == 0) {
            var obj = { id: "life", name: "Life" };
            this.coverType.push(obj);
          }
        }
        else {
          this.proposedInsuranceDetails.lifeCover.splice(index, 1);

          if (this.proposedInsuranceDetails.lifeCover.length == 0) {
            var obj = { id: "life", name: "Life" };
            this.coverType.push(obj);
          }
        }
    }

    ////TPD
  AddTpdCoverDetails(data: any, type: number) {

    if (type == 0) {
      var isExist = false;
      var id$ = data.recId;
      if (this.currentInsuranceDetails.tpdCover.length > 0) {
        this.currentInsuranceDetails.tpdCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.currentInsuranceDetails.tpdCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.currentInsuranceDetails.tpdCover.length > 0) {
          var t = Math.max.apply(Math, this.currentInsuranceDetails.tpdCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.currentInsuranceDetails.tpdCover.push(data);
      }

      if (this.currentInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "tpd"), 1);
      }
    }
    else {
      var isExist = false;
      var id$ = data.recId;
      if (this.proposedInsuranceDetails.tpdCover.length > 0) {
        this.proposedInsuranceDetails.tpdCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.proposedInsuranceDetails.tpdCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.proposedInsuranceDetails.tpdCover.length > 0) {
          var t = Math.max.apply(Math, this.proposedInsuranceDetails.tpdCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.proposedInsuranceDetails.tpdCover.push(data);
      }

      if (this.proposedInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "tpd"), 1);
      }
    }

   
    this.onCloseTpdCover();
  }
    EditTpdCoverDetails(data: any) {
        this.dropOptionMain = 'block';
        this.tpdCoverDetails = data;
        this.currentTpdCoverView = 'block';
    }
    DeleteTpdCoverDetails(index: any, type: number) {
        if (type == 0) {
          this.currentInsuranceDetails.tpdCover.splice(index, 1);

          if (this.currentInsuranceDetails.tpdCover.length == 0) {
            var obj = { id: "tpd", name: "TPD" };
            this.coverType.push(obj);
          }
        }
        else {
          this.proposedInsuranceDetails.tpdCover.splice(index, 1);

          if (this.proposedInsuranceDetails.tpdCover.length == 0) {
            var obj = { id: "tpd", name: "TPD" };
            this.coverType.push(obj);
          }
        }
    }

    //////Trauma
  AddTraumaCoverDetails(data: any, type: number) {
    if (type == 0) {
      var isExist = false;
      var id$ = data.recId;
      if (this.currentInsuranceDetails.traumaCover.length > 0) {
        this.currentInsuranceDetails.traumaCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.currentInsuranceDetails.traumaCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.currentInsuranceDetails.traumaCover.length > 0) {
          var t = Math.max.apply(Math, this.currentInsuranceDetails.traumaCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.currentInsuranceDetails.traumaCover.push(data);
      }

      if (this.currentInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "trauma"), 1);
      }
    }
    else {
      var isExist = false;
      var id$ = data.recId;
      if (this.proposedInsuranceDetails.traumaCover.length > 0) {
        this.proposedInsuranceDetails.traumaCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.proposedInsuranceDetails.traumaCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.proposedInsuranceDetails.traumaCover.length > 0) {
          var t = Math.max.apply(Math, this.proposedInsuranceDetails.traumaCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.proposedInsuranceDetails.traumaCover.push(data);
      }


      if (this.proposedInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "trauma"), 1);
      }
    }

 
    this.onCloseTraumaCover();
  }
    EditTraumaCoverDetails(data: any) {
        this.dropOptionMain = 'block';
        this.traumaCoverDetails = data;
        this.currentTraumaCoverView = 'block';
    }
    DeleteTraumaCoverDetails(index: any, type: number) {
        if (type == 0) {
          this.currentInsuranceDetails.traumaCover.splice(index, 1);

          if (this.currentInsuranceDetails.traumaCover.length == 0) {
            var obj = { id: "trauma", name: "Trauma" };
            this.coverType.push(obj);
          }
        }
        else {
          this.proposedInsuranceDetails.traumaCover.splice(index, 1);

          if (this.proposedInsuranceDetails.traumaCover.length == 0) {
            var obj = { id: "trauma", name: "Trauma" };
            this.coverType.push(obj);
          }
        }
    }

    //////Income
  AddIncomeCoverDetails(data: any, type: number) {
    if (type == 0) {
      var isExist = false;
      var id$ = data.recId;
      if (this.currentInsuranceDetails.incomeCover.length > 0) {
        this.currentInsuranceDetails.incomeCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.currentInsuranceDetails.incomeCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.currentInsuranceDetails.incomeCover.length > 0) {
          var t = Math.max.apply(Math, this.currentInsuranceDetails.incomeCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.currentInsuranceDetails.incomeCover.push(data);
      }

      if (this.currentInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "incomeProtection"), 1);
      }
    }
    else {
      var isExist = false;
      var id$ = data.recId;
      if (this.proposedInsuranceDetails.incomeCover.length > 0) {
        this.proposedInsuranceDetails.incomeCover.forEach((pData: any, index: any) => {
          if (pData.recId == id$) {
            this.proposedInsuranceDetails.incomeCover.splice(index, 1, data);
            isExist = true;

          }
        });
      }

      if (isExist == false) {

        if (this.proposedInsuranceDetails.incomeCover.length > 0) {
          var t = Math.max.apply(Math, this.proposedInsuranceDetails.incomeCover.map(function (o: any) { return o.recId; }))
          data.recId = t + 1;
        }
        else {
          data.recId = 1;
        }
        this.proposedInsuranceDetails.incomeCover.push(data);
      }


      if (this.proposedInsuranceDetails.lifeCover.length == 1) {
        this.coverType.splice(this.coverType.findIndex(v => v.id === "incomeProtection"), 1);
      }
    }

  
    this.onCloseIncomeCover();
  }
    EditIncomeCoverDetails(data: any) {
        this.dropOptionMain = 'block';
        this.incomeCoverDetails = data;
        this.currentIncomeCoverView = 'block';
    }
    DeleteIncomeCoverDetails(index: any, type: number) {
        if (type == 0) {
          this.currentInsuranceDetails.incomeCover.splice(index, 1);

          if (this.currentInsuranceDetails.incomeCover.length == 0) {
            var obj = { id: "incomeProtection", name: "Income Protection" };
            this.coverType.push(obj);
          }
        }
        else {
          this.proposedInsuranceDetails.incomeCover.splice(index, 1);

          if (this.currentInsuranceDetails.incomeCover.length == 0) {
            var obj = { id: "incomeProtection", name: "Income Protection" };
            this.coverType.push(obj);
          }
        }
    }

    //Close_Functions
  onCloseInsurance() {
    var sources = [];

    sources.push(
      this.insuranceSwitchingService.getCurrentInsurance(this.selectedClient),
      this.insuranceSwitchingService.getProposedInsurance(this.selectedClient)
    );

    Observable.forkJoin(sources).subscribe((data: any) => {
      this.currentInsurance = data[0];
      this.proposedInsurance = data[1];
    }, err => {
      if (err.status == 404)
        this.router.navigate(['/home']);
      });

        this.dropOptionMain = 'none';
        this.currentInsuranceView = 'none';
        this.proposedInsuranceView = 'none';
        this.feeDetails = {
            recId: 0,
            headerId: 0,
            feeType: 'premium',
            amount: 0,
            frequency: 'Yearly',
            specialNotes: ''

        };

        this.currentInsuranceDetails = {
            recId: 0,
            provider: '',
            owner: 'Client',
            feeDetails: [],
            lifeCover: [],
            tpdCover: [],
            traumaCover: [],
            incomeCover: []
        };

        this.ongoingType = {
            recId: 0,
            headerId: 0,
            coaType: "ongoing",
            commission: 0,
            adviser: 0,
            practice: 0,
            riadvice: 0
        };

        this.implementationType = {
            recId: 0,
            headerId: 0,
            coaType: "implementation",
            commission: 0,
            adviser: 0,
            practice: 0,
            riadvice: 0
        };

        this.proposedInsuranceDetails = {
            recId: 0,
            provider: '',
            owner: 'Client',
            feeDetails: [],
            lifeCover: [],
            tpdCover: [],
            traumaCover: [],
            incomeCover: [],
          implementation: this.implementationType,
          ongoing: this.ongoingType,
            replacement: []
        };

      this.coverType = [
        { id: "default", name: "Select cover type" },
        { id: "life", name: "Life" },
        { id: "tpd", name: "TPD" },
        { id: "trauma", name: "Trauma" },
        { id: "incomeProtection", name: "Income Protection" }
      ];

  }
  onCloseLifeCover() {
    this.dropOptionMain = 'block';
    if (this.isCurrent == 0) {
      this.currentInsuranceView = 'block';
    }
    else {
      this.proposedInsuranceView = 'block';
    }
    this.currentLifeCoverView = 'none';
    this.lifeCoverDetails = {
      recId: 0,
      headerRecId: 0,
      policyOwner: 'superFund',
      benefitAmount: 0,
      premiumType: '',
      withinSuper: 0,
      futureInsurability: 0,
      terminalIllness: 0
    };
    this.coverDetails = {
      type: 'default',
      policyOwner: 'superFund',
      benefitAmount: 0
    };
  }
  onCloseTpdCover() {
    this.dropOptionMain = 'block';
    if (this.isCurrent == 0) {
      this.currentInsuranceView = 'block';
    }
    else {
      this.proposedInsuranceView = 'block';
    }
    this.currentTpdCoverView = 'none';
    this.tpdCoverDetails = {
      recId: 0,
      headerRecId: 0,
      policyOwner: 'superFund',
      benefitAmount: 0,
      premiumType: '',
      standaloneOrLinked: '',
      definition: '',
      withinSuper: '',
      disabilityTerm: '',
      doubleTpd: 0,
      waiverOfPremium: 0,
      futureInsurability: 0
    };
    this.coverDetails = {
      type: 'default',
      policyOwner: 'superFund',
      benefitAmount: 0
    };
  }
  onCloseTraumaCover() {
    this.dropOptionMain = 'block';
    if (this.isCurrent == 0) {
      this.currentInsuranceView = 'block';
    }
    else {
      this.proposedInsuranceView = 'block';
    }
    this.currentTraumaCoverView = 'none';
    this.traumaCoverDetails = {
      recId: 0,
      headerRecId: 0,
      policyOwner: 'superFund',
      benefitAmount: 0,
      premiumType: '',
      standaloneOrLinked: '',
      withinSuper: '',
      reinstatement: 0,
      doubleTrauma: 0,
      childTrauma: 0,
      waiverOfPremium: 0,
    };
    this.coverDetails = {
      type: 'default',
      policyOwner: 'superFund',
      benefitAmount: 0
    };
  }
  onCloseIncomeCover() {
    this.dropOptionMain = 'block';
    if (this.isCurrent == 0) {
      this.currentInsuranceView = 'block';
    }
    else {
      this.proposedInsuranceView = 'block';
    }
    this.currentIncomeCoverView = 'none';
    this.incomeCoverDetails = {
      recId: 0,
      headerRecId: 0,
      policyOwner: 'superFund',
      monthlyBenefitAmount: 0,
      premiumType: '',
      withinSuper: 0,
      definition: '',
      waitingPeriod: '',
      benefitPeriod: '',
      claimsIndexation: 0,
      criticalConditionsCover: 0,
      accidentBenefit: 0,

    };
    this.coverDetails = {
      type: 'default',
      policyOwner: 'superFund',
      benefitAmount: 0
    };
  }

    updateInsurance(data: any, type: number) {

        if (type == 0) {
            this.insuranceSwitchingService.addCurrentInsurance(data, this.selectedClient).subscribe((data: any) => {
                var isExist = false;
                var id$ = data.recId;
                if (this.currentInsurance.length > 0) {
                    this.currentInsurance.forEach((pData: any, index: any) => {
                        if (pData.recId == id$) {
                            this.currentInsurance.splice(index, 1, data);
                            isExist = true;

                        }
                    });
                }

                if (isExist == false) {
                    this.currentInsurance.push(data);
                }

                this.currentInsuranceDetails = {
                    recId: 0,
                    provider: '',
                    owner: 'Client',
                    feeDetails: [],
                    lifeCover: [],
                    tpdCover: [],
                    traumaCover: [],
                    incomeCover: []
                };

                this.onCloseInsurance();
            });
        }
        else {
          console.log(data);
          var rep: any[] = [];

          if (data.replacement.length > 0) {
            data.replacement.forEach((pData: any, index: any) => {
              var obj1: any = {};
              obj1["currentId"] = pData;
              obj1["recId"] = 0;
              rep.push(obj1);
            });
          }
            data.replacement = rep;
            data.implementation.coaType = "implementation";
            data.ongoing.coaType = "ongoing";

            this.insuranceSwitchingService.addProposedInsurance(data, this.selectedClient).subscribe((data: any) => {
                var isExist = false;
                var id$ = data.recId;
                if (this.proposedInsurance.length > 0) {
                    this.proposedInsurance.forEach((pData: any, index: any) => {
                        if (pData.recId == id$) {
                            this.proposedInsurance.splice(index, 1, data);
                            isExist = true;

                        }
                    });
                }

                if (isExist == false) {
                    this.proposedInsurance.push(data);
                }
                this.ongoingType = {
                    recId: 0,
                    headerId: 0,
                    coaType: "ongoing",
                    commission: 0,
                    adviser: 0,
                    practice: 0,
                    riadvice: 0
                };

                this.implementationType = {
                    recId: 0,
                    headerId: 0,
                    coaType: "implementation",
                    commission: 0,
                    adviser: 0,
                    practice: 0,
                    riadvice: 0
                };

                this.proposedInsuranceDetails = {
                    recId: 0,
                    provider: '',
                    owner: 'Client',
                    feeDetails: [],
                    lifeCover: [],
                    tpdCover: [],
                    traumaCover: [],
                    incomeCover: [],
                  implementation: this.implementationType,
                  ongoing: this.ongoingType,
                    replacement: []
                };

                this.onCloseInsurance();
            });
        }
    }

  retainProduct() {

    var selectedProd = this.selectedProduct;
    var obj = $.grep(this.proposedInsurance, function (obj: any) {
      return obj.recId == selectedProd;
    });

    obj[0].replacementId = this.selectedClientProduct;

    console.log(obj[0]);
    this.insuranceSwitchingService.retainProduct(obj[0], this.selectedClientProduct, this.selectedClient).subscribe((data: any) => {


      this.dropOptionMain = 'block';
      this.proposedInsuranceView = 'block';

      this.selectedProduct = data.recId;
      this.setProposedInvestment(this.selectedProduct);

    });
  }

  setProposedInvestment(recId: number) {
    this.insuranceSwitchingService.getProposedInsurance(this.selectedClient).subscribe((data: any) => {
      this.proposedInsurance = data;

      var obj = $.grep(this.proposedInsurance, function (obj: any) {
        return obj.recId == recId;
      });
      this.proposedInsuranceDetails = obj[0];
    });
  }

  generateWordDocument() {
    this.showLoadingIndicator = true;
    this.generateWord = "none";
    this.dropOptionMain = 'block';

    this.needsAnalysisService.getNeedsAnalysis(this.selectedClient).subscribe((data: any) => {
      var needsAnalysis = data;

    
      this.document.currentInsurance = this.currentInsurance;
      this.document.proposedInsurance = this.proposedInsurance;
      this.document.needsAnalysis = needsAnalysis;
      this.document.clientDetails = this.clientDetails;

      var fileName = this.clientDetails.familyName.trim() + "," + this.clientDetails.clientName.trim() + " - Insurance.docx";
      this.insuranceSwitchingService.generateWord(this.document).subscribe(blob => {
      this.generateWord = 'block';
      this.showLoadingIndicator = false;
      this.dropOptionMain = 'none';
      saveAs(blob, fileName);

    });





    });
  }
}
