import { Component, OnInit } from '@angular/core';
import { General } from '../../models/General';
import { LifestyleAssetService } from './../../services/lifestyleAsset.service';
import { CashFlowService } from './../../services/cashFlow.service';
import { PropertyService } from './../../services/property.service';
import { InvestmentService } from './../../services/investment.service';
import { SuperService } from './../../services/super.service';
import { PensionService } from './../../services/pension.service';
import { LifestyleAsset } from '../../models/LifestyleAsset';
import { Property } from '../../models/Property';
import { Investment, InvestmentDetails } from '../../models/Investment';
import { Super, SuperDetails } from '../../models/Super';
import { CommonService } from './../../services/common.service';
import { Validators, FormControl } from '@angular/forms';
import * as $ from 'jquery';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';

@Component({
  selector: 'app-assets',
  templateUrl: './assets.component.html',
  styleUrls: ['./assets.component.css']
})
export class AssetsComponent implements OnInit {
    employment: any = [];
    notEmployed = true;
    sgIncreaseToLimit = false;
    sgrate = [
    ]

    display = 'none';
    lifestyle = 'none';
    property = 'none';
    investment = 'none';
    super = 'none';
    pension = 'none';
    propertyValue = 'Domestic Property';
    assetAssumptions: any = [];
    pensionDrawDownAssumptions: any = [];
    selectedClient: any;
    clientDetails: any;
    //Lifestyle
    lifestyleAssets: any = [];
    lAssetDetails: any = {
        lAssetId: 0,
        clientId: 0,
        lAssetType: 'PrimaryResidence',
        name: '',
        owner: 'Client',
        value: 0,
        growth: 0.0,
        costBase: 0,
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0   
    };
    lAssetUpdateDetails: any = {
        lAssetId: 0,
        clientId: 0,
        lAssetType: 'PrimaryResidence',
        name: '',
        owner: 'Client',
        value: 0,
        growth: 0.0,
        costBase: 0,
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0 
    };
    //Property
    properties: any = [];
    propertyDetails: any = {
        propertyId: 0,
        clientId: 0,
        name: '',
        owner: 'Client',
        value: 0,
        rent: 0,
        expenses: 0,
        growth: 0.0,
        costBase: 0,
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0
    };
    propertyUpdateDetails: any = {
        propertyId: 0,
        clientId: 0,
        name: '',
        owner: 'Client',
        value: 0,
        rent: 0,
        expenses: 0,
        growth: 0.0,
        costBase: 0,
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0
    };

    //Investments
    investments: any = [];
    investmentDetails: any = {
        investmentId: 0,
        clientId: 0,
        type:'',
        name: '',
        owner: 'Client',
        value: 0,
        growth: 0.00,
        income: 0.00,
        franked: 0.00,
        productFees: 0.0000,
        costBase: 0,
        reinvest: 'Y',
        centrelink: 'N',
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0
    };
    investmentUpdateDetails: any = {
        investmentId: 0,
        clientId: 0,
        type: '',
        name: '',
        owner: 'Client',
        value: 0,
        growth: 0.00,
        income: 0.00,
        franked: 0.00,
        productFees: 0.0000,
        costBase: 0,
        reinvest: 'Y',
        centrelink: 'N',
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0
    };

    //Super
    supers: any = [];
    superDetails: any = {
        superId: 0,
        clientId: 0,
        type: '',
        name: '',
        owner: 'Client',
        value: 0,
        taxFreeComponent: 0,
        taxableComponent: 0,
        growth: 0.00,
        income: 0.00,
        franked: 0.00,
        productFees: 0.0000,
        insuranceCost :0,
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0,
        superSalary: 0,
        increaseToLimit: 'N',
        sgrate: ''

    };
    superUpdateDetails: any = {
        superId: 0,
        clientId: 0,
        type: '',
        name: '',
        owner: 'Client',
        value: 0,
        taxFreeComponent: 0,
        taxableComponent: 0,
        growth: 0.00,
        income: 0.00,
        franked: 0.00,
        productFees: 0.0000,
        insuranceCost: 0,
        startDateType: 'Existing',
        startDate: 0,
        endDateType: 'Retain',
        endDate: 0,
        superSalary: 0,
        increaseToLimit: 'N',
        sgrate: ''
    };


    //Super
    pensions: any = [];
    pensionDetails: any = {
        pensionId: 0,
        clientId: 0,
        type: '',
        pensionType: 'ABP',
        name: '',
        owner: 'Client',
        value: 0,
        taxFreeComponent: 0,
        taxableComponent: 0,
        growth: 0.00,
        income: 0.00,
        franked: 0.00,
        productFees: 0.0000,
        totalBalance: 0,
        pensionRebootFromType: 'Existing',
        pensionRebootFromDate: 0,
        endDateType: 'End',
        endDate: 0
      
    };
    pensionUpdateDetails: any = {
        pensionId: 0,
        clientId: 0,
        type: '',
        pensionType: 'ABP',
        name: '',
        owner: 'Client',
        value: 0,
        taxFreeComponent: 0,
        taxableComponent: 0,
        growth: 0.00,
        income: 0.00,
        franked: 0.00,
        productFees: 0.0000,
        totalBalance: 0,
        pensionRebootFromType: 'Existing',
        pensionRebootFromDate: 0,
        endDateType: 'End',
        endDate: 0
    };


    pensionDD: any = [];
    pensionDDDetails: any = {
        pensionId: 0,
        clientId: 0,
        type: 'Minimum',
        amount: 0,
        fromDateType: 'Start',
        fromDate: 0,
        toDateType: 'End',
        toDate: 0
    };



    cw: any = [];
    cwDetails: any = {
        investmentId: 0,
        clientId: 0,
        type: 'C',
        value: 0,
        fromDateType: 'Start',
        fromDate: 0,
        toDateType: 'End',
        toDate: 0
    };

    superCW: any = [];
    superCWDetails: any = {
        superId: 0,
        clientId: 0,
        type: '',
        amount: 0,
        increaseToLimit: 'N',
        fromDateType: 'Start',
        fromDate: 0,
        toDateType: 'End',
        toDate: 0
    };

    investmentType = [
        { id: "C", name: "Contribution" },
        { id: "W", name: "Withdrawal" }

    ];
   
    booleanValues = [
        { id: "Y", name: "Yes" },
        { id: "N", name: "No" }

    ];
    owner: any = [];
    SPowner: any = [];
    sgCWType: any = [];
    assetDetails: any = {
        assetName: '',
        owner: 'Client',
        type: 'Lifestyle',
        value: 0,
        investmentProfile:'',

    };

    type = [
        { id: "Lifestyle", name: "Lifestyle" },
        { id: "DirectInvestment", name: "Direct Investment" },
        { id: "Super", name: "Super" },
        { id: "Pension", name: "Pension" },

    ];

    lifestyleType = [
    { id: "PrimaryResidence", name: "Primary Residence" },
    { id: "HomeContents", name: "Home Contents" },
    { id: "MotorVehicles", name: "Motor Vehicles" }
    ];

