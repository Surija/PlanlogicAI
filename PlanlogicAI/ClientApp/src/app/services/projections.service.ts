import { Injectable, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Client, BasicDetails} from '../models/Client';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { CashFlow } from '../models/CashFlow';
import { CashFlowService } from './cashFlow.service';
import { ClientService } from './client.service';
import { CommonService } from './common.service';
import 'rxjs/add/Observable/forkJoin';
import * as $ from 'jquery';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()

export class ProjectionService {

  private readonly cFEndpoint = '/api/projections';
  constructor(private http: HttpClient) { }

  //projections(clientId: number) {
  //  return this.http.get(this.cFEndpoint + '/' + clientId);
  //}

  projections(id: number) {
    return this.http.get(this.cFEndpoint + '/' + id);
  }

}

//export class ProjectionService implements OnInit {
//    clientAge: number = 0;
//    partnerAge: number = 0;
//    years: any[] = [];
//    selectedClient: any = {};
//    cashFlowIncome: CashFlow[] = [];
//    cashFlowExpenditure: CashFlow[] = [];
  
//    period: number = 0;
//    marginalTaxRates: any[] = [];

//    cfiClient: any = [];
//    cfiPartner: any = [];
//    EPRTClient: any = [];
//    EPRTPartner: any = [];
//    EPRTJoint: any = [];

//    //Client
//    public Income: any[] = [];

//    public ClientDeductions: any[] = [];
//    public PartnerDeductions: any[] = [];

//    public clientTaxableIncome: any[] = [];
//    public clientLossAdjustment: any[] = [];
//    public clientNRTaxOffset: any[] = [];
//    public clientRTaxOffset: any[] = [];
//    public clientMedicareLevy: any[] = [];
//    public clientGrossTax: any[] = [];

//    //Partner

//    public partnerTaxableIncome: any[] = [];
//    public partnerLossAdjustment: any[] = [];
//    public partnerNRTaxOffset: any[] = [];
//    public partnerRTaxOffset: any[] = [];
//    public partnerMedicareLevy: any[] = [];

//    public GrossTax: any[] = [];
//    public NetPayable: any[] = [];
//    public TotalPayable: any[] = [];

//    clientRetirementYear: number = 0;
//    partnerRetirementYear: number = 0;

//    total: number = 10;
//    constructor(public route: ActivatedRoute,
//        public router: Router, public cashFlowService: CashFlowService,
//        public clientService: ClientService, public commonService: CommonService) { }

//    ngOnInit() {

//        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
//        var years = [];
//        var sources = [];
//        var age: number;
//        this.period = this.selectedClient.period + 1;

//        this.clientRetirementYear = this.selectedClient.clientRetirementYear;
//        this.partnerRetirementYear = this.selectedClient.partnerRetirementYear;

//        if (this.selectedClient.clientId) {
//            sources.push(this.cashFlowService.getCashFlows(this.selectedClient.clientId, "I"));
//            sources.push(this.cashFlowService.getCashFlows(this.selectedClient.clientId, "E"));
//            sources.push(this.commonService.getMarginalTaxRates());
//        }

//        Observable.forkJoin(sources).subscribe((data: any) => {
//            if (this.selectedClient.clientId) {
//                this.cashFlowIncome = data[0];
//                this.cashFlowExpenditure = data[1];
//                this.marginalTaxRates = data[2];
//                this.marginalTaxRates = this.marginalTaxRates.sort(function (obj1: any, obj2: any) {
//                    return obj1.index - obj2.index;
//                });
//                this.cfiClient = this.cashFlowIncome.filter(c => c.owner === "Client");
//                this.cfiPartner = this.cashFlowIncome.filter(c => c.owner === "Partner");

//                this.EPRTClient = this.cashFlowExpenditure.filter(c => c.owner === "Client").filter(r => r.type === "Pre-tax");
//                this.EPRTJoint = this.cashFlowExpenditure.filter(c => c.owner === "Joint").filter(r => r.type === "Pre-tax");
//                this.EPRTPartner = this.cashFlowExpenditure.filter(c => c.owner === "Partner").filter(r => r.type === "Pre-tax");




