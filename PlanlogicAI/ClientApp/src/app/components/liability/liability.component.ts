import { Component, OnInit } from '@angular/core';
import { General } from '../../models/General';
import { Liability, LiabilityDetails } from '../../models/Liability';
import { CommonService } from './../../services/common.service';
import { LiabilityService } from './../../services/liability.service';
import { PropertyService } from './../../services/property.service';
import { InvestmentService } from './../../services/investment.service';
import { Validators, FormControl } from '@angular/forms';
import * as $ from 'jquery';

@Component({
  selector: 'app-liability',
  templateUrl: './liability.component.html',
  styleUrls: ['./liability.component.css']
})
export class LiabilityComponent implements OnInit {
    associatedAssets: any = [];
    display = 'none';
    employment: any = [];
    notEmployed = true;
    sgIncreaseToLimit = false;
    interestRate: any;
    minRepayment: number = 0;
    minRepaymentUpdate: number = 0;
    liability = 'none';
    selectedClient: any;
    clientDetails: any;

    //Investments
    liabilities: any = [];
    liabilityDetails: any = {
    liabilityId: 0,
    clientId: 0,
    type: 'Non-Deductible',
    name: '',
    deductibility : 0,
    owner: 'Client',
    principal: 0,
    repaymentType : 'PI',
    repayment : 0, //Calculate
    interestRate: 0,
    term: 30,
    commenceOnDateType: 'Existing',
    commenceOnDate: 0,
    repaymentDateType: 'Retain',
    repaymentDate: 0,
    associatedAsset: '',
    creditLimit: 0
    };

    liabilityUpdateDetails: any = {
        liabilityId: 0,
        clientId: 0,
        type: 'Non-Deductible',
        name: '',
        deductibility: 0,
        owner: 'Client',
        principal: 0,
        repaymentType: 'PI',
        repayment: 0, //Calculate
        interestRate: 0,//Calculate
        term: 30,
        commenceOnDateType: 'Existing',
        commenceOnDate: 0,
        repaymentDateType: 'Retain',
        repaymentDate: 0,
        associatedAsset: '',
        creditLimit: 0
    };



    liabilityDrawdowns: any = [];
    drawdown: any = {
        liabilityId: 0,
        clientId: 0,
        amount: 0,
        fromDateType: 'Start',
        fromDate: 0,
        toDateType: 'End',
        toDate: 0
    };

  

    repayment = [
        { id: "PI", name: "Principal & Interest" },
        { id: "IO", name: "Interest only" }

    ];

    type = ["Non-Deductible", "Deductible"];

    owner: any = [];

    years: any[] = [];
    currentYear = new Date().getFullYear();
    currentMonth = new Date().getMonth();

    start = [
        { id: "Year", name: "Select year" },
        { id: "Existing", name: "Existing" },
        { id: "Start", name: "Start" },


    ];
    end = [
        { id: "Year", name: "Select year" },
        { id: "Retain", name: "Retain" },
        { id: "End", name: "End" }
    ];


    from = [
        { id: "Year", name: "Select year" },
        { id: "Start", name: "Start" },
    ];
    to = [
        { id: "Year", name: "Select year" },
        { id: "End", name: "End" }
    ];

    investmentProfiles: any = [];