    directInvestment = [
    { id: "Domestic Cash", name: "Cash" },
    { id: "Domestic Fixed Interest", name: "Fixed Interest" },
    { id: "Domestic Equity", name: "Australian Equity" },
    { id: "International Equity", name: "International Equity" },
    { id: "Domestic Property", name: "Property" },
    { id: "Growth Alternatives", name: "Growth Alternative" },
    { id: "Defensive Alternative", name: "Defensive Alternative" },
    { id: "Preservation", name: "Profile:Preservation" },
    { id: "Defensive", name: "Profile:Defensive" },
    { id: "Moderate", name: "Profile:Moderate" },
    { id: "Balanced", name: "Profile:Balanced" },
    { id: "Growth", name: "Profile:Growth" },
    { id: "High Growth", name: "Profile:High Growth" }

];

    superPension = [
        { id: "Preservation", name: "Preservation" },
        { id: "Defensive", name: "Defensive" },
        { id: "Moderate", name: "Moderate" },
        { id: "Balanced", name: "Balanced" },
        { id: "Growth", name: "Growth" },
        { id: "High Growth", name: "High Growth" }

    ];

    years: any[] | undefined;
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


    pensionStart = [
        { id: "Year", name: "Select year" },
        { id: "Existing", name: "Existing" },
        //{ id: "NoReboot", name: "No Reboot" },
        { id: "Start", name: "Start" }


    ];
    pensionEnd = [
        { id: "Year", name: "Select year" },
        { id: "Retain", name: "Retain" },
        { id: "End", name: "End" }
    ];

    pensionType = [
        { id: "ABP", name: "Account Based Pension" },
        { id: "TTR", name: "Transition to Retirement Pension" }
    ];

    pensionDDType = ["Minimum", "Maximum", "Custom"];
    //removed mixed expenses 

    from = [
        { id: "Year", name: "Select year" },
        { id: "Start", name: "Start" },
    ];
    to = [
        { id: "Year", name: "Select year" },
        { id: "End", name: "End" }
    ];

    pensionDDfrom = [
        { id: "Year", name: "Select year" },
        { id: "Start", name: "Start" }
        //{ id: "EndTTR", name: "End of TTR Phase" }
    ];

    pensionDDto = [
        { id: "Year", name: "Select year" },
        { id: "End", name: "End" }
        //{ id: "EndTTR", name: "End of TTR Phase" }
    ];
    investmentProfiles: any = [];

    constructor(private lifestyleAssetService: LifestyleAssetService, private cashFlowService: CashFlowService, private commonService: CommonService, private propertyService: PropertyService, private investmentService: InvestmentService, private superService: SuperService, private pensionService: PensionService) { }