//                //Calculate Assessible Income
//                //var indexRangeCIncome: any = [];
//                this.cfiClient.forEach((x: any) => { // client
//                    var obj: any = {};
//                    var obj1: any = {};
//                    obj["owner"] = x.owner;
//                    obj["name"] = x.cfname;
//                    var j = 1;
//                    for (var i = 0; i < this.selectedClient.period; i++) {

//                        if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
//                            if (j == 1) {
//                                obj1[this.selectedClient.startDate + i] = x.value.toFixed();

//                            }
//                            else {
//                                obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
//                            }
//                            j++;
//                        }
//                        else {
//                            obj1[this.selectedClient.startDate + i] = 0;
//                        }
//                        obj["values"] = obj1;

//                    }
//                    //indexRangeCIncome.push(obj);
//                    this.Income.push(obj);
//                })


//                //var indexRangePIncome: any = [];
//                this.cfiPartner.forEach((x: any) => { // partner
//                    var obj: any = {};
//                    var obj1: any = {};
//                    obj["owner"] = x.owner;
//                    obj["name"] = x.cfname;
//                    var j = 1;
//                    for (var i = 0; i < this.selectedClient.period; i++) {

//                        if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
//                            if (j == 1) {
//                                obj1[this.selectedClient.startDate + i] = x.value.toFixed();

//                            }
//                            else {
//                                obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
//                            }
//                            j++;
//                        }
//                        else {
//                            obj1[this.selectedClient.startDate + i] = 0;
//                        }
//                        obj["values"] = obj1;

//                    }
//                    //indexRangePIncome.push(obj);
//                    this.Income.push(obj);
//                })



//                //Calculate Deductions

//                this.EPRTClient.forEach((x: any) => { // client
//                    var obj: any = {};
//                    var obj1: any = {};
//                    obj["owner"] = x.owner;
//                    obj["name"] = x.cfname;
//                    var j = 1;
//                    for (var i = 0; i < this.selectedClient.period; i++) {

//                        if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
//                            if (j == 1) {
//                                obj1[this.selectedClient.startDate + i] = x.value.toFixed();

//                            }
//                            else {
//                                obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
//                            }
//                            j++;
//                        }
//                        else {
//                            obj1[this.selectedClient.startDate + i] = 0;
//                        }
//                        obj["values"] = obj1;

//                    }
//                    //indexRangeDeduction.push(obj);
//                    this.ClientDeductions.push(obj);
//                })

//                this.EPRTPartner.forEach((x: any) => { // client
//                    var obj: any = {};
//                    var obj1: any = {};
//                    obj["owner"] = x.owner;
//                    obj["name"] = x.cfname;
//                    var j = 1;
//                    for (var i = 0; i < this.selectedClient.period; i++) {

//                        if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
//                            if (j == 1) {
//                                obj1[this.selectedClient.startDate + i] = x.value.toFixed();

//                            }
//                            else {
//                                obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
//                            }
//                            j++;
//                        }
//                        else {
//                            obj1[this.selectedClient.startDate + i] = 0;
//                        }
//                        obj["values"] = obj1;

//                    }
//                    //indexRangeDeduction.push(obj);
//                    this.PartnerDeductions.push(obj);
//                })

//                this.EPRTJoint.forEach((x: any) => { // joint
//                    var obj: any = {};
//                    var obj1: any = {};
//                    obj["owner"] = "Client";
//                    obj["name"] = x.cfname;
//                    var j = 1;
//                    for (var i = 0; i < this.selectedClient.period; i++) {

//                        if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
//                            if (j == 1) {
//                                obj1[this.selectedClient.startDate + i] = (x.value / 2).toFixed();

//                            }
//                            else {
//                                obj1[this.selectedClient.startDate + i] = ((x.value * (Math.pow((1 + x.indexation / 100), j))) / 2).toFixed();
//                            }
//                            j++;
//                        }
//                        else {
//                            obj1[this.selectedClient.startDate + i] = 0;
//                        }
//                        obj["values"] = obj1;