    constructor(private liabilityService: LiabilityService, private commonService: CommonService, private propertyService: PropertyService, private investmentService: InvestmentService) { }

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
            this.from.push(obj);
            this.to.push(obj);
        }
        if (this.clientDetails.partnerRetirementYear != 0) {
            var obj: any = {};
            obj["id"] = "Partner Retirement";
            obj["name"] = "Partner's Retirement";
            this.start.push(obj);
            this.end.push(obj);
            this.from.push(obj);
            this.to.push(obj);
        }

        this.liabilityService.getLiabilities(this.selectedClient).subscribe(
            liabilities => {
                this.liabilities = liabilities;

            }
        );


        this.commonService.getGeneralAssumptions().subscribe((generalAssumptions: any) => {
                var assetAssumptions = generalAssumptions.filter((a: any) => a.type == "RBA");
                var rate = assetAssumptions[0].percentage;
                this.interestRate = [
                    { id: Number(rate), name: "RBA Rate" },
                    { id: Number(rate) + 1, name: "RBA Rate + 1%" },
                    { id: Number(rate) + 2, name: "RBA Rate + 2%" },
                    { id: Number(rate) + 3, name: "RBA Rate + 3%" },
                    { id: Number(rate) + 4, name: "RBA Rate + 4%" },
                    { id: Number(rate) + 5, name: "RBA Rate + 5%" }
                ];

                this.liabilityDetails.interestRate = Number(rate) + 2;
                this.setRepaymentValue(this.liabilityDetails.repaymentType, this.liabilityDetails.principal, this.liabilityDetails.interestRate, this.liabilityDetails.term, 0);



            }
        );

        this.getAssociatedAssets();
      
        //$("[data-toggle=popover]").each(function (i, obj) {

        //    (<any>$(this)).popover({
        //        html: true,
        //        content: function () {
        //            var id = $(this).attr('id')
        //            if (id == 'Property') {
        //                return $('#popover-content-property').html();
        //            }

        //              //  return $('#popover-content-investment').html();

        //        }
        //    });

        //});

    }
     setYear() {

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

     getAssociatedAssets() {
         this.propertyService.getProperties(this.selectedClient).subscribe((properties: any) => {
                    var properties = properties;
                    for (var i = 0; i < properties.length; i++) {
                        var obj: any = {};
                        obj["id"] = "Property-" + properties[i].propertyId;
                        obj["name"] = properties[i].name
                        this.associatedAssets.push(obj);
                    }
                }
        );
         this.investmentService.getInvestments(this.selectedClient).subscribe((investments: any) => {
                    var investments = investments;
                    for (var i = 0; i < investments.length; i++) {
                        var obj: any = {};
                        obj["id"] = "Investment-" + investments[i].investmentId;
                        obj["name"] = investments[i].name
                        this.associatedAssets.push(obj);
                    }
                }
            );
    }

     setRepaymentValue(repaymentType: any,principal:any,interestRate:any, term:any, type: number) {
        if (type == 0) {
            if (repaymentType == undefined || repaymentType == "") {
                repaymentType = this.liabilityDetails.repaymentType;
            }
            if (principal == undefined || principal == "") {
                principal = this.liabilityDetails.principal;
            }
            if (interestRate == undefined || interestRate == "") {
                interestRate = this.liabilityDetails.interestRate;
            }
            if (term == undefined) {
                term = this.liabilityDetails.term;
            }

            if (repaymentType == "PI") {
                this.liabilityDetails.repayment = (Number(principal) * (interestRate / 100) * (Math.pow((1 + (interestRate / 100)), term) / (Math.pow((1 + (interestRate / 100)), term) - 1))).toFixed();
                this.minRepayment = this.liabilityDetails.repayment;
            }
            else {
                this.liabilityDetails.repayment = (Number(principal) * (interestRate / 100)).toFixed();
                this.minRepayment = this.liabilityDetails.repayment;
            }
          
        }
        else {
            if (repaymentType == undefined || repaymentType == "") {
                repaymentType = this.liabilityUpdateDetails.repaymentType;
            }
            if (principal == undefined || principal == "") {
                principal = this.liabilityUpdateDetails.principal;
            }
            if (interestRate == undefined || interestRate == "") {
                interestRate = this.liabilityUpdateDetails.interestRate;
            }
            if (term == undefined || term == "") {
                term = this.liabilityUpdateDetails.term;
            }
            if (repaymentType == "PI") {
                this.liabilityUpdateDetails.repayment = (Number(principal) * (interestRate / 100) * (Math.pow((1 + (interestRate / 100)), term) / (Math.pow((1 + (interestRate / 100)), term) - 1))).toFixed();
                this.minRepaymentUpdate = this.liabilityUpdateDetails.repayment;
            }
            else {
                this.liabilityUpdateDetails.repayment = (Number(principal) * (interestRate / 100)).toFixed();
                this.minRepaymentUpdate = this.liabilityUpdateDetails.repayment;
            }
        }
    
    }

     checkMinRepayment(repayment: any, type: number) {
        if (type == 0) {
            if (repayment < this.minRepayment) {
                alert("Specified repayment amount should be higher than the minimum - " + this.minRepayment);
            }
        }
        else {
            if (repayment < this.minRepaymentUpdate) {
                alert("Specified repayment amount should be higher than the minimum - " + this.minRepaymentUpdate);
            }

        }
    }


    changeType(type: any, update: any) {
        if (type == 'Non-Deductible') {
            if (update == '0') {

                this.liabilityDetails.deductibility = 0;

            } else {
                this.liabilityUpdateDetails.deductibility = 0;
                }
           
        }
        else if (type == 'Deductible') {
        if (update == '0') {

            this.liabilityDetails.deductibility = 100;

        } else {
            this.liabilityUpdateDetails.deductibility = 100;
        }
    }

        }
       
    openModal(item: any, index: any) {
        this.liabilityUpdateDetails = JSON.parse(JSON.stringify(this.liabilities[index]));
        this.liabilityService.getLiabilityDetails(this.selectedClient, this.liabilityUpdateDetails.liabilityId).subscribe(
            liabilityDrawdown => {
                this.liabilityDrawdowns = liabilityDrawdown;

                }
        );

        this.display = 'block';
    }

    onCloseHandled() {
        this.display = 'none';
        this.liabilityUpdateDetails = {
            liabilityId: 0,
            clientId: 0,
            type: 'Non-Deductible',
            name: '',
            deductibility: 0,
            owner: 'Client',
            principal: 0,
            repaymentType: 'PI',
            repayment: 0, //Calculate
            interestRate: 0,//Calculate
            term: 30,
            commenceOnDateType: 'Existing',
            commenceOnDate: 0,
            repaymentDateType: 'Retain',
            repaymentDate: 0,
            associatedAsset: '',
            creditLimit: 0
        };
    }
   
    //Liability
    addFieldValue(liabilityDetails: any) {
        if (this.liabilityDetails.commenceOnDateType == "Start") {
            this.liabilityDetails.commenceOnDate = 0;
        }
        else if (this.liabilityDetails.commenceOnDateType == "Existing") {
            this.liabilityDetails.commenceOnDate = 0;
        }
        else if (this.liabilityDetails.commenceOnDateType == "Client Retirement") {
            this.liabilityDetails.commenceOnDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.liabilityDetails.commenceOnDateType == "Partner Retirement") {
            this.liabilityDetails.commenceOnDate = this.clientDetails.partnerRetirementYear - 1;
        }

        if (this.liabilityDetails.repaymentDateType == "End") {
            this.liabilityDetails.repaymentDate = 0;
        }
        else if (this.liabilityDetails.repaymentDateType == "Retain") {
            this.liabilityDetails.repaymentDate = 0;
        }
        else if (this.liabilityDetails.repaymentDateType == "Client Retirement") {
            this.liabilityDetails.repaymentDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.liabilityDetails.repaymentDateType == "Partner Retirement") {
            this.liabilityDetails.repaymentDate = this.clientDetails.partnerRetirementYear - 1;
        }

        this.liabilityDetails.repayment = Number(this.liabilityDetails.repayment);

        this.liabilityService.create(this.liabilityDetails, [], this.selectedClient).subscribe((data: any) => {
            var exist = false;
            var id$ = data.liabilityId;

            this.liabilities.forEach((data1: any, index: any) => {
                if (data1.liabilityId == id$) {
                    this.liabilities.splice(index, 1);
                    this.liabilities.splice(index, 0, data);
                    exist = true;
                }
            });

            if (exist == false) {
                this.liabilities.push(data);
            }

            this.liabilityDetails = {
                liabilityId: 0,
                clientId: 0,
                type: 'Non-Deductible',
                name: '',
                deductibility: 0,
                owner: 'Client',
                principal: 0,
                repaymentType: 'PI',
                repayment: 0, //Calculate
                interestRate: 0,//Calculate
                term: 30,
                commenceOnDateType: 'Existing',
                commenceOnDate: 0,
                repaymentDateType: 'Retain',
                repaymentDate: 0,
                associatedAsset: '',
                creditLimit: 0
            };



        });

        this.onCloseHandled();

        $('#liabilityName').attr('placeholder', this.liabilityDetails.type);
    }
    deleteLiability(item: any, index: any) {
        var id$ = item.liabilityId;
        if (this.liabilities.length > 0) {
            this.liabilities.forEach((data: any, index: any) => {
                if (data.liabilityId == id$) {
                    this.liabilityService.delete(id$).subscribe((data) => {
                        this.liabilities.splice(index, 1);
                    });
                }
            });
        }
    }

    updateLiability(liabilityDetails: any) {

        if (liabilityDetails.commenceOnDateType == "Start") {
            this.liabilityUpdateDetails.commenceOnDate = 0;
        }
        else if (liabilityDetails.commenceOnDateType == "Existing") {
            this.liabilityUpdateDetails.commenceOnDate = 0;
        }
        else if (liabilityDetails.commenceOnDateType == "Client Retirement") {
            this.liabilityUpdateDetails.commenceOnDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (liabilityDetails.commenceOnDateType == "Partner Retirement") {
            this.liabilityUpdateDetails.commenceOnDate = this.clientDetails.partnerRetirementYear - 1;
        }

        if (liabilityDetails.repaymentDateType == "End") {
            this.liabilityUpdateDetails.repaymentDate = 0;
        }
        else if (liabilityDetails.repaymentDateType == "Retain") {
            this.liabilityUpdateDetails.repaymentDate = 0;
        }
        else if (liabilityDetails.repaymentDateType == "Client Retirement") {
            this.liabilityUpdateDetails.repaymentDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (liabilityDetails.repaymentDateType == "Partner Retirement") {
            this.liabilityUpdateDetails.repaymentDate = this.clientDetails.partnerRetirementYear - 1;
        }
        this.liabilityUpdateDetails.repayment = Number(this.liabilityUpdateDetails.repayment);

        this.liabilityService.create(this.liabilityUpdateDetails, this.liabilityDrawdowns, this.selectedClient).subscribe((data: any) => {
            var exist = false;
            var id$ = data.liabilityId;

            this.liabilities.forEach((data1: any, index: any) => {
                if (data1.liabilityId == id$) {
                    this.liabilities.splice(index, 1);
                    this.liabilities.splice(index, 0, data);
                    exist = true;
                }
            });

        });


        this.onCloseHandled();

    }
   

    //LiabilityDrawdown
    AddLiabilityDD(liabilityDD: any) {

        var index$ = liabilityDD.index;
        var exist = false;
        if (this.liabilities.length > 0) {
            this.liabilities.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.liabilities.splice(index, 1);
                    this.liabilities.splice(index, 0, liabilityDD);
                    exist = true;

                }

            });
        }
        if (exist == false) {
            if (liabilityDD.fromDateType == "Start") {
                liabilityDD.fromDate = 0;
            }
            else if (liabilityDD.fromDateType == "Client Retirement") {
                liabilityDD.fromDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (liabilityDD.fromDateType == "Partner Retirement") {
                liabilityDD.fromDate = this.clientDetails.partnerRetirementYear - 1;
            }

            if (liabilityDD.toDateType == "End") {
                liabilityDD.toDate = 0;
            }
            else if (liabilityDD.toDateType == "Client Retirement") {
                liabilityDD.toDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (liabilityDD.toDateType == "Partner Retirement") {
                liabilityDD.toDate = this.clientDetails.partnerRetirementYear - 1;
            }
            this.liabilityDrawdowns.push(liabilityDD);
        }

        this.drawdown = {
            liabilityId: 0,
            clientId: 0,
            amount: 0,
            fromDateType: 'Start',
            fromDate: 0,
            toDateType: 'End',
            toDate: 0
        };

    }
    DeleteLiabilityDD(item: any, index: any) {

        var index$ = index;
        if (this.liabilityDrawdowns.length > 0) {
            this.liabilityDrawdowns.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.liabilityDrawdowns.splice(index, 1);
                }

            });
        }
    }








}
