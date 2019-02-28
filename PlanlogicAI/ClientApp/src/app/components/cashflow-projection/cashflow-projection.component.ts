import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../../services/client.service';
import { ToastyService } from 'ng2-toasty';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';
import { Client, BasicDetails } from './../../models/Client';
import { CommonService } from '../../services/common.service';
import { CashFlowService } from '../../services/cashFlow.service';
import { CashflowExpenditureComponent } from '../cashflow-expenditure/cashflow-expenditure.component';
import { CashFlow } from '../../models/CashFlow';

@Component({
  selector: 'app-cashflow-projection',
  templateUrl: './cashflow-projection.component.html',
  styleUrls: ['./cashflow-projection.component.css']
})
export class CashflowProjectionComponent implements OnInit {
    clientAge: number = 0;
    partnerAge: number = 0;
    years: any[] = [];
    selectedClient: any = {};
    cashFlowIncome: CashFlow[] = []; 
    cashFlowExpenditure: CashFlow[] = [];
    period: number = 0;

    cfiClient: any = [];
    cfiPartner: any = [];
    cfeClient: any = [];
    cfePartner: any = [];
    cfeJoint: any = [];
    inflow: any[] = [];
    outflow: any[] = [];
    net: any[] = [];

    clientRetirementYear: number = 0;
    partnerRetirementYear: number = 0;

    //total: number = 10;
    constructor(private route: ActivatedRoute,
        private router: Router, private cashFlowService: CashFlowService,
        private clientService: ClientService, private commonService: CommonService) { }

    ngOnInit()  {

        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
        var years = [];
        var sources = [];
        var age: number;
        this.period = this.selectedClient.period + 1;

        this.clientRetirementYear = this.selectedClient.clientRetirementYear;
        this.partnerRetirementYear = this.selectedClient.partnerRetirementYear;

        if (this.selectedClient.clientId) {
            sources.push(this.cashFlowService.getCashFlows(this.selectedClient.clientId, "I"));
            sources.push(this.cashFlowService.getCashFlows(this.selectedClient.clientId, "E"));
            //sources.push(this.commonService.getGeneralAssumptions());
        }

           Observable.forkJoin(sources).subscribe((data:any[]) => {
               if (this.selectedClient.clientId) {
                   this.cashFlowIncome = data[0];
                   this.cashFlowExpenditure = data[1];

                   this.cfiClient = this.cashFlowIncome.filter(c => c.owner === "Client");
                   this.cfiPartner = this.cashFlowIncome.filter(c => c.owner === "Partner");

                   this.cfeClient = this.cashFlowExpenditure.filter(c => c.owner === "Client");
                   this.cfePartner = this.cashFlowExpenditure.filter(c => c.owner === "Partner");
                   this.cfeJoint = this.cashFlowExpenditure.filter(c => c.owner === "Joint");


                   //Calculate Inflow Values
                   var indexRangeInflow: any = [];
                   this.cfiClient.forEach((x: any) => { // client
                       var obj: any = {};
                       var obj1: any = {};
                       obj["owner"] = x.owner;
                       obj["name"] = x.cfname;
                       var j = 1;
                       for (var i = 0; i < this.selectedClient.period; i++) {

                           if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
                               if (j == 1) {
                                   obj1[this.selectedClient.startDate + i] = x.value.toFixed();

                               }
                               else {
                                   obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
                               }
                               j++;
                           }
                           else {
                               obj1[this.selectedClient.startDate + i] = "-";
                           }
                           obj["values"] = obj1;

                       }
                       indexRangeInflow.push(obj);

                   })
                   this.cfiPartner.forEach((x: any) => { // partner
                       var obj: any = {};
                       var obj1: any = {};
                       obj["owner"] = x.owner;
                       obj["name"] = x.cfname;
                       var j = 1;
                       for (var i = 0; i < this.selectedClient.period; i++) {

                           if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
                               if (j == 1) {
                                   obj1[this.selectedClient.startDate + i] = x.value.toFixed();

                               }
                               else {
                                   obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
                               }
                               j++;
                           }
                           else {
                               obj1[this.selectedClient.startDate + i] = "-";
                           }
                           obj["values"] = obj1;

                       }
                       indexRangeInflow.push(obj);

                   })

                   this.inflow = indexRangeInflow;

                   //Calculate Outflow Values
                   var indexRangeOutflow: any = [];
                   this.cfeClient.forEach((x: any) => { // client
                       var obj: any = {};
                       var obj1: any = {};
                       obj["owner"] = x.owner;
                       obj["name"] = x.cfname;
                       var j = 1;
                       for (var i = 0; i < this.selectedClient.period; i++) {

                           if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
                               if (j == 1) {
                                   obj1[this.selectedClient.startDate + i] = x.value.toFixed();

                               }
                               else {
                                   obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
                               }
                               j++;
                           }
                           else {
                               obj1[this.selectedClient.startDate + i] = "-";
                           }
                           obj["values"] = obj1;

                       }
                       indexRangeOutflow.push(obj);

                   })
                   this.cfePartner.forEach((x: any) => { // partner
                       var obj: any = {};
                       var obj1: any = {};
                       obj["owner"] = x.owner;
                       obj["name"] = x.cfname;
                       var j = 1;
                       for (var i = 0; i < this.selectedClient.period; i++) {

                           if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
                               if (j == 1) {
                                   obj1[this.selectedClient.startDate + i] = x.value.toFixed();

                               }
                               else {
                                   obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
                               }
                               j++;
                           }
                           else {
                               obj1[this.selectedClient.startDate + i] = "-";
                           }
                           obj["values"] = obj1;

                       }
                       indexRangeOutflow.push(obj);

                   })
                   this.cfeJoint.forEach((x: any) => { // joint
                       var obj: any = {};
                       var obj1: any = {};
                       obj["owner"] = x.owner;
                       obj["name"] = x.cfname;
                       var j = 1;
                       for (var i = 0; i < this.selectedClient.period; i++) {

                           if (x.startDate <= this.selectedClient.startDate + i && x.endDate >= this.selectedClient.startDate + i) {
                               if (j == 1) {
                                   obj1[this.selectedClient.startDate + i] = x.value.toFixed();

                               }
                               else {
                                   obj1[this.selectedClient.startDate + i] = (x.value * (Math.pow((1 + x.indexation / 100), j))).toFixed();
                               }
                               j++;
                           }
                           else {
                               obj1[this.selectedClient.startDate + i] = "-";
                           }
                           obj["values"] = obj1;

                       }
                       indexRangeOutflow.push(obj);

                   })