//                    }
//                    this.ClientDeductions.push(obj);
//                })
//                this.EPRTJoint.forEach((x: any) => { // joint
//                    var obj: any = {};
//                    var obj1: any = {};
//                    obj["owner"] = "Partner";
//                    obj["name"] = x.cfname;
//                    var j = 1;
//                    for (var i = 0; i < this.selectedClient.period; i++) {

//                        if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
//                            if (j == 1) {
//                                obj1[this.selectedClient.startDate + i] = (x.value / 2).toFixed();

//                            }
//                            else {
//                                obj1[this.selectedClient.startDate + i] = ((x.value * (Math.pow((1 + x.indexation / 100), j))) / 2).toFixed();
//                            }
//                            j++;
//                        }
//                        else {
//                            obj1[this.selectedClient.startDate + i] = 0;
//                        }
//                        obj["values"] = obj1;

//                    }
//                    this.PartnerDeductions.push(obj);
//                })
//                // this.clientDeductions = indexRangeDeduction;


//                //Calculate Totals
//                this.calculateTotalIncome("Total-client", "Client");
//                this.calculateTotalIncome("Total-partner", "Partner");
//                this.calculateTotalAssessibleIncome("ClientAssessibleIncome", "Total-client");
//                this.calculateTotalAssessibleIncome("PartnerAssessibleIncome", "Total-partner");

//                this.calculateClientTotalDeductions("ClientDeductions", "Client");
//                this.calculatePartnerTotalDeductions("PartnerDeductions", "Partner");


//                this.calculateClientTaxableIncome();
//                this.calculatePartnerTaxableIncome();

//                this.calculateLowIncomeTO("ClientLowIncomeTO", "ClientTaxableIncome", "Client");
//                this.calculateLowIncomeTO("PartnerLowIncomeTO", "PartnerTaxableIncome", "Partner");


//                this.calculateRefundableTaxOffset("ClientFrankingCredits", "Client");
//                this.calculateRefundableTaxOffset("PartnerFrankingCredits", "Partner");

//                this.calculateClientTotalNRTaxOffset("ClientTotalTO", "ClientLowIncomeTO");
//                this.calculatePartnerTotalNRTaxOffset("PartnerTotalTO", "PartnerLowIncomeTO");

//                this.calculateMedicareLevy("ClientMedicareLevy", "ClientTaxableIncome", "Client");
//                this.calculateMedicareLevy("PartnerMedicareLevy", "PartnerTaxableIncome", "Partner");

//                this.calculateGrossTax("ClientGrossTax", "ClientTaxableIncome", "Client");
//                this.calculateGrossTax("PartnerGrossTax", "PartnerTaxableIncome", "Partner");

//                this.calculateTaxPayableNonRefundable("ClientTPNonRefundable", "ClientGrossTax", "ClientTotalTO", "Client");
//                this.calculateTaxPayableNonRefundable("PartnerTPNonRefundable", "PartnerGrossTax", "PartnerTotalTO", "Partner");

//                this.calculateTaxPayableRefundable("ClientTPRefundable", "ClientTPNonRefundable", "ClientFrankingCredits", "Client");
//                this.calculateTaxPayableRefundable("PartnerTPRefundable", "PartnerTPNonRefundable", "PartnerFrankingCredits", "Partner");

//                this.calculateTotalTaxesPayable("ClientTotalTaxPayable", "ClientTPRefundable", "ClientMedicareLevy", "Client");
//                this.calculateTotalTaxesPayable("PartnerTotalTaxPayable", "PartnerTPRefundable", "PartnerMedicareLevy", "Partner");

//                this.calculateAverageTaxRate("ClientAverageTaxRate", "ClientTotalTaxPayable", "ClientAssessibleIncome", "Client");
//                this.calculateAverageTaxRate("PartnerAverageTaxRate", "PartnerTotalTaxPayable", "PartnerAssessibleIncome", "Partner");