    ngOnInit() {
        this.setYear();
        var sources = [];
        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
        this.clientDetails = JSON.parse(localStorage.getItem('ClientDetails') || '{}');


        if (this.clientDetails.maritalStatus == "S") {
            this.owner = ["Client"];
            this.SPowner = ["Client"];
            this.sgCWType = [
                { id: "SS", name: "Salary Sacrifice/Personal Deductible Contributions" },
                { id: "PNC", name: "Personal Non-Concessional Contributions" },
                { id: "LumpSum", name: "Lump Sum Withdrawals" }
            ];

            
        } else {
            this.owner = ["Client", "Partner", "Joint"];
            this.SPowner = ["Client", "Partner"];
            this.sgCWType = [
                { id: "SS", name: "Salary Sacrifice/Personal Deductible Contributions" },
                { id: "PNC", name: "Personal Non-Concessional Contributions" },
                { id: "Spouse", name: "Spouse Contributions" },
                { id: "LumpSum", name: "Lump Sum Withdrawals" }
            ];
        }

        if (this.clientDetails.clientRetirementYear != 0) {
            var obj: any = {};
            obj["id"] = "Client Retirement";
            obj["name"] = "Client's Retirement";
            this.start.push(obj);
            this.end.push(obj);
            this.from.push(obj);
            this.to.push(obj);
            this.pensionStart.push(obj);
            this.pensionEnd.push(obj);
            this.pensionDDto.push(obj);
            this.pensionDDfrom.push(obj);
        }
        if (this.clientDetails.partnerRetirementYear != 0) {
            var obj: any = {};
            obj["id"] = "Partner Retirement";
            obj["name"] = "Partner's Retirement";
            this.start.push(obj);
            this.end.push(obj);
            this.from.push(obj);
            this.to.push(obj);
            this.pensionStart.push(obj);
            this.pensionEnd.push(obj);
            this.pensionDDto.push(obj);
            this.pensionDDfrom.push(obj);
        }

        this.lifestyleAssetService.getLifestyleAssets(this.selectedClient).subscribe(
            lifestyleAsset => {
                this.lifestyleAssets = lifestyleAsset;

            }
        );

        this.propertyService.getProperties(this.selectedClient).subscribe(
            properties => {
                this.properties = properties;

            }
        );

        this.investmentService.getInvestments(this.selectedClient).subscribe(
            investments => {
                this.investments = investments;

            }
        );

        

        this.pensionService.getPensions(this.selectedClient).subscribe(
            pensions => {
                this.pensions = pensions;

            }
        );

        this.superService.getSupers(this.selectedClient).subscribe(
            supers => {
                this.supers = supers;

            }
        );

        this.commonService.getAssetTypesAssumptions().subscribe(
            assetAssumptions => {
                this.assetAssumptions = assetAssumptions;
              
            }
        );

        this.commonService.getPensionDrawdownAssumptions().subscribe(
            pensionDDAssumptions => {
                this.pensionDrawDownAssumptions = pensionDDAssumptions;
                this.pensionDrawDownAssumptions = this.pensionDrawDownAssumptions.sort(function (obj1: any, obj2: any) {
                    return obj1.index - obj2.index;
                });
            }
        );

        this.changeAssetType(this.assetDetails.type);
        this.checkSGContributions();
        this.setSGRate();
  

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

    private setSGRate() {

        var indexRange: any = [];
       
        var sgc = { id: 'SGC', name: "SGC" }
        indexRange.push(sgc);
        for (var i = 0.0; i <= 10.0; i += 0.5) {
            var obj: any = {};
            obj["id"] = i.toString();
            obj["name"] = i.toFixed(1).toString() + '%';
            indexRange.push(obj);
        }
        this.sgrate = indexRange;
    }

    changeAssetType(assetType: any) {
        if (assetType == "Lifestyle") {

            this.investmentProfiles = this.lifestyleType;
            this.assetDetails.investmentProfile = "PrimaryResidence";
            
            
        }
        else if (assetType == "DirectInvestment") {
            this.investmentProfiles = this.directInvestment;
            this.assetDetails.investmentProfile = "Domestic Cash";
            
        }
        else {
            this.investmentProfiles = this.superPension;
            this.assetDetails.investmentProfile = "Preservation";
        }


    }
    changeType(investmentProfile: any, update: any, type: any) {
        if (type == 'Lifestyle') {
            if (update == '0') {
                if (investmentProfile == "PrimaryResidence") {
                    this.lAssetDetails.growth = this.assetAssumptions.filter((a: any) => a.name == "Domestic Property")[0].growth;
                }
                else {
                    this.lAssetDetails.growth = 0.0;
                }
            }
            else {
                if (investmentProfile == "PrimaryResidence") {
                    this.lAssetUpdateDetails.growth = this.assetAssumptions.filter((a: any) => a.name == "Domestic Property")[0].growth;
                }
                else {
                    this.lAssetUpdateDetails.growth = 0.0;
                }
            }
        }
        else if (type == 'DirectInvestment') {
            if (update == '0') {
                if (investmentProfile == "Domestic Property") {
                    this.propertyDetails.growth = this.assetAssumptions.filter((a: any) => a.name == "Domestic Property")[0].growth;
                }
                else {
                    this.investmentDetails.growth = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].growth;
                    this.investmentDetails.income = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].income;
                    this.investmentDetails.franked = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].franking;
                    this.investmentDetails.productFees = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].productFees;
                }
            }
            else {
                if (investmentProfile == "Domestic Property") {
                    this.propertyUpdateDetails.growth = this.assetAssumptions.filter((a: any) => a.name == "Domestic Property")[0].growth;
                }
                else {
                    //TODO: set investmentProfile key to same name
                    this.investmentUpdateDetails.growth = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].growth;
                    this.investmentUpdateDetails.income = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].income;
                    this.investmentUpdateDetails.franked = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].franking;
                    this.investmentUpdateDetails.productFees = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].productFees;
                }
            }
        }
        else if (type == 'Super') {
            if (update == '0') {

                this.superDetails.growth = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].growth;
                this.superDetails.income = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].income;
                this.superDetails.franked = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].franking;
                this.superDetails.productFees = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].productFees;

            
            }
            else {
                this.superUpdateDetails.growth = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].growth;
                this.superUpdateDetails.income = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].income;
                this.superUpdateDetails.franked = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].franking;
                this.superUpdateDetails.productFees = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].productFees;

            }
        }
        else if (type == 'Pension') {
            if (update == '0') {

                this.pensionDetails.growth = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].growth;
                this.pensionDetails.income = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].income;
                this.pensionDetails.franked = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].franking;
                this.pensionDetails.productFees = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].productFees;


            }
            else {
                this.pensionUpdateDetails.growth = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].growth;
                this.pensionUpdateDetails.income = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].income;
                this.pensionUpdateDetails.franked = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].franking;
                this.pensionUpdateDetails.productFees = this.assetAssumptions.filter((a: any) => a.name == investmentProfile)[0].productFees;

            }
        }
    }
    changeSGType(sgContribution: any, update: any) {

        //TODO : Reconfirm  and change value of every SS if SG changes to yes

        if (sgContribution == "LumpSum") {
            this.sgIncreaseToLimit = true;
            this.superCWDetails.increaseToLimit = 'N';
        }
        else if (sgContribution == "SS" && this.superUpdateDetails.increaseToLimit == 'Y') {
            this.sgIncreaseToLimit = true;
            this.superCWDetails.increaseToLimit = 'N';
        }
        else if (sgContribution == "Spouse") {
            var exists = this.superCW.some(function (el: any) { return (el.type === "PNC" && el.increaseToLimit === "Y" ) })
            if (exists) {
                this.sgIncreaseToLimit = true;
                this.superCWDetails.increaseToLimit = 'N';
            }
            else {
                this.sgIncreaseToLimit = false;
            }
        }
        else if (sgContribution == "PNC") {
            var exists = this.superCW.some(function (el: any) { return el.type === "Spouse" && el.increaseToLimit === "Y"  })
            if (exists) {
                this.sgIncreaseToLimit = true;
                this.superCWDetails.increaseToLimit = 'N';
            }
            else {
                this.sgIncreaseToLimit = false;
            }
        }
        else {
            this.sgIncreaseToLimit = false;
        }
    }

    changePensionDDType(type: any, index: any, update: any) {
        if (update == '1') {
            $('#pensionDDAmount' + index).attr('placeholder', type);
            if (type === "Custom") {
                $('#pensionDDAmount' + index).prop('disabled', false);
                this.pensionDD[index].amount = 0;
            }
            else {
                $('#pensionDDAmount' + index).prop('disabled', true);
                var MinDrawdown = 0;
                var MaxDrawdown = 0;
                var minRate = 0;
                if (this.pensionUpdateDetails.pensionType === "ABP") {

                    if (this.pensionUpdateDetails.owner == "Client") {
                        var date1 = new Date("7/01/ " + this.currentYear);
                        var date2 = new Date(this.clientDetails.clientDob)
                        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;
                        var clientAge = Math.round(diffDays * 10) / 10;

                        let index: number = 0;
                        Math.max.apply(Math, this.pensionDrawDownAssumptions.map(function (o: any) { if (o.index > index) index = o.index; }))
                        for (var j = 1; j <= index; j++) {

                            let one = this.pensionDrawDownAssumptions.filter((c: any) => c.index === (j - 1));
                            let two = this.pensionDrawDownAssumptions.filter((c: any) => c.index === j);
                            if (j == index) {
                                if (clientAge >= one[0].age) {
                                    minRate = one[0].minRate;
                                }
                            }
                            else {
                                if (clientAge >= one[0].age && clientAge < two[0].age) {
                                    minRate = one[0].minRate;
                                    break;
                                }
                            }


                        }


                    }
                    else {
                        var date1 = new Date("7/01/ " + this.currentYear);
                        var date2 = new Date(this.clientDetails.partnerDob)
                        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;
                        var partnerAge = Math.round(diffDays * 10) / 10;

                        let index: number = 0;
                        Math.max.apply(Math, this.pensionDrawDownAssumptions.map(function (o: any) { if (o.index > index) index = o.index; }))
                        for (var j = 1; j <= index; j++) {

                            let one = this.pensionDrawDownAssumptions.filter((c: any) => c.index === (j - 1));
                            let two = this.pensionDrawDownAssumptions.filter((c: any) => c.index === j);
                            if (j == index) {
                                if (partnerAge >= one[0].age) {
                                    minRate = one[0].minRate;
                                }
                            }
                            else {
                                if (partnerAge >= one[0].age && partnerAge < two[0].age) {
                                    minRate = one[0].minRate;
                                    break;
                                }
                            }


                        }



                    }
                    MinDrawdown = minRate * this.pensionUpdateDetails.totalBalance;
                    MaxDrawdown = this.pensionUpdateDetails.totalBalance;
                }
                else {
                    MinDrawdown = 0;
                    MaxDrawdown = (10 / 100) * this.pensionUpdateDetails.totalBalance;
                }

                if (type === "Maximum") {
                    this.pensionDD[index].amount = MaxDrawdown;
                }
                else if (type === "Minimum") {
                    this.pensionDD[index].amount = MinDrawdown;
                }
                else {
                    this.pensionDD[index].amount = 0;
                }


            }
        }
        else {

            if (type === "Custom") {
                this.pensionDDDetails.amount == 0;
            }
            else {
              
                var MinDrawdown = 0;
                var MaxDrawdown = 0;
                var minRate = 0;
                if (this.pensionUpdateDetails.pensionType === "ABP")  {
                    if (this.pensionUpdateDetails.owner == "Client") {
                        var date1 = new Date("7/01/ " + this.currentYear);
                        var date2 = new Date(this.clientDetails.clientDob)
                        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;
                        var clientAge = Math.round(diffDays * 10) / 10;

                        let index: number = 0;
                        Math.max.apply(Math, this.pensionDrawDownAssumptions.map(function (o: any) { if (o.index > index) index = o.index; }))
                        for (var j = 1; j <= index; j++) {

                            let one = this.pensionDrawDownAssumptions.filter((c: any) => c.index === (j - 1));
                            let two = this.pensionDrawDownAssumptions.filter((c: any) => c.index === j);
                            if (j == index) {
                                if (clientAge >= one[0].age) {
                                    minRate = one[0].minRate;
                                }
                            }
                            else {
                                if (clientAge >= one[0].age && clientAge < two[0].age) {
                                    minRate = one[0].minRate;
                                    break;
                                }
                            }


                        }


                    }
                    else
                    {
                        var date1 = new Date("7/01/ " + this.currentYear);
                        var date2 = new Date(this.clientDetails.partnerDob)
                        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;
                        var partnerAge = Math.round(diffDays * 10) / 10;

                        let index: number = 0;
                        Math.max.apply(Math, this.pensionDrawDownAssumptions.map(function (o: any) { if (o.index > index) index = o.index; }))
                        for (var j = 1; j <= index; j++) {

                            let one = this.pensionDrawDownAssumptions.filter((c: any) => c.index === (j - 1));
                            let two = this.pensionDrawDownAssumptions.filter((c: any) => c.index === j);
                            if (j == index) {
                                if (partnerAge >= one[0].age) {
                                    minRate = one[0].minRate;
                                }
                            }
                            else {
                                if (partnerAge >= one[0].age && partnerAge < two[0].age) {
                                    minRate = one[0].minRate;
                                    break;
                                }
                            }


                        }



                    }
                    MinDrawdown = minRate * this.pensionUpdateDetails.totalBalance;
                    MaxDrawdown = this.pensionUpdateDetails.totalBalance;
                }
                else {
                    MinDrawdown = 0;
                    MaxDrawdown = (10 / 100) * this.pensionUpdateDetails.totalBalance;
                }

                if (type === "Maximum") {
                    this.pensionDDDetails.amount = MaxDrawdown;
                }
                else if (type === "Minimum") {
                    this.pensionDDDetails.amount = MinDrawdown;
                }
                else {
                    this.pensionDDDetails.amount = 0;

                }


            }
        }
       



    } 

    addFieldValue(assetDetails: any) {
        if (assetDetails.type == 'Lifestyle') {
            this.lAssetDetails.name = assetDetails.assetName;
            this.lAssetDetails.owner = assetDetails.owner;
            this.lAssetDetails.value = assetDetails.value;
            this.lAssetDetails.lAssetType = assetDetails.investmentProfile;
          
            if (this.lAssetDetails.startDateType == "Start") {
                this.lAssetDetails.startDate = 0;
            }
            else if (this.lAssetDetails.startDateType == "Existing") {
                this.lAssetDetails.startDate = 0;
            }
            else if (this.lAssetDetails.startDateType == "Client Retirement") {
                this.lAssetDetails.startDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (this.lAssetDetails.startDateType == "Partner Retirement") {
                this.lAssetDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
            }

            if (this.lAssetDetails.endDateType == "End") {
                this.lAssetDetails.endDate = 0;
            }
            else if (this.lAssetDetails.endDateType == "Retain") {
                this.lAssetDetails.endDate = 0;
            }
            else if (this.lAssetDetails.endDateType == "Client Retirement") {
                this.lAssetDetails.endDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (this.lAssetDetails.endDateType == "Partner Retirement") {
                this.lAssetDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
            }

            this.lifestyleAssetService.create(this.lAssetDetails, this.selectedClient).subscribe((data: any) => {
                var exist = false;
                var id$ = data.lassetId;
             
                this.lifestyleAssets.forEach((data1: any, index: any) => {
                    if (data1.lAssetID == id$) {
                        this.lifestyleAssets.splice(index, 1);
                        this.lifestyleAssets.splice(index, 0, data);
                        exist = true;
                    }
                });

                if (exist == false) {
                    this.lifestyleAssets.push(data);
                }



                this.lAssetDetails = {
                    lAssetId: 0,
                    clientId: 0,
                    lAssetType: 'PrimaryResidence',
                    name: '',
                    owner: 'Client',
                    value: 0,
                    growth: 0.0,
                    costBase: 0,
                    startDateType: 'Existing',
                    startDate: 0,
                    endDateType: 'Retain',
                    endDate: 0
                };
                
               
               
            });
        }

        else if (assetDetails.type == 'DirectInvestment') { 
            if (assetDetails.investmentProfile == "Domestic Property") {

                this.propertyDetails.name = assetDetails.assetName;
                this.propertyDetails.owner = assetDetails.owner;
                this.propertyDetails.value = assetDetails.value;

                if (this.propertyDetails.startDateType == "Start") {
                    this.propertyDetails.startDate = 0;
                }
                else if (this.propertyDetails.startDateType == "Existing") {
                    this.propertyDetails.startDate = 0;
                }
                else if (this.propertyDetails.startDateType == "Client Retirement") {
                    this.propertyDetails.startDate = this.clientDetails.clientRetirementYear - 1;
                }
                else if (this.propertyDetails.startDateType == "Partner Retirement") {
                    this.propertyDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
                }

                if (this.propertyDetails.endDateType == "End") {
                    this.propertyDetails.endDate = 0;
                }
                else if (this.propertyDetails.endDateType == "Retain") {
                    this.propertyDetails.endDate = 0;
                }
                else if (this.propertyDetails.endDateType == "Client Retirement") {
                    this.propertyDetails.endDate = this.clientDetails.clientRetirementYear - 1;
                }
                else if (this.propertyDetails.endDateType == "Partner Retirement") {
                    this.propertyDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
                }

                this.propertyService.create(this.propertyDetails, this.selectedClient).subscribe((data: any) => {
                    var exist = false;
                    var id$ = data.propertyId;

                    this.properties.forEach((data1: any, index: any) => {
                        if (data1.propertyId == id$) {
                            this.properties.splice(index, 1);
                            this.properties.splice(index, 0, data);
                            exist = true;
                        }
                    });

                    if (exist == false) {
                        this.properties.push(data);
                    }

                    this.propertyDetails = {
                        propertyId: 0,
                        clientId: 0,
                        name: '',
                        owner: 'Client',
                        value: 0,
                        rent: 0,
                        expenses: 0,
                        growth: 0.0,
                        costBase: 0,
                        startDateType: 'Existing',
                        startDate: 0,
                        endDateType: 'Retain',
                        endDate: 0
                    };



                });
            }
            else {

                this.investmentDetails.name = assetDetails.assetName;
                this.investmentDetails.owner = assetDetails.owner;
                this.investmentDetails.value = assetDetails.value;
                this.investmentDetails.type = assetDetails.investmentProfile;

                if (this.investmentDetails.startDateType == "Start") {
                    this.investmentDetails.startDate = 0;
                }
                else if (this.investmentDetails.startDateType == "Existing") {
                    this.investmentDetails.startDate = 0;
                }
                else if (this.investmentDetails.startDateType == "Client Retirement") {
                    this.investmentDetails.startDate = this.clientDetails.clientRetirementYear - 1;
                }
                else if (this.investmentDetails.startDateType == "Partner Retirement") {
                    this.investmentDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
                }

                if (this.investmentDetails.endDateType == "End") {
                    this.investmentDetails.endDate = 0;
                }
                else if (this.investmentDetails.endDateType == "Retain") {
                    this.investmentDetails.endDate = 0;
                }
                else if (this.investmentDetails.endDateType == "Client Retirement") {
                    this.investmentDetails.endDate = this.clientDetails.clientRetirementYear - 1;
                }
                else if (this.investmentDetails.endDateType == "Partner Retirement") {
                    this.investmentDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
                }

                this.investmentService.create(this.investmentDetails, [], this.selectedClient).subscribe((data: any) => {
                    var exist = false;
                    var id$ = data.investmentId;

                    this.investments.forEach((data1: any, index: any) => {
                        if (data1.investmentId == id$) {
                            this.investments.splice(index, 1);
                            this.investments.splice(index, 0, data);
                            exist = true;
                        }
                    });

                    if (exist == false) {
                        this.investments.push(data);
                    }

                    this.investmentDetails = {
                        investmentId: 0,
                        clientId: 0,
                        type: '',
                        name: '',
                        owner: 'Client',
                        value: 0,
                        growth: 0.00,
                        income: 0.00,
                        franked: 0.00,
                        productFees: 0.0000,
                        costBase: 0,
                        reinvest: 'Y',
                        centrelink: 'N',
                        startDateType: 'Existing',
                        startDate: 0,
                        endDateType: 'Retain',
                        endDate: 0
                    };



                });

            }
        }
        else if (assetDetails.type == 'Super') {
           

            this.superDetails.name = assetDetails.assetName;
            this.superDetails.owner = assetDetails.owner;
            this.superDetails.value = assetDetails.value;
            this.superDetails.type = assetDetails.investmentProfile;
            this.superDetails.taxableComponent = this.superDetails.value - this.superDetails.taxFreeComponent;

           

            if (this.superDetails.startDateType == "Start") {
                this.superDetails.startDate = 0;
                }
            else if (this.superDetails.startDateType == "Existing") {
                this.superDetails.startDate = 0;
                }
            else if (this.superDetails.startDateType == "Client Retirement") {
                this.superDetails.startDate = this.clientDetails.clientRetirementYear - 1;
                }
            else if (this.superDetails.startDateType == "Partner Retirement") {
                this.superDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
                }

            if (this.superDetails.endDateType == "End") {
                this.superDetails.endDate = 0;
                }
            else if (this.superDetails.endDateType == "Retain") {
                this.superDetails.endDate = 0;
                }
            else if (this.superDetails.endDateType == "Client Retirement") {
                this.superDetails.endDate = this.clientDetails.clientRetirementYear - 1;
                }
            else if (this.superDetails.endDateType == "Partner Retirement") {
                this.superDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
                }

            this.superService.create(this.superDetails, [], this.selectedClient).subscribe((data: any) => {
                    var exist = false;
                    var id$ = data.superId;

                    this.supers.forEach((data1: any, index: any) => {
                        if (data1.superId == id$) {
                            this.supers.splice(index, 1);
                            this.supers.splice(index, 0, data);
                            exist = true;
                        }
                    });

                    if (exist == false) {
                        this.supers.push(data);
                    }

                    this.superDetails = {
                        superId: 0,
                        clientId: 0,
                        type: '',
                        name: '',
                        owner: 'Client',
                        value: 0,
                        taxFreeComponent: 0,
                        taxableComponent: 0,
                        growth: 0.00,
                        income: 0.00,
                        franked: 0.00,
                        productFees: 0.0000,
                        insuranceCost: 0,
                        startDateType: 'Existing',
                        startDate: 0,
                        endDateType: 'Retain',
                        endDate: 0,
                        superSalary: 0,
                        increaseToLimit: 'N',
                        sgrate: ''
                    };



                });

            
        }
        else if (assetDetails.type == 'Pension') {


            this.pensionDetails.name = assetDetails.assetName;
            this.pensionDetails.owner = assetDetails.owner;
            this.pensionDetails.value = assetDetails.value;
            this.pensionDetails.type = assetDetails.investmentProfile;
            this.pensionDetails.taxableComponent = this.pensionDetails.value - this.pensionDetails.taxFreeComponent;



            if (this.pensionDetails.pensionRebootFromType == "Start") {
                this.pensionDetails.pensionRebootFromDate = 0;
            }
            else if (this.pensionDetails.pensionRebootFromType == "Existing") {
                this.pensionDetails.pensionRebootFromDate = 0;
            }
            else if (this.pensionDetails.pensionRebootFromType == "Client Retirement") {
                this.pensionDetails.pensionRebootFromDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (this.pensionDetails.pensionRebootFromType == "Partner Retirement") {
                this.pensionDetails.pensionRebootFromDate = this.clientDetails.partnerRetirementYear - 1;
            }

            if (this.pensionDetails.endDateType == "End") {
                this.pensionDetails.endDate = 0;
            }
            else if (this.pensionDetails.endDateType == "Retain") {
                this.pensionDetails.endDate = 0;
            }
            else if (this.pensionDetails.endDateType == "Client Retirement") {
                this.pensionDetails.endDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (this.pensionDetails.endDateType == "Partner Retirement") {
                this.pensionDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
            }

            this.pensionService.create(this.pensionDetails, [], this.selectedClient).subscribe((data: any) => {
                var exist = false;
                var id$ = data.pensionId;

                this.pensions.forEach((data1: any, index: any) => {
                    if (data1.pensionId == id$) {
                        this.pensions.splice(index, 1);
                        this.pensions.splice(index, 0, data);
                        exist = true;
                    }
                });

                if (exist == false) {
                    this.pensions.push(data);
                }

                this.pensionDetails = {
                    pensionId: 0,
                    clientId: 0,
                    type: '',
                    pensionType: 'ABP',
                    name: '',
                    owner: 'Client',
                    value: 0,
                    taxFreeComponent: 0,
                    taxableComponent: 0,
                    growth: 0.00,
                    income: 0.00,
                    franked: 0.00,
                    productFees: 0.0000,
                    totalBalance: 0,
                    pensionRebootFromType: 'Existing',
                    pensionRebootFromDate: 0,
                    endDateType: 'End',
                    endDate: 0
                };



            });


        }

        this.assetDetails = {
            assetName: '',
            owner: 'Client',
            type: 'Lifestyle',
            value: 0,
            investmentProfile: '',

        };

        $('#assetName').attr('placeholder', this.assetDetails.type);
    }
    private checkSGContributions()
    {
    if (this.clientDetails.clientEmpStatus == "Employed") {
        this.superDetails.sgrate = "SGC";
        this.cashFlowService.getCashFlows(this.selectedClient, "I").subscribe(
            cashFlowIncome => {
                this.employment = cashFlowIncome.filter((g: any) => g.type === "Employment");
                var total = 0;
                for (var i = 0; i < this.employment.length; i++) {
                    if (isNaN(this.employment[i].value)) {
                        continue;
                    }
                    total += Number(this.employment[i].value)
                }

                this.superDetails.superSalary = total;

            }
        );



    }
}
    //TODO: change HTML
    UpdateTaxableComponent(value: any, type: any, asset: any) {
        if (asset == "Super") {
            if (type == 'taxfree') {
                this.superUpdateDetails.taxableComponent = this.superUpdateDetails.value - value;

            }
            else {
                this.superUpdateDetails.taxableComponent = value - this.superUpdateDetails.taxFreeComponent;

            }
        }
        else if (asset == "Pension") {
            if (type == 'taxfree') {
                this.pensionUpdateDetails.taxableComponent = this.pensionUpdateDetails.value - value;

            }
            else {
                this.pensionUpdateDetails.taxableComponent = value - this.pensionUpdateDetails.taxFreeComponent;

            }
        }
   
    }
    openModal(item: any, index: any, type: any) {
        if (type == 'lifestyle') {
            this.lAssetUpdateDetails = JSON.parse(JSON.stringify(this.lifestyleAssets[index]));
            this.display = 'block';
            this.lifestyle = 'block';
        }
        else if (type == 'property') {
            this.propertyUpdateDetails = JSON.parse(JSON.stringify(this.properties[index]));
            this.display = 'block';
            this.property = 'block';
        }
        else if (type == 'investment') {
            this.investmentUpdateDetails = JSON.parse(JSON.stringify(this.investments[index]));

            this.investmentService.getInvestmentDetails(this.selectedClient, this.investmentUpdateDetails.investmentId).subscribe(
                investmentDetails => {
                    this.cw = investmentDetails;

                }
            );
            this.display = 'block';
            this.investment = 'block';
        }
        else if (type == 'super') {
            this.superUpdateDetails = JSON.parse(JSON.stringify(this.supers[index]));

            this.superService.getSuperDetails(this.selectedClient, this.superUpdateDetails.superId).subscribe(
                superDetails => {
                    this.superCW = superDetails;

                }
            );
            this.display = 'block';
            this.super = 'block';

            if (this.clientDetails.clientEmpStatus == "Employed") {
                this.notEmployed = false;
            }
        }
        else if (type == 'pension') {
            this.pensionUpdateDetails = JSON.parse(JSON.stringify(this.pensions[index]));

            this.pensionService.getPensionDetails(this.selectedClient, this.pensionUpdateDetails.pensionId).subscribe(
                pensionDetails => {
                    this.pensionDD = pensionDetails;

                }
            );

            this.changePensionDDType(this.pensionDDDetails.type, 0, 0);

            this.display = 'block';
            this.pension = 'block';

           
        }
    }

    //Lifestyle
    updateLifestyle(assetDetails: any) {
            //this.lAssetUpdateDetails.name = assetDetails.name;
            //this.lAssetUpdateDetails.owner = assetDetails.owner;
            //this.lAssetUpdateDetails.value = assetDetails.value;
            //this.lAssetUpdateDetails.lAssetType = assetDetails.lassetType;

        if (this.lAssetUpdateDetails.startDateType == "Start") {
                this.lAssetUpdateDetails.startDate = 0;
            }
        else if (this.lAssetUpdateDetails.startDateType == "Existing") {
                this.lAssetUpdateDetails.startDate = 0;
            }
        else if (this.lAssetUpdateDetails.startDateType == "Client Retirement") {
                this.lAssetUpdateDetails.startDate = this.clientDetails.clientRetirementYear - 1;
            }
        else if (this.lAssetUpdateDetails.startDateType == "Partner Retirement") {
                this.lAssetUpdateDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
            }

        if (this.lAssetUpdateDetails.endDateType == "End") {
                this.lAssetUpdateDetails.endDate = 0;
            }
        else if (this.lAssetUpdateDetails.endDateType == "Retain") {
                this.lAssetUpdateDetails.endDate = 0;
            }
        else if (this.lAssetUpdateDetails.endDateType == "Client Retirement") {
                this.lAssetUpdateDetails.endDate = this.clientDetails.clientRetirementYear - 1;
            }
        else if (this.lAssetUpdateDetails.endDateType == "Partner Retirement") {
                this.lAssetUpdateDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
            }

        this.lifestyleAssetService.create(this.lAssetUpdateDetails, this.selectedClient).subscribe((data: any) => {
                var exist = false;
                var id$ = data.lassetId;

                this.lifestyleAssets.forEach((data1: any, index: any) => {
                    if (data1.lassetId == id$) {
                        this.lifestyleAssets.splice(index, 1);
                        this.lifestyleAssets.splice(index, 0, data);
                        exist = true;
                    }
                });

               



                



            });


        this.onCloseLifestyle();
       
    }  
    deleteLifestyleAsset(item: any, index: any) {
        var id$ = item.lassetId;
        if (this.lifestyleAssets.length > 0) {
            this.lifestyleAssets.forEach((data: any, index: any) => {
                if (data.lassetId == id$) {
                    this.lifestyleAssetService.delete(id$).subscribe((data) => {
                        this.lifestyleAssets.splice(index, 1);
                    });
                }
            });
        }
    }
    onCloseLifestyle() {
        this.display = 'none';
        this.lifestyle = 'none';
        this.lAssetUpdateDetails = {
            lAssetId: 0,
            clientId: 0,
            lAssetType: 'PrimaryResidence',
            name: '',
            owner: 'Client',
            value: 0,
            growth: 0.0,
            costBase: 0,
            startDateType: 'Existing',
            startDate: 0,
            endDateType: 'Retain',
            endDate: 0
        };
    }

    //Property
    updateProperty(assetDetails: any) {
        //this.propertyUpdateDetails.name = assetDetails.name;
        //this.propertyUpdateDetails.owner = assetDetails.owner;
        //this.propertyUpdateDetails.value = assetDetails.value;

        if (this.propertyUpdateDetails.startDateType == "Start") {
            this.propertyUpdateDetails.startDate = 0;
        }
        else if (this.propertyUpdateDetails.startDateType == "Existing") {
            this.propertyUpdateDetails.startDate = 0;
        }
        else if (this.propertyUpdateDetails.startDateType == "Client Retirement") {
            this.propertyUpdateDetails.startDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.propertyUpdateDetails.startDateType == "Partner Retirement") {
            this.propertyUpdateDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
        }

        if (this.propertyUpdateDetails.endDateType == "End") {
            this.propertyUpdateDetails.endDate = 0;
        }
        else if (this.propertyUpdateDetails.endDateType == "Retain") {
            this.propertyUpdateDetails.endDate = 0;
        }
        else if (this.propertyUpdateDetails.endDateType == "Client Retirement") {
            this.propertyUpdateDetails.endDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.propertyUpdateDetails.endDateType == "Partner Retirement") {
            this.propertyUpdateDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
        }

        this.propertyService.create(this.propertyUpdateDetails, this.selectedClient).subscribe((data: any) => {
            var exist = false;
            var id$ = data.propertyId;

            this.properties.forEach((data1: any, index: any) => {
                if (data1.propertyId == id$) {
                    this.properties.splice(index, 1);
                    this.properties.splice(index, 0, data);
                    exist = true;
                }
            });






        });


        this.onCloseProperty();

    }
    deleteProperty(item: any, index: any) {
        var id$ = item.propertyId;
        if (this.properties.length > 0) {
            this.properties.forEach((data: any, index: any) => {
                if (data.propertyId == id$) {
                    this.propertyService.delete(id$).subscribe((data) => {
                        this.properties.splice(index, 1);
                    });
                }
            });
        }
    }
    onCloseProperty() {
        this.display = 'none';
        this.property = 'none';
        this.propertyUpdateDetails = {
            propertyId: 0,
            clientId: 0,
            name: '',
            owner: 'Client',
            value: 0,
            rent: 0,
            expenses: 0,
            growth: 0.0,
            costBase: 0,
            startDateType: 'Existing',
            startDate: 0,
            endDateType: 'Retain',
            endDate: 0
        };
    }

    //Investment
    updateInvestment(assetDetails: any) {
        //this.investmentUpdateDetails.name = assetDetails.name;
        //this.investmentUpdateDetails.owner = assetDetails.owner;
        //this.investmentUpdateDetails.value = assetDetails.value;
        //this.investmentDetails.type = assetDetails.investmentProfile;

        if (this.investmentUpdateDetails.startDateType == "Start") {
            this.investmentUpdateDetails.startDate = 0;
        }
        else if (this.investmentUpdateDetails.startDateType == "Existing") {
            this.investmentUpdateDetails.startDate = 0;
        }
        else if (this.investmentUpdateDetails.startDateType == "Client Retirement") {
            this.investmentUpdateDetails.startDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.investmentUpdateDetails.startDateType == "Partner Retirement") {
            this.investmentUpdateDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
        }

        if (this.investmentUpdateDetails.endDateType == "End") {
            this.investmentUpdateDetails.endDate = 0;
        }
        else if (this.investmentUpdateDetails.endDateType == "Retain") {
            this.investmentUpdateDetails.endDate = 0;
        }
        else if (this.investmentUpdateDetails.endDateType == "Client Retirement") {
            this.investmentUpdateDetails.endDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.investmentUpdateDetails.endDateType == "Partner Retirement") {
            this.investmentUpdateDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
        }

        this.investmentService.create(this.investmentUpdateDetails, this.cw, this.selectedClient).subscribe((data: any) => {
            var exist = false;
            var id$ = data.investmentId;

            this.investments.forEach((data1: any, index: any) => {
                if (data1.investmentId == id$) {
                    this.investments.splice(index, 1);
                    this.investments.splice(index, 0, data);
                    exist = true;
                }
            });






        });


        this.onCloseInvestment();

    }
    deleteInvestment(item: any, index: any) {
        var id$ = item.investmentId;
        if (this.investments.length > 0) {
            this.investments.forEach((data: any, index: any) => {
                if (data.investmentId == id$) {
                    this.investmentService.delete(id$).subscribe((data) => {
                        this.investments.splice(index, 1);
                    });
                }
            });
        }
    }
    onCloseInvestment() {
        this.display = 'none';
        this.investment = 'none';
        this.investmentUpdateDetails = {
            investmentId: 0,
            clientId: 0,
            type: '',
            name: '',
            owner: 'Client',
            value: 0,
            growth: 0.00,
            income: 0.00,
            franked: 0.00,
            productFees: 0.0000,
            costBase: 0,
            reinvest: 'Y',
            centrelink: 'N',
            startDateType: 'Existing',
            startDate: 0,
            endDateType: 'Retain',
            endDate: 0
        };
    }

    //InvestmentDetails
    AddTempCW(cwDetails: any) {

        var index$ = cwDetails.index;
        var exist = false;
        if (this.cw.length > 0) {
            this.cw.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.cw.splice(index, 1);
                    this.cw.splice(index, 0, cwDetails);
                    exist = true;

                }

            });
        }
        if (exist == false) {
            if (cwDetails.fromDateType == "Start") {
                cwDetails.fromDate = 0;
            }
            else if (cwDetails.fromDateType == "Client Retirement") {
                cwDetails.fromDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (cwDetails.fromDateType == "Partner Retirement") {
                cwDetails.fromDate = this.clientDetails.partnerRetirementYear - 1;
            }

            if (cwDetails.toDateType == "End") {
                cwDetails.toDate = 0;
            }
            else if (cwDetails.toDateType == "Client Retirement") {
                cwDetails.toDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (cwDetails.toDateType == "Partner Retirement") {
                cwDetails.toDate = this.clientDetails.partnerRetirementYear - 1;
            }
            this.cw.push(cwDetails);
        }

        this.cwDetails = {
            investmentId: 0,
            clientId: 0,
            type: 'C',
            value: 0,
            fromDateType: 'Start',
            fromDate: 0,
            toDateType: 'End',
            toDate: 0
        };
    }
    DeleteTempCW(item: any, index: any) {

        var index$ = index;
        if (this.cw.length > 0) {
            this.cw.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.cw.splice(index, 1);
                }

            });
        }
    }


    //Super
    updateSuper(assetDetails: any) {

        if (this.superUpdateDetails.startDateType == "Start") {
            this.superUpdateDetails.startDate = 0;
        }
        else if (this.superUpdateDetails.startDateType == "Existing") {
            this.superUpdateDetails.startDate = 0;
        }
        else if (this.superUpdateDetails.startDateType == "Client Retirement") {
            this.superUpdateDetails.startDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.superUpdateDetails.startDateType == "Partner Retirement") {
            this.superUpdateDetails.startDate = this.clientDetails.partnerRetirementYear - 1;
        }

        if (this.superUpdateDetails.endDateType == "End") {
            this.superUpdateDetails.endDate = 0;
        }
        else if (this.superUpdateDetails.endDateType == "Retain") {
            this.superUpdateDetails.endDate = 0;
        }
        else if (this.superUpdateDetails.endDateType == "Client Retirement") {
            this.superUpdateDetails.endDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.superUpdateDetails.endDateType == "Partner Retirement") {
            this.superUpdateDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
        }

        this.superService.create(this.superUpdateDetails, this.superCW, this.selectedClient).subscribe((data: any) => {
            var exist = false;
            var id$ = data.superId;

            this.supers.forEach((data1: any, index: any) => {
                if (data1.superId == id$) {
                    this.supers.splice(index, 1);
                    this.supers.splice(index, 0, data);
                    exist = true;
                }
            });

        });


        this.onCloseSuper();

    }
    deleteSuper(item: any, index: any) {
        var id$ = item.superId;
        if (this.supers.length > 0) {
            this.supers.forEach((data: any, index: any) => {
                if (data.superId == id$) {
                    this.superService.delete(id$).subscribe((data) => {
                        this.supers.splice(index, 1);
                    });
                }
            });
        }
    }
    onCloseSuper() {
        this.display = 'none';
        this.super = 'none';
        this.superUpdateDetails = {
            superId: 0,
            clientId: 0,
            type: '',
            name: '',
            owner: 'Client',
            value: 0,
            taxFreeComponent: 0,
            taxableComponent: 0,
            growth: 0.00,
            income: 0.00,
            franked: 0.00,
            productFees: 0.0000,
            insuranceCost: 0,
            startDateType: 'Existing',
            startDate: 0,
            endDateType: 'Retain',
            endDate: 0,
            superSalary: 0,
            increaseToLimit: 'N',
            sgrate: ''
        };

       
    }

    //InvestmentDetails
    AddTempSuperCW(superCWDetails: any) {

        var index$ = superCWDetails.index;
        var exist = false;
        if (this.superCW.length > 0) {
            this.superCW.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.superCW.splice(index, 1);
                    this.superCW.splice(index, 0, superCWDetails);
                    exist = true;

                }

            });
        }
        if (exist == false) {
            if (superCWDetails.fromDateType == "Start") {
                superCWDetails.fromDate = 0;
            }
            else if (superCWDetails.fromDateType == "Client Retirement") {
                superCWDetails.fromDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (superCWDetails.fromDateType == "Partner Retirement") {
                superCWDetails.fromDate = this.clientDetails.partnerRetirementYear - 1;
            }

            if (superCWDetails.toDateType == "End") {
                superCWDetails.toDate = 0;
            }
            else if (superCWDetails.toDateType == "Client Retirement") {
                superCWDetails.toDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (superCWDetails.toDateType == "Partner Retirement") {
                superCWDetails.toDate = this.clientDetails.partnerRetirementYear - 1;
            }
            this.superCW.push(superCWDetails);
        }

        this.superCWDetails = {
            superId: 0,
            clientId: 0,
            type: 'C',
            amount: 0,
            increaseToLimit: 'N',
            fromDateType: 'Start',
            fromDate: 0,
            toDateType: 'End',
            toDate: 0
        };
        this.sgIncreaseToLimit = false;
    }
    DeleteTempSuperCW(item: any, index: any) {

        var index$ = index;
        if (this.superCW.length > 0) {
            this.superCW.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.superCW.splice(index, 1);
                }

            });
        }
    }
   
    //Pension
    updatePension(assetDetails: any) {

        if (this.pensionUpdateDetails.pensionRebootFromType == "Start") {
            this.pensionUpdateDetails.pensionRebootFromDate = 0;
        }
        else if (this.pensionUpdateDetails.pensionRebootFromType == "Existing") {
            this.pensionUpdateDetails.pensionRebootFromDate = 0;
        }
        else if (this.pensionUpdateDetails.pensionRebootFromType == "Client Retirement") {
            this.pensionUpdateDetails.pensionRebootFromDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.pensionUpdateDetails.pensionRebootFromType == "Partner Retirement") {
            this.pensionUpdateDetails.pensionRebootFromDate = this.clientDetails.partnerRetirementYear - 1;
        }

        if (this.pensionUpdateDetails.endDateType == "End") {
            this.pensionUpdateDetails.endDate = 0;
        }
        else if (this.pensionUpdateDetails.endDateType == "Retain") {
            this.pensionUpdateDetails.endDate = 0;
        }
        else if (this.pensionUpdateDetails.endDateType == "Client Retirement") {
            this.pensionUpdateDetails.endDate = this.clientDetails.clientRetirementYear - 1;
        }
        else if (this.pensionUpdateDetails.endDateType == "Partner Retirement") {
            this.pensionUpdateDetails.endDate = this.clientDetails.partnerRetirementYear - 1;
        }

        this.pensionService.create(this.pensionUpdateDetails, this.pensionDD, this.selectedClient).subscribe((data: any) => {
            var exist = false;
            var id$ = data.pensionId;

            this.pensions.forEach((data1: any, index: any) => {
                if (data1.pensionId == id$) {
                    this.pensions.splice(index, 1);
                    this.pensions.splice(index, 0, data);
                    exist = true;
                }
            });

        });


        this.onClosePension();

    }
    deletePension(item: any, index: any) {
        var id$ = item.pensionId;
        if (this.pensions.length > 0) {
            this.pensions.forEach((data: any, index: any) => {
                if (data.pensionId == id$) {
                    this.pensionService.delete(id$).subscribe((data) => {
                        this.pensions.splice(index, 1);
                    });
                }
            });
        }
    }
    onClosePension() {
        this.display = 'none';
        this.pension = 'none';
        this.pensionUpdateDetails = {
            pensionId: 0,
            clientId: 0,
            type: '',
            pensionType: 'ABP',
            name: '',
            owner: 'Client',
            value: 0,
            taxFreeComponent: 0,
            taxableComponent: 0,
            growth: 0.00,
            income: 0.00,
            franked: 0.00,
            productFees: 0.0000,
            totalBalance: 0,
            pensionRebootFromType: 'Existing',
            pensionRebootFromDate: 0,
            endDateType: 'End',
            endDate: 0 
        };


    }
   
    //PensionDetails
    AddTempPensionDD(pensionddDetails: any) {

        var index$ = pensionddDetails.index;
        var exist = false;
        if (this.pensionDD.length > 0) {
            this.pensionDD.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.pensionDD.splice(index, 1);
                    this.pensionDD.splice(index, 0, pensionddDetails);
                    exist = true;

                }

            });
        }
        if (exist == false) {
            if (pensionddDetails.fromDateType == "Start") {
                pensionddDetails.fromDate = 0;
            }
            else if (pensionddDetails.fromDateType == "Client Retirement") {
                pensionddDetails.fromDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (pensionddDetails.fromDateType == "Partner Retirement") {
                pensionddDetails.fromDate = this.clientDetails.partnerRetirementYear - 1;
            }
            else if (pensionddDetails.fromDateType == "EndTTR") {
                pensionddDetails.fromDate = 0;
            }

            if (pensionddDetails.toDateType == "End") {
                pensionddDetails.toDate = 0;
            }
            else if (pensionddDetails.toDateType == "Client Retirement") {
                pensionddDetails.toDate = this.clientDetails.clientRetirementYear - 1;
            }
            else if (pensionddDetails.toDateType == "Partner Retirement") {
                pensionddDetails.toDate = this.clientDetails.partnerRetirementYear - 1;
            }
            else if (pensionddDetails.toDateType == "EndTTR") {
                pensionddDetails.toDate = 0;
            }
            this.pensionDD.push(pensionddDetails);
        }

        this.pensionDDDetails = {
            pensionId: 0,
            clientId: 0,
            type: 'Minimum',
            amount: 0,
            fromDateType: 'Start',
            fromDate: 0,
            toDateType: 'End',
            toDate: 0
        };

        $('#pensionDDAmountVal').prop('disabled', true);
    }
    DeleteTempPensionDD(item: any, index: any) {

        var index$ = index;
        if (this.pensionDD.length > 0) {
            this.pensionDD.forEach((data: any, index: any) => {
                if (index == index$) {
                    this.pensionDD.splice(index, 1);
                }

            });
        }
    }

    


}
