//import { Component, OnInit } from '@angular/core';
//import { CashFlow } from './../../models/CashFlow';
//import { CashFlowService } from './../../services/cashFlow.service';
//import { Client } from '../../models/Client';
////import { FinancialAsset } from '../../models/FinancialAsset';
////import { FinancialAssetDetails } from '../../models/FinancialAsset';
//import { CommonService } from './../../services/common.service';
//import { FinancialAssetService } from './../../services/financialAsset.service';
//import { General } from '../../models/General';
//import { Validators, FormControl } from '@angular/forms';




//@Component({
//  selector: 'app-financial-asset',
//  templateUrl: './financial-asset.component.html',
//  styleUrls: ['./financial-asset.component.css']
//})
//export class FinancialAssetComponent implements OnInit {

//    assetAssumptions: any = [];
//    assetClass: any = [];
//    investmentProfile: any = [];
//    selectedClient: any;
//    financialAsset: any = [];
//    faDetails: any = {
//        fAssetId: 0,
//        clientId: 0,
//        name: '',
//        owner: 'Client',
//        value: 0,
//        costBase: 0,
//        startDateType: 'Start',
//        startDate: 0,
//        endDateType: 'End',
//        endDate: 0,
//        assetType: 'C',
//        assetName: '',
//        growth: 0,
//        income: 0,
//        franked: 0,
//        reinvest: 'Y',
//        reinvestStartDateType: 'Start',
//        reinvestStartDate: 0,
//        reinvestEndDateType: 'End',
//        reinvestEndDate: 0
  
//    };
//    years: any[];
//    currentYear = new Date().getFullYear();
//    currentMonth = new Date().getMonth();
//    owner: any = [];
//    booleanValues = [
//        { id: "Y", name: "Yes" },
//        { id: "N", name: "No" }

//    ];
//    start = [
//        { id: "Year", name: "Select year" },
//        { id: "Start", name: "Start" },
//        { id: "Existing", name: "Existing" }


//    ];
//    end = [
//        { id: "Year", name: "Select year" },
//        { id: "Retain", name: "Retain" },
//        { id: "End", name: "End" }
//    ];

//    from = [
//        { id: "Year", name: "Select year" },
//        { id: "Start", name: "Start" }
      


//    ];
//    to = [
//        { id: "Year", name: "Select year" },
//        { id: "End", name: "End" }
//    ];

//    indexation = [
//    ];

//    //TODO : Validations
//    //rateControl = new FormControl("", [Validators.max(100), Validators.min(5)])

//    constructor(private financialAssetService: FinancialAssetService, private commonService: CommonService) { }


//    ngOnInit() {

//        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
//        if (this.selectedClient.maritalStatus == "S") {
//            this.owner = ["Client"];
//        } else {
//            this.owner = ["Client", "Partner","Joint"];
//        }

//        if (this.selectedClient.clientRetirementYear != 0) {
//            var obj: any = {};
//            obj["id"] = "Client Retirement";
//            obj["name"] = "Client's Retirement";
//            this.start.push(obj);
//            this.end.push(obj);
//            this.from.push(obj);
//            this.to.push(obj);
//        }
//        if (this.selectedClient.partnerRetirementYear != 0) {
//            var obj: any = {};
//            obj["id"] = "Partner Retirement";
//            obj["name"] = "Partner's Retirement";
//            this.start.push(obj);
//            this.end.push(obj);
//            this.from.push(obj);
//            this.to.push(obj);
//        }

//        this.financialAssetService.getFinancialAssets(this.selectedClient.clientId).subscribe(
//            fa => {
//                console.log(fa);
                
//            }
//        );

//        this.commonService.getAssetTypesAssumptions().subscribe(
//            assetAssumptions => {
//                this.assetAssumptions = assetAssumptions;
//                this.assetClass = this.assetAssumptions.filter((a: any) => a.type == "C");
//                this.investmentProfile = this.assetAssumptions.filter((a: any) => a.type == "P");
//            }
//        );

//        this.setYear();

//    }

//    //private setAssetTypes() {