                   this.outflow = indexRangeOutflow;

                   //Calculate Totals
                   this.calculateTotalIncome("Total-client", "Client");
                   this.calculateTotalIncome("Total-partner", "Partner");
                   this.calculateTotalExpenditure("Total-client", "Client");
                   this.calculateTotalExpenditure("Total-partner", "Partner");
                   this.calculateTotalExpenditure("Total-joint", "Joint");
                   this.calculateTotalInflows();
                   this.calculateTotalOutflows();
                   this.calculateNetCashflow();
               }

           }, err => {
                      if (err.status == 404)
                            this.router.navigate(['/home']);
                    });

        this.setYear();
        this.setClientAge();
        if (this.selectedClient.maritalStatus == 'M') {
            this.setPartnerAge();
        }
       
    }


    private calculateTotalIncome(owner: string, filter: string) {

          var total: any = {};
                   var totalVal: any = {};
                   total["owner"] = owner;
                   total["name"] = "Income";
                   for (var i = 0; i < this.selectedClient.period; i++) {
                       let sum: number = 0;
                       this.inflow.forEach((x: any) => {
                           if (x.owner == filter) {
                               if (x.values[this.selectedClient.startDate + i] != "-") {
                                   var t = parseInt(x.values[this.selectedClient.startDate + i]);
                                   sum = sum + t;
                               }
                           }
                       });
                       if (sum > 0) {
                           totalVal[this.selectedClient.startDate + i] = sum;
                       } else {
                           totalVal[this.selectedClient.startDate + i] = "-";
                       }
                       total["values"] = totalVal;

                   }

        this.inflow.push(total);
    }
    private calculateTotalExpenditure(owner: string, filter: string) {
        var total: any = {};
        var totalVal: any = {};
        total["owner"] = owner;
        total["name"] = "Expenditure";
        for (var i = 0; i < this.selectedClient.period; i++) {
            let sum: number = 0;
            this.outflow.forEach((x: any) => {
                if (x.owner == filter) {
                    if (x.values[this.selectedClient.startDate + i] != "-") {
                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
                        sum = sum + t;
                    }
                }
            });
            if (sum > 0) {
                totalVal[this.selectedClient.startDate + i] = sum;
            } else {
                totalVal[this.selectedClient.startDate + i] = "-";
            }
            total["values"] = totalVal;

        }

        this.outflow.push(total);
    }
    private calculateTotalInflows() {
        var total: any = {};
        var totalVal: any = {};
        total["owner"] = "Inflow";
        total["name"] = "Inflow";
        for (var i = 0; i < this.selectedClient.period; i++) {
            let sum: number = 0;
            this.inflow.forEach((x: any) => {
                if (x.owner == "Total-client" || x.owner == "Total-partner") {
                    if (x.values[this.selectedClient.startDate + i] != "-") {
                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
                        sum = sum + t;
                    }
                }
            });
            if (sum > 0) {
                totalVal[this.selectedClient.startDate + i] = sum;
            } else {
                totalVal[this.selectedClient.startDate + i] = "-";
            }
            total["values"] = totalVal;

        }

        this.inflow.push(total);



    }
    private calculateTotalOutflows() {
        var total: any = {};
        var totalVal: any = {};
        total["owner"] = "Outflow";
        total["name"] = "Outflow";
        for (var i = 0; i < this.selectedClient.period; i++) {
            let sum: number = 0;
            this.outflow.forEach((x: any) => {
                if (x.owner == "Total-client" || x.owner == "Total-partner" || x.owner == "Total-joint") {
                    if (x.values[this.selectedClient.startDate + i] != "-") {
                        var t = parseInt(x.values[this.selectedClient.startDate + i]);
                        sum = sum + t;
                    }
                }
            });
            if (sum > 0) {
                totalVal[this.selectedClient.startDate + i] = sum;
            } else {
                totalVal[this.selectedClient.startDate + i] = "-";
            }
            total["values"] = totalVal;

        }

        this.outflow.push(total);



    }
    private calculateNetCashflow() {
        var total: any = {};
        var totalVal: any = {};
        total["owner"] = "NetCashflow";
        total["name"] = "NetCashflow";
        var inflow = this.inflow.filter(c => c.name === "Inflow");
        var outflow = this.outflow.filter(c => c.name === "Outflow");
        for (var i = 0; i < this.selectedClient.period; i++) {
          

           
            var diff = 0;
            if (typeof inflow[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(inflow[0].values[this.selectedClient.startDate + i]) && typeof outflow[0].values[this.selectedClient.startDate + i] === "number" && !isNaN(outflow[0].values[this.selectedClient.startDate + i]))
            {
                diff = inflow[0].values[this.selectedClient.startDate + i] - outflow[0].values[this.selectedClient.startDate + i];
            }

            if (diff > 0) {
                totalVal[this.selectedClient.startDate + i] = diff;
            } else if (diff < 0) {
                totalVal[this.selectedClient.startDate + i] = diff;
            }
            else {
                totalVal[this.selectedClient.startDate + i] = "-";
            }
            total["values"] = totalVal;

           

        }
        this.net.push(total);

    }
    private setYear() {
        var range = [];
        range.push(this.selectedClient.startDate);
        for (var i = 1; i < this.selectedClient.period; i++) {
            range.push(this.selectedClient.startDate + i);
        }

        this.years = range;
    }

    //TODO: Test all scenarios
    private setClientAge() {
        var date1 = new Date("7/01/ " + this.selectedClient.startDate);
        var date2 = new Date(this.selectedClient.clientDob);
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24)))/365;

        this.clientAge = Math.round(diffDays * 10) / 10;

    }
    private setPartnerAge() {

        var date1 = new Date("7/01/ " + this.selectedClient.startDate);
        var date2 = new Date(this.selectedClient.partnerDob);
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;

        this.partnerAge = Math.round(diffDays * 10) / 10;
    }

    
}
