import { Component, OnInit } from '@angular/core';
import { CashFlow } from './../../models/CashFlow';
import { CashFlowService } from './../../services/cashFlow.service';
import { Client } from '../../models/Client';
import { CommonService } from './../../services/common.service';
import { General } from '../../models/General';
import { Validators, FormControl } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import * as $ from 'jquery';

@Component({
  selector: 'app-cashflow-expenditure',
  templateUrl: './cashflow-expenditure.component.html',
  styleUrls: ['./cashflow-expenditure.component.css']
})
export class CashflowExpenditureComponent implements OnInit {
    display = 'none';
     generalAssumptions: any = [];
     selectedClient: any;
     clientDetails: any;
    cashFlowExpenditure: any = [];
    cfDetails: any = {
        cflowId: 0,
        clientId: 0,
        cftype: 'E',
        cfname: '',
        owner: 'Client',
        type: 'Post-tax',
        value: 0,
        indexation: 0,
        startDateType: 'Start',
        startDate: 0,
        endDateType: 'End',
        endDate: 0
    };
    cfUpdateDetails: any = {
        cflowId: 0,
        clientId: 0,
        cftype: 'E',
        cfname: '',
        owner: 'Client',
        type: 'Post-tax',
        value: 0,
        indexation: 0,
        startDateType: 'Start',
        startDate: 0,
        endDateType: 'End',
        endDate: 0
    };


    years: any[] = [];
    currentYear = new Date().getFullYear();
    currentMonth = new Date().getMonth();
    owner: any = [];
    type = ["Post-tax", "Pre-tax"];
    start = [
        { id: "Year", name: "Select year" },
        { id: "Start", name: "Start" },
   

    ];
    end = [
        { id: "Year", name: "Select year" },
        { id: "End", name: "End" }
    ];

    indexation = [
    ]

    constructor(private cashFlowService: CashFlowService, private commonService: CommonService) { }


    ngOnInit() {
        this.setYear();
      
        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
        this.clientDetails = JSON.parse(localStorage.getItem('ClientDetails') || '{}');

        if (this.clientDetails.maritalStatus == "S") {
            this.owner = ["Client"];
        } else {
            this.owner = ["Client", "Partner", "Joint"];
        }

        if (this.clientDetails.clientRetirementYear != 0) {
            var obj: any = {};
            obj["id"] = "Client Retirement";
            obj["name"] = "Client's Retirement";
            this.start.push(obj);
            this.end.push(obj);
        }
        if (this.clientDetails.partnerRetirementYear != 0) {
            var obj: any = {};
            obj["id"] = "Partner Retirement";
            obj["name"] = "Partner's Retirement";
            this.start.push(obj);
            this.end.push(obj);
        }

        this.cashFlowService.getCashFlows(this.selectedClient, "E").subscribe(
            cashFlowExpenditure => {
                this.cashFlowExpenditure = cashFlowExpenditure;

            }
        );


        this.commonService.getGeneralAssumptions().subscribe(
            generalAssumptions => {
                this.generalAssumptions = generalAssumptions;
                this.setIndexation();
            }
        );

      //$(document).ready(function () {
      //  //$("[data-toggle=popover]").each(function (i, obj) {

      //  //  (<any>$(this)).popover({
      //  //    html: true,
      //  //    content: function () {
      //  //      var id = $(this).attr('id')
      //  //      return $('#popover-content-' + id).html();
      //  //    }
      //  //  });

      //    //(<any>$(this)).popover({
      //    //    html: true,
      //    //    content: function () {
      //    //        var id = $(this).attr('id')
      //    //        return $('#popover-content-' + id).html();
      //    //    }
      //    //});

      //  });
      //});

     
    }

    private setIndexation() {

        var indexRange: any = [];
        let Inflation: any = this.generalAssumptions
            .filter((g: General) => g.type === "Inflation");
        let SalaryInflation: any = this.generalAssumptions
            .filter((g: General) => g.type === "SalaryInflation");
        var cpi = { id: Inflation[0].percentage, name: "Inflation (CPI)" }
        var awote = { id: SalaryInflation[0].percentage, name: "Salary Inflation (AWOTE)" }
        indexRange.push(cpi);
        indexRange.push(awote);
        for (var i = 0.0; i <= 10.0; i += 0.5) {
            var obj: any = {};
            obj["id"] = i;
            obj["name"] = i.toFixed(1) + '%';
            indexRange.push(obj);
        }
        this.indexation = indexRange;

        this.cfDetails.indexation = Inflation[0].percentage;
    }

    private setYear() {

        if (this.currentMonth < 6) {
            this.currentYear = this.currentYear - 1;

        }
        var range = [];
        range.push(this.currentYear);
        for (var i = 1; i < 26; i++) {
            range.push(this.currentYear + i);
        }

        this.years = range;
    }