//                this.calculateMarginalTaxRate("ClientMarginalTaxRate", "ClientTaxableIncome", "Client");
//                this.calculateMarginalTaxRate("PartnerMarginalTaxRate", "PartnerTaxableIncome", "Partner");
//            }

//        }, err => {
//            if (err.status == 404)
//                this.router.navigate(['/home']);
//        });

//        this.setYear();
//        this.setClientAge();
//        if (this.selectedClient.maritalStatus == 'M') {
//            this.setPartnerAge();
//        }


//        $(document).ready(function () {
//            //$('.client').click(function () {
//            //    $(this).nextUntil('tr.partner').toggle();
//            //});
//            $('.header').click(function () {
//                $(this).nextUntil('tr.header').toggle();
//                // alert($(this).find("span:eq(0)").attr('class'));
//                $(this).find("span:eq(0)").toggleClass("glyphicon-chevron-down").toggleClass("glyphicon-chevron-up");
//            });

//        });
//    }

//    public calculateTotalIncome(owner: string, filter: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Income";
//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let sum: number = 0;
//            this.Income.forEach((x: any) => {
//                if (x.owner == filter) {
//                    if (x.values[this.selectedClient.startDate + i] != "-") {
//                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
//                        sum = sum + t;
//                    }
//                }
//            });
//            if (sum > 0) {
//                totalVal[this.selectedClient.startDate + i] = sum;
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.Income.push(total);
//    }
//    public calculateTotalAssessibleIncome(owner: string, filter: string) {
//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "ClientAssessibleIncome";
//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let sum: number = 0;
//            this.Income.forEach((x: any) => {
//                if (x.owner == filter) {
//                    if (x.values[this.selectedClient.startDate + i] != "-") {
//                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
//                        sum = sum + t;
//                    }
//                }
//            });
//            if (sum > 0) {
//                totalVal[this.selectedClient.startDate + i] = sum;
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.Income.push(total);



//    }
//    public calculateClientTotalDeductions(owner: string, filter: string) {
//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = owner;
//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let sum: number = 0;
//            this.ClientDeductions.forEach((x: any) => {
//                if (x.owner == filter || x.owner == "Joint") {
//                    if (x.values[this.selectedClient.startDate + i] != "-") {
//                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
//                        sum = sum + t;
//                    }
//                }
//            });
//            if (sum > 0) {
//                totalVal[this.selectedClient.startDate + i] = sum;
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.ClientDeductions.push(total);
//    }
//    public calculatePartnerTotalDeductions(owner: string, filter: string) {
//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = owner;
//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let sum: number = 0;
//            this.PartnerDeductions.forEach((x: any) => {
//                if (x.owner == filter || x.owner == "Joint") {
//                    if (x.values[this.selectedClient.startDate + i] != "-") {
//                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
//                        sum = sum + t;
//                    }
//                }
//            });
//            if (sum > 0) {
//                totalVal[this.selectedClient.startDate + i] = sum;
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.PartnerDeductions.push(total);
//    }
//    public calculateClientTaxableIncome() {

//        var taxable: any = {};
//        var adjustment: any = {};
//        var taxableVal: any = {};
//        var adjustmentVal: any = {};
//        taxable["owner"] = "ClientTaxableIncome";
//        taxable["name"] = "ClientTaxableIncome";
//        adjustment["owner"] = "ClientAdjustment";
//        adjustment["name"] = "ClientAdjustment";
//        let lossG: number = 0;
//        let lossBF: number = 0;
//        let lossAdj: number = 0;
//        var income = this.Income.filter(c => c.owner === "ClientAssessibleIncome");
//        var deduction = this.ClientDeductions.filter(c => c.owner === "ClientDeductions");

//        for (var i = 0; i < this.selectedClient.period; i++) {

//            //Gti stands for ‘Gross taxable income’

//            let taxInc: number = 0;

//            if (typeof income[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(income[0].values[this.selectedClient.startDate + i]) && typeof deduction[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(deduction[0].values[this.selectedClient.startDate + i])) {

