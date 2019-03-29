import { Client, BasicDetails} from './../../models/Client';
import { ClientService } from './../../services/client.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import * as _moment from 'moment';

//import { default as _rollupMoment } from 'moment';
import { FormControl } from '@angular/forms';
import { MatDatepickerInputEvent } from '@angular/material';
const moment = _moment;

export const MY_FORMATS = {
  parse: {
    dateInput: 'YYYY-MM-DD',
  },
  display: {
    dateInput: 'YYYY-MM-DD',
    monthYearLabel: 'MM YYYY',
    dateA11yLabel: 'L',
    monthYearA11yLabel: 'MM YYYY',
  },
};



@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css'],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: DateAdapter, useClass: MomentDateAdapter },
   { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})
export class ClientListComponent implements OnInit {
    clients: Client[] = [];
    display = 'none';

    cl: any = {};
    clientDOB = new FormControl(moment());
    partnerDOB = new FormControl(moment());
    client: BasicDetails = {
        clientId: 0,
        familyName: '',
        clientName: '',
        clientDob: new Date(),
        clientEmpStatus: 'Employed',
        clientRetirementYear: 0,
        clientRiskProfile: 'Preservation',
        clientPrivateHealthInsurance: 'Y',
        maritalStatus: 'S',
        partnerName: '',
        partnerDob: new Date(),
        partnerEmpStatus: 'Employed',
        partnerRetirementYear: 0,
        partnerRiskProfile: 'Preservation',
        jointRiskProfile: 'Preservation',
        partnerPrivateHealthInsurance: 'Y',
        startDate: 0,
        period: 0,
        noOfDependents: 0,
    };
    showHide: boolean = true;
    isMarried: boolean = false;
    empStatuses = ["Employed","Self-employed", "Retired", "Unemployed"];
    booleanValues = [
        { id: "Y", name: "Yes" },
        { id: "N", name: "No" }

    ];
    maritalStatus = [
        { id: "S", name: "Individual" },
        { id: "M", name: "Couple" }

    ];
    riskProfiles = ["Preservation", "Defensive", "Moderate", "Balanced", "Growth", "High Growth"];
    clientEmpStatusDisabled: boolean = true;
    partnerEmpStatusDisabled: boolean = true;
    retirement: any[] = [];
    currentYear = new Date().getFullYear();
    currentMonth = new Date().getMonth();
    selectedClient: any;

    constructor(private route: ActivatedRoute,
        private router: Router,private clientService: ClientService) { }

  ngOnInit() {
       
        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
        this.showHide = false;
        this.clientEmpStatusDisabled = false;
        this.partnerEmpStatusDisabled = false;
        this.isMarried = false;

        this.clientService.getClient(this.selectedClient).subscribe((clients: any) => {
                this.cl = clients;
                this.client.familyName = clients.familyName;

            }
        );

        this.clientService.getBasicDetails(this.selectedClient).subscribe((basicDetails: any)=> {
                this.client = basicDetails;
                this.client.clientDob = new Date(basicDetails.clientDob);
                this.clientEmpStatusChange(this.client.clientEmpStatus);
                this.clientMaritalStatusChange(this.client.maritalStatus);
                this.partnerEmpStatusChange(this.client.partnerEmpStatus);
                this.setRetirementYear();
            }
        );
      
    }

    openModal() {
        this.display = 'block';
    }
    onCloseHandled() {
        this.display = 'none';
    }

    private setRetirementYear() {

        if (this.currentMonth < 6) {
            this.currentYear = this.currentYear - 1;

        }
        var range = [];
        var obj: any = {};
        obj["id"] = this.currentYear;
        obj["name"] = "01/07/" + this.currentYear;
        var date1 = new Date("7/01/ " + this.currentYear);
        var date2 = new Date(this.client.clientDob)
        var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        var diffDays = (Math.ceil(timeDiff / (1000 * 3600 * 24))) / 365;

        obj["clientage"] = Math.round(diffDays * 10) / 10;

        var date3 = new Date(this.client.partnerDob);
        var timeDiff1 = Math.abs(date3.getTime() - date1.getTime());
        var diffDays1 = (Math.ceil(timeDiff1 / (1000 * 3600 * 24))) / 365;
        obj["partnerage"] = Math.round(diffDays1 * 10) / 10;
        range.push(obj);

        //TODO : Upto age 100
        for (var i = 1; i < 41; i++) {
            var obj1: any = {};
            var val = this.currentYear + i;
            obj1["id"] = val;
            obj1["name"] = "01/07/" + val;

            var datea = new Date("7/01/ " + val);
            var dateb = new Date(this.client.clientDob);
            var timeDiffa = Math.abs(dateb.getTime() - datea.getTime());
            var diffDaysa = (Math.ceil(timeDiffa / (1000 * 3600 * 24))) / 365;

            obj1["clientage"] = Math.round(diffDaysa * 10) / 10;


            var datec = new Date(this.client.partnerDob);
            var timeDiffb = Math.abs(datec.getTime() - datea.getTime());
            var diffDaysb = (Math.ceil(timeDiffb / (1000 * 3600 * 24))) / 365;
            obj1["partnerage"] = Math.round(diffDaysb * 10) / 10;
            range.push(obj1);
        }
        this.retirement = range;

        if (this.client.clientRetirementYear == 0) {

            this.client.clientRetirementYear = this.retirement.filter(x => Math.floor(x.clientage) == 65).length > 0 ? this.retirement.filter(x => Math.floor(x.clientage) == 65)[0].id : 0;
        }
        if (this.client.partnerRetirementYear == 0) {

            this.client.partnerRetirementYear = this.retirement.filter(x => Math.floor(x.partnerage) == 65).length > 0 ? this.retirement.filter(x => Math.floor(x.partnerage) == 65)[0].id : 0;
        }
    }

    ShowButton() {
        this.showHide = true;
        this.isMarried = true;
    }
    HideButton() {
        this.showHide = false;
        this.isMarried = false;
    }

    public clientEmpStatusChange(event: any): void {

        if (event == "Retired") {
            this.client.clientRetirementYear = 0;
            this.clientEmpStatusDisabled = true;

        } else {
            this.clientEmpStatusDisabled = false;
        }
    }
    public clientMaritalStatusChange(event: any): void {

        if (event == "S") {
            this.showHide = false;
            this.isMarried = false;

        } else {
            this.showHide = true;
            this.isMarried = true;
        }
    }
    public partnerEmpStatusChange(event: any): void {

        if (event == "Retired") {
            this.client.partnerRetirementYear = 0;
            this.partnerEmpStatusDisabled = true;
        } else {
            this.partnerEmpStatusDisabled = false;
        }
    }

    submitted = false;

    onSubmit() {
        this.cl.familyName = this.client.familyName;
        this.clientService.createBasicDetails(this.client, this.cl, this.selectedClient).subscribe((data) => {
            localStorage.removeItem('ClientDetails');
            localStorage.setItem('ClientDetails', JSON.stringify(data));
          this.router.navigate(['counter/current-position']);

          console.log(JSON.stringify(this.client));
        });
    }
  changeDOB(event: MatDatepickerInputEvent<Date>, type: any) {
        if (type == 'client') {
            //var val = value.toUTCString();
          //var dt = moment(val, 'YYYY-MM-DD').toDate();
          this.client.clientDob = event.value;

        }
        else {
          this.client.partnerDob = event.value;
        }
        this.setRetirementYear();
    }

    //onValueChange(value: Date): void {
    //    this.data = value;
    //}
}