    addFieldValue(cfDetails: any) {

        if (cfDetails.startDateType == "Start") {
            cfDetails.startDate = 0;
        }
        else if (cfDetails.startDateType == "Client Retirement") {
            cfDetails.startDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (cfDetails.startDateType == "Partner Retirement") {
            cfDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
        }

        if (cfDetails.endDateType == "End") {
            cfDetails.endDate = 0;
        }
        else if (cfDetails.endDateType == "Client Retirement") {
            cfDetails.endDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (cfDetails.endDateType == "Partner Retirement") {
            cfDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
        }

        this.cashFlowService.create(cfDetails, this.selectedClient, "I").subscribe((data: any) => {
            var exist = false;
            var id$ = data.cflowId;

            this.cashFlowExpenditure.forEach((data1: any, index: any) => {
                if (data1.cflowId == id$) {
                    this.cashFlowExpenditure.splice(index, 1);
                    this.cashFlowExpenditure.splice(index, 0, data);
                    exist = true;
                }

          
        });

            if (exist == false) {
                this.cashFlowExpenditure.push(data);
            }



        this.cfDetails = {
                cflowId: 0,
                clientId: 0,
                cftype: 'E',
                cfname: '',
                owner: 'Client',
                type: 'Post-tax',
                value: 0,
                indexation: 0,
                startDateType: 'Start',
                startDate: 0,
                endDateType: 'End',
                endDate: 0
        };

        this.setDefaultIndexation();
            this.onCloseHandled();
        });


        $('#cfExpenseName').attr('placeholder', this.cfDetails.type);
    }

    deleteCashFlow(item: any, index: any) {
        var id$ = item.cflowId;
        if (this.cashFlowExpenditure.length > 0) {
            this.cashFlowExpenditure.forEach((data: any, index: any) => {
                if (data.cflowId == id$) {
                    this.cashFlowService.delete(id$).subscribe((data) => {
                        this.cashFlowExpenditure.splice(index, 1);
                    });
                }
            });
        }
    }

    private setDefaultIndexation() {
        let Inflation: any = this.generalAssumptions
            .filter((g: General) => g.type === "Inflation");
        this.cfDetails.indexation = Inflation[0].percentage;
    }

    openModal(item: any, index: any) {
        this.cfUpdateDetails = JSON.parse(JSON.stringify(this.cashFlowExpenditure[index]));
        //  this.cfUpdateDetails.index = index;
        this.display = 'block';
    }

    onCloseHandled() {
        this.display = 'none';
        this.cfUpdateDetails = {
            cflowId: 0,
            clientId: 0,
            cftype: 'E',
            cfname: '',
            owner: 'Client',
            type: 'Post-tax',
            value: 0,
            indexation: 0,
            startDateType: 'Start',
            startDate: 0,
            endDateType: 'End',
            endDate: 0
        };
    }

    //fnClick(item: any, index: any) {
    //    this.cfDetails = JSON.parse(JSON.stringify(this.cashFlowExpenditure[index]));
    //    this.cfDetails.index = index;
    //}

    //AddTempData(cfDetails: any) {

    //    var index$ = cfDetails.index;
    //    var exist = false;
    //    if (this.cashFlowExpenditure.length > 0) {
    //        this.cashFlowExpenditure.forEach((data: any, index: any) => {
    //            if (index == index$) {
    //                this.cashFlowExpenditure.splice(index, 1);
    //                this.cashFlowExpenditure.splice(index, 0, cfDetails);
    //                exist = true;

    //            }

    //        });
    //    }
    //    if (exist == false) {
    //        this.cashFlowExpenditure.push(cfDetails);
    //    }

    //    this.cfDetails = {
    //        cflowId: 0,
    //        clientId: 0,
    //        cftype: 'E',
    //        cfname: '',
    //        owner: 'Client',
    //        type: 'Post-tax',
    //        value: 0,
    //        indexation: 0,
    //        startDateType: 'Start',
    //        startDate: 0,
    //        endDateType: 'End',
    //        endDate: 0
    //    };
    //}

    //DeleteTempData(item: any, index: any) {

    //    var index$ = index;
    //    if (this.cashFlowExpenditure.length > 0) {
    //        this.cashFlowExpenditure.forEach((data: any, index: any) => {
    //            if (index == index$) {
    //                this.cashFlowExpenditure.splice(index, 1);
    //            }

    //        });
    //    }
    //}


    //Clear() {
    //    this.cfDetails = {
    //        cflowId: 0,
    //        clientId: 0,
    //        cftype: 'E',
    //        cfname: '',
    //        owner: 'Client',
    //        type: 'Post-tax',
    //        value: 0,
    //        indexation: 0,
    //        startDateType: 'Start',
    //        startDate: 0,
    //        endDateType: 'End',
    //        endDate: 0
    //    };
    //}

    //onSubmit() {
    //    this.cashFlowExpenditure.forEach((x: CashFlow) => {

    //        if (x.startDateType == "Start") {
    //            x.startDate = this.selectedClient.startDate;
    //        }
    //        else if (x.startDateType == "Client Retirement") {
    //            x.startDate = this.selectedClient.clientRetirementYear - 1;
    //        }
    //        else if (x.startDateType == "Partner Retirement") {
    //            x.startDate = this.selectedClient.partnerRetirementYear - 1;
    //        }

    //        if (x.endDateType == "End") {
    //            x.endDate = this.selectedClient.startDate + this.selectedClient.period;
    //        }
    //        else if (x.endDateType == "Client Retirement") {
    //            x.endDate = this.selectedClient.clientRetirementYear - 1;
    //        }
    //        else if (x.endDateType == "Partner Retirement") {
    //            x.endDate = this.selectedClient.partnerRetirementYear - 1;
    //        }
    //    });

    //    this.cashFlowService.create(this.cashFlowExpenditure, this.selectedClient.clientId, "E").subscribe((data) => {
    //    });
    //}
}