//                let Gti: number = Math.max(0, (income[0].values[this.selectedClient.startDate + i] - deduction[0].values[this.selectedClient.startDate + i]));
//                if (Gti > 0) {
//                    lossAdj = Math.min(lossBF, Gti);
//                }


//                if (deduction[0].values[this.selectedClient.startDate + i] > income[0].values[this.selectedClient.startDate + i]) {
//                    lossG = deduction[0].values[this.selectedClient.startDate + i] - income[0].values[this.selectedClient.startDate + i];
//                }

//                //LossCF stands for ‘Loss carried forward’
//                let lossCF: number = lossG + lossBF - lossAdj;
//                lossBF = lossCF;

//                taxInc = Gti - lossAdj;

//            }

//            if (lossAdj > 0) {
//                adjustmentVal[this.selectedClient.startDate + i] = lossAdj;
//            }
//            else {
//                adjustmentVal[this.selectedClient.startDate + i] = 0;
//            }
//            adjustment["values"] = adjustmentVal;


//            if (taxInc > 0) {
//                taxableVal[this.selectedClient.startDate + i] = taxInc;
//            }
//            else {
//                taxableVal[this.selectedClient.startDate + i] = 0;
//            }
//            taxable["values"] = taxableVal;

//        }
//        this.clientTaxableIncome.push(taxable);
//        this.clientLossAdjustment.push(adjustment);

//    }
//    public calculatePartnerTaxableIncome() {

//        var taxable: any = {};
//        var adjustment: any = {};
//        var taxableVal: any = {};
//        var adjustmentVal: any = {};
//        taxable["owner"] = "PartnerTaxableIncome";
//        taxable["name"] = "PartnerTaxableIncome";
//        adjustment["owner"] = "PartnerAdjustment";
//        adjustment["name"] = "PartnerAdjustment";
//        let lossG: number = 0;
//        let lossBF: number = 0;
//        let lossAdj: number = 0;
//        var income = this.Income.filter(c => c.owner === "PartnerAssessibleIncome");
//        var deduction = this.PartnerDeductions.filter(c => c.owner === "PartnerDeductions");
//        for (var i = 0; i < this.selectedClient.period; i++) {

//            //Gti stands for ‘Gross taxable income’

//            let taxInc: number = 0;

//            if (typeof income[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(income[0].values[this.selectedClient.startDate + i]) && typeof deduction[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(deduction[0].values[this.selectedClient.startDate + i])) {

//                let Gti: number = Math.max(0, (income[0].values[this.selectedClient.startDate + i] - deduction[0].values[this.selectedClient.startDate + i]));
//                if (Gti > 0) {
//                    lossAdj = Math.min(lossBF, Gti);
//                }


//                if (deduction[0].values[this.selectedClient.startDate + i] > income[0].values[this.selectedClient.startDate + i]) {
//                    lossG = deduction[0].values[this.selectedClient.startDate + i] - income[0].values[this.selectedClient.startDate + i];
//                }

//                //LossCF stands for ‘Loss carried forward’
//                let lossCF: number = lossG + lossBF - lossAdj;
//                lossBF = lossCF;

//                taxInc = Gti - lossAdj;

//            }

//            if (lossAdj > 0) {
//                adjustmentVal[this.selectedClient.startDate + i] = lossAdj;
//            }
//            else {
//                adjustmentVal[this.selectedClient.startDate + i] = 0;
//            }
//            adjustment["values"] = adjustmentVal;


//            if (taxInc > 0) {
//                taxableVal[this.selectedClient.startDate + i] = taxInc;
//            }
//            else {
//                taxableVal[this.selectedClient.startDate + i] = 0;
//            }
//            taxable["values"] = taxableVal;

//        }
//        this.partnerTaxableIncome.push(taxable);
//        this.partnerLossAdjustment.push(adjustment);

//    }
//    public calculateLowIncomeTO(owner: string, filter: string, type: string) {
//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Low income tax offset";
//        var taxableIncome: any = {};
//        if (type == "Client") { taxableIncome = this.clientTaxableIncome.filter(c => c.owner === filter); }
//        else if (type == "Partner") { taxableIncome = this.partnerTaxableIncome.filter(c => c.owner === filter); }