//    //    var indexRange: any = [];
//    //    //let Inflation: any = this.assetAssumptions
//    //    //    .filter((g: General) => g.type === "Inflation");
//    //    //let SaleryInflation: any = this.assetAssumptions
//    //    //    .filter((g: General) => g.type === "SalaryInflation");
//    //    //var cpi = { id: Inflation[0].percentage, name: "Inflation (CPI)" }
//    //    //var awote = { id: SaleryInflation[0].percentage, name: "Salary Inflation (AWOTE)" }
//    //    //indexRange.push(cpi);
//    //    //indexRange.push(awote);
//    //    //for (var i = 0.0; i <= 10.0; i += 0.5) {
//    //    //    var obj: any = {};
//    //    //    var obj: any = {};
//    //    //    obj["id"] = i;
//    //    //    obj["growth"] = i.toFixed(1) + '%';
//    //    //    obj["franked"] = 
//    //    //    indexRange.push(obj);
//    //    //}
//    //    //this.indexation = indexRange;
//    //}

//     setYear() {

//        if (this.currentMonth < 6) {
//            this.currentYear = this.currentYear - 1;

//        }
//        var range = [];
//        range.push(this.currentYear);
//        for (var i = 1; i < 26; i++) {
//            range.push(this.currentYear + i);
//        }

//        this.years = range;
//    }


//    assetTypeChange(value: any) {

//        this.faDetails.growth = this.assetAssumptions.filter((a: any) => a.name == value)[0].growth;
//        this.faDetails.income = this.assetAssumptions.filter((a: any) => a.name == value)[0].income;
//        this.faDetails.franked = this.assetAssumptions.filter((a: any) => a.name == value)[0].franking;
//    }
//    //fnClick(item: any, index: any) {
//    //    this.faDetails = JSON.parse(JSON.stringify(this.financialAsset[index]));
//    //    this.faDetails.index = index;
//    //}

//    //AddTempData(cfDetails: any) {

//    //    var index$ = cfDetails.index;
//    //    var exist = false;
//    //    if (this.cashFlowIncome.length > 0) {
//    //        this.cashFlowIncome.forEach((data: any, index: any) => {
//    //            if (index == index$) {
//    //                this.cashFlowIncome.splice(index, 1);
//    //                this.cashFlowIncome.splice(index, 0, cfDetails);
//    //                exist = true;

//    //            }

//    //        });
//    //    }
//    //    if (exist == false) {
//    //        this.cashFlowIncome.push(cfDetails);
//    //    }

//    //    this.cfDetails = {
//    //        cflowId: 0,
//    //        clientId: 0,
//    //        cftype: 'I',
//    //        cfname: '',
//    //        owner: 'Client',
//    //        type: 'Employment',
//    //        value: 0,
//    //        indexation: 0,
//    //        startDateType: 'Start',
//    //        startDate: 0,
//    //        endDateType: 'End',
//    //        endDate: 0
//    //    };
//    //}

//    //DeleteTempData(item: any, index: any) {

//    //    var index$ = index;
//    //    if (this.cashFlowIncome.length > 0) {
//    //        this.cashFlowIncome.forEach((data: any, index: any) => {
//    //            if (index == index$) {
//    //                this.cashFlowIncome.splice(index, 1);
//    //            }

//    //        });
//    //    }
//    //}


//    //Clear() {
//    //    this.cfDetails = {
//    //        cflowId: 0,
//    //        clientId: 0,
//    //        cftype: 'I',
//    //        cfname: '',
//    //        owner: 'Client',
//    //        type: 'Employment',
//    //        value: 0,
//    //        indexation: 0,
//    //        startDateType: 'Start',
//    //        startDate: 0,
//    //        endDateType: 'End',
//    //        endDate: 0
//    //    };
//    //}

//    //onSubmit() {

//    //    this.cashFlowIncome.forEach((x: CashFlow) => {

//    //        if (x.startDateType == "Start") {
//    //            x.startDate = this.selectedClient.startDate;
//    //        }
//    //        else if (x.startDateType == "Client Retirement") {
//    //            x.startDate = this.selectedClient.clientRetirementYear - 1;
//    //        }
//    //        else if (x.startDateType == "Partner Retirement") {
//    //            x.startDate = this.selectedClient.partnerRetirementYear - 1;
//    //        }

//    //        if (x.endDateType == "End") {
//    //            x.endDate = this.selectedClient.startDate + this.selectedClient.period;
//    //        }
//    //        else if (x.endDateType == "Client Retirement") {
//    //            x.endDate = this.selectedClient.clientRetirementYear - 1;
//    //        }
//    //        else if (x.endDateType == "Partner Retirement") {
//    //            x.endDate = this.selectedClient.partnerRetirementYear - 1;
//    //        }
//    //    });
//    //    this.cashFlowService.create(this.cashFlowIncome, this.selectedClient.clientId, "I").subscribe((data) => {
//    //    });
//    //}

//}