//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let lito: number = 0;

//            if (typeof taxableIncome[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.selectedClient.startDate + i])) {

//                let val: number = taxableIncome[0].values[this.selectedClient.startDate + i];
//                if (val <= 37000) {
//                    lito = 445;
//                } else if (val > 37000 && val <= 66667) {
//                    lito = 445 - ((val - 37000) * 0.015)

//                } else if (val > 66667) {
//                    lito = 0;
//                }

//            }


//            if (lito > 0) {
//                totalVal[this.selectedClient.startDate + i] = lito.toFixed();
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }
//        if (type == "Client") {
//            this.clientNRTaxOffset.push(total);
//        } else if (type == "Partner") {
//            this.partnerNRTaxOffset.push(total);
//        }

//    }
//    public calculateClientTotalNRTaxOffset(owner: string, filter: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Total non-refundable tax offsets";
//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let sum: number = 0;
//            this.clientNRTaxOffset.forEach((x: any) => {
//                if (x.owner == filter) {
//                    if (x.values[this.selectedClient.startDate + i] != "-") {
//                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
//                        sum = sum + t;
//                    }
//                }
//            });
//            if (sum > 0) {
//                totalVal[this.selectedClient.startDate + i] = sum.toFixed();
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.clientNRTaxOffset.push(total);
//    }
//    public calculatePartnerTotalNRTaxOffset(owner: string, filter: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Total non-refundable tax offsets";
//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let sum: number = 0;
//            this.partnerNRTaxOffset.forEach((x: any) => {
//                if (x.owner == filter) {
//                    if (x.values[this.selectedClient.startDate + i] != "-") {
//                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
//                        sum = sum + t;
//                    }
//                }
//            });
//            if (sum > 0) {
//                totalVal[this.selectedClient.startDate + i] = sum.toFixed();
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.partnerNRTaxOffset.push(total);
//    }
//    public calculateRefundableTaxOffset(owner: string, type: string) {
//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Franking Credits";
//        //var franking: any = {};
//        //if (type == "Client") { taxableIncome = this.clientTaxableIncome.filter(c => c.owner === filter); }
//        //else if (type == "Partner") { taxableIncome = this.partnerTaxableIncome.filter(c => c.owner === filter); }

//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let fc: number = 0;

//            //if (typeof taxableIncome[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.selectedClient.startDate + i])) {

//            //    let val: number = taxableIncome[0].values[this.selectedClient.startDate + i];
//            //    if (val <= 37000) {
//            //        lito = 445;
//            //    } else if (val > 37000 && val <= 66667) {
//            //        lito = 445 - ((val - 37000) * 0.015)

//            //    } else if (val > 66667) {
//            //        lito = 0;
//            //    }

//            //}


//            //if (fc > 0) {
//            totalVal[this.selectedClient.startDate + i] = fc.toFixed();
//            //} else {
//            //    totalVal[this.selectedClient.startDate + i] = 0;
//            //}
//            total["values"] = totalVal;

//        }
//        if (type == "Client") {
//            this.clientRTaxOffset.push(total);
//        } else if (type == "Partner") {
//            this.partnerRTaxOffset.push(total);
//        }

//    }
//    //TODO: get medicare levy (2) from database
//    public calculateMedicareLevy(owner: string, filter: string, type: string) {
//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Medicare levy";
//        var taxableIncome: any = {};
//        if (type == "Client") { taxableIncome = this.clientTaxableIncome.filter(c => c.owner === filter); }
//        else if (type == "Partner") { taxableIncome = this.partnerTaxableIncome.filter(c => c.owner === filter); }


//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let ml: number = 0;

//            if (typeof taxableIncome[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.selectedClient.startDate + i])) {

//                let val: number = taxableIncome[0].values[this.selectedClient.startDate + i];
//                ml = (val * 2) / 100;

//            }


//            if (ml > 0) {
//                totalVal[this.selectedClient.startDate + i] = ml.toFixed();
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }
//        if (type == "Client") {
//            this.clientMedicareLevy.push(total);
//        } else if (type == "Partner") {
//            this.partnerMedicareLevy.push(total);
//        }
//    }
//    public calculateGrossTax(owner: string, filter: string, type: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Gross tax payable";
//        var taxableIncome: any = {};
//        if (type == "Client") { taxableIncome = this.clientTaxableIncome.filter(c => c.owner === filter); }
//        else if (type == "Partner") { taxableIncome = this.partnerTaxableIncome.filter(c => c.owner === filter); }

//        let index: number = 0;
//        Math.max.apply(Math, this.marginalTaxRates.map(function (o: any) { if (o.index > index) index = o.index; }))
//        for (var i = 0; i < this.selectedClient.period; i++) {

//            let totalGrTax: number = 0;
//            let incTaxed: number = 0;

//            for (var j = index; j >= 1; j--) {
//                let threshold = this.marginalTaxRates.filter(c => c.index === (j - 1));
//                let rate = this.marginalTaxRates.filter(c => c.index === j);
//                if (typeof taxableIncome[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.selectedClient.startDate + i])) {
//                    let inc: number = Math.max(0, taxableIncome[0].values[this.selectedClient.startDate + i] - incTaxed - threshold[0].threshold);
//                    let grTax: number = inc * rate[0].rate;
//                    totalGrTax = totalGrTax + grTax;
//                    incTaxed = incTaxed + inc;
//                }

//            }

//            if (totalGrTax > 0) {
//                totalVal[this.selectedClient.startDate + i] = totalGrTax.toFixed();
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }
//        this.GrossTax.push(total);



//    }

//    public calculateTaxPayableNonRefundable(owner: string, filter1: string, filter2: string, type: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Tax payable after non-refundable tax offsets";

//        var grossTax: any = {};
//        var nRTaxOffset: any = {};

//        if (type == "Client") {
//            grossTax = this.GrossTax.filter(c => c.owner === filter1);
//            nRTaxOffset = this.clientNRTaxOffset.filter(c => c.owner === filter2);

//        }
//        else if (type == "Partner") {
//            grossTax = this.GrossTax.filter(c => c.owner === filter1);
//            nRTaxOffset = this.partnerNRTaxOffset.filter(p => p.owner === filter2);
//        }


//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let val: number = 0;


//            val = Math.max(0, (grossTax[0].values[this.selectedClient.startDate + i] - nRTaxOffset[0].values[this.selectedClient.startDate + i]));

//            if (val > 0) {
//                totalVal[this.selectedClient.startDate + i] = val;
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.NetPayable.push(total);
//    }
//    public calculateTaxPayableRefundable(owner: string, filter1: string, filter2: string, type: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Tax payable after refundable tax offsets";

//        var tp: any = {};
//        var rTaxOffset: any = {};

//        if (type == "Client") {
//            tp = this.NetPayable.filter(c => c.owner === filter1);
//            rTaxOffset = this.clientRTaxOffset.filter(c => c.owner === filter2);

//        }
//        else if (type == "Partner") {
//            tp = this.NetPayable.filter(c => c.owner === filter1);
//            rTaxOffset = this.partnerRTaxOffset.filter(p => p.owner === filter2);
//        }


//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let val: number = 0;


//            val = tp[0].values[this.selectedClient.startDate + i] - rTaxOffset[0].values[this.selectedClient.startDate + i];


//            totalVal[this.selectedClient.startDate + i] = val;

//            total["values"] = totalVal;

//        }

//        this.NetPayable.push(total);
//    }
//    public calculateTotalTaxesPayable(owner: string, filter1: string, filter2: string, type: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Total taxes payable";

//        var netTax: any = {};
//        var medicare: any = {};

//        if (type == "Client") {
//            netTax = this.NetPayable.filter(c => c.owner === filter1);
//            medicare = this.clientMedicareLevy.filter(c => c.owner === filter2);

//        }
//        else if (type == "Partner") {
//            netTax = this.NetPayable.filter(c => c.owner === filter1);
//            medicare = this.partnerMedicareLevy.filter(p => p.owner === filter2);
//        }


//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let val: number = 0;

//            val = netTax[0].values[this.selectedClient.startDate + i] + parseInt(medicare[0].values[this.selectedClient.startDate + i]);

//            if (val > 0) {
//                totalVal[this.selectedClient.startDate + i] = val;
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.TotalPayable.push(total);
//    }
//    public calculateAverageTaxRate(owner: string, filter1: string, filter2: string, type: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Average tax rate";

//        var totalTax: any = {};
//        var income: any = {};

//        if (type == "Client") {
//            totalTax = this.TotalPayable.filter(c => c.owner === filter1);
//            income = this.Income.filter(c => c.owner === filter2);

//        }
//        else if (type == "Partner") {
//            totalTax = this.TotalPayable.filter(c => c.owner === filter1);
//            income = this.Income.filter(p => p.owner === filter2);
//        }


//        for (var i = 0; i < this.selectedClient.period; i++) {
//            let val: number = 0;

//            val = totalTax[0].values[this.selectedClient.startDate + i] / parseInt(income[0].values[this.selectedClient.startDate + i]);

//            if (val > 0) {
//                totalVal[this.selectedClient.startDate + i] = val.toFixed(1);
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }

//        this.TotalPayable.push(total);
//    }

//    public calculateMarginalTaxRate(owner: string, filter: string, type: string) {

//        var total: any = {};
//        var totalVal: any = {};
//        total["owner"] = owner;
//        total["name"] = "Marginal tax rate";

//        var taxableIncome: any = {};
//        if (type == "Client") { taxableIncome = this.clientTaxableIncome.filter(c => c.owner === filter); }
//        else if (type == "Partner") { taxableIncome = this.partnerTaxableIncome.filter(c => c.owner === filter); }

//        let index: number = 0;
//        Math.max.apply(Math, this.marginalTaxRates.map(function (o: any) { if (o.index > index) index = o.index; }))
//        for (var i = 0; i < this.selectedClient.period; i++) {

//            let mtr: number = 0;
//            let val: number = taxableIncome[0].values[this.selectedClient.startDate + i];

//            for (var j = 1; j <= index; j++) {

//                let one = this.marginalTaxRates.filter(c => c.index === (j - 1));
//                let two = this.marginalTaxRates.filter(c => c.index === j);


//                if (typeof taxableIncome[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(taxableIncome[0].values[this.selectedClient.startDate + i])) {
//                    if (j == index) {
//                        if (val > one[0].threshold) {
//                            mtr = two[0].rate;
//                        }
//                    }
//                    else {
//                        if (val > one[0].threshold && val <= two[0].threshold) {
//                            mtr = two[0].rate;
//                            break;
//                        }
//                    }

//                }


//            }

//            if (mtr > 0) {
//                totalVal[this.selectedClient.startDate + i] = mtr;
//            } else {
//                totalVal[this.selectedClient.startDate + i] = 0;
//            }
//            total["values"] = totalVal;

//        }
//        this.TotalPayable.push(total);



//    }



//    public setYear() {
//        var range = [];
//        range.push(this.selectedClient.startDate);
//        for (var i = 1; i < this.selectedClient.period; i++) {
//            range.push(this.selectedClient.startDate + i);
//        }

//        this.years = range;
//    }

//    //TODO: Test all scenarios
//    public setClientAge() {

//        var date1 = new Date("7/01/ " + this.selectedClient.startDate);
//        var date2 = new Date(this.selectedClient.clientDob);
//        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
//        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;

//        this.clientAge = Math.round(diffDays * 10) / 10;

//    }
//    public setPartnerAge() {

//        var date1 = new Date("7/01/ " + this.selectedClient.startDate);
//        var date2 = new Date(this.selectedClient.partnerDob);
//        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
//        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;

//        this.partnerAge = Math.round(diffDays * 10) / 10;
//    }
//}
