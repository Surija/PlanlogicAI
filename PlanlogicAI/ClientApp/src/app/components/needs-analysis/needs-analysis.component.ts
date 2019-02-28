import { Component, OnInit, NgZone } from '@angular/core';
import { NeedsAnalysisService } from './../../services/needs-analysis.service';
import * as $ from 'jquery';
import * as XLSX from 'xlsx';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'needs-analysis',
  templateUrl: './needs-analysis.component.html',
  styleUrls: ['./needs-analysis.component.css']
})
export class NeedsAnalysisComponent implements OnInit {

    clientDetails: any;
    selectedClient: any;
  needsAnalysis: any[] = [];
  clientNeedsAnalysis: any[] = [];
  partnerNeedsNanlysis: any[] = [];
  clientNeeds: any = {
    recId: 0,
    description: '',
    clientId:0,
    owner: 'Client',
    life: "",
    tpd: "",
    trauma: "",
    incomeProtection: "",
    isDefault: 0
  }

  totalClientNeeds: any = {
    life: 0,
    tpd: 0,
    trauma: 0,
    incomeProtection: 0,
  }

  totalPartnerNeeds: any = {
    life: 0,
    tpd: 0,
    trauma: 0,
    incomeProtection: 0,
  }

  partnerNeeds : any = {
    recId: 0,
    description: '',
    clientId: 0,
    owner: 'Partner',
    life: "",
    tpd: "",
    trauma: "",
    incomeProtection: "",
    isDefault: 0
  }

    constructor(private route: ActivatedRoute, 
      private router: Router, private zone: NgZone, private needsAnalysisService: NeedsAnalysisService) {
    }

    ngOnInit() {

        this.selectedClient = JSON.parse(localStorage.getItem('selectedClient') || '{}');
        this.clientDetails = JSON.parse(localStorage.getItem('ClientDetails') || '{}');

        console.log(this.selectedClient);
        this.clientNeeds.clientId = this.selectedClient;
        this.partnerNeeds.clientId = this.selectedClient;

      this.needsAnalysisService.getNeedsAnalysis(this.selectedClient).subscribe((data: any) => {
        this.needsAnalysis = data;
        if (this.needsAnalysis.length <= 0) {
          this.clientNeedsAnalysis = [
            { recId: 0, description: 'Total liabilities to be paid out (e.g.- mortgage, etc)', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault : 1},
            { recId: 0, description: 'Children/s education expenses', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1 },
            { recId: 0, description: 'Gross income to be replaced', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1},
            { recId: 0, description: 'For how long that income needs to be replaced', clientId: this.selectedClient, owner: 'Client', life: "", tpd: "", trauma: "", incomeProtection: "", isDefault: 1},
            { recId: 0, description: 'Funeral expenses', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1},
            { recId: 0, description: 'Medical expenses', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1 },
            { recId: 0, description: 'Amount of gross income to cover', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 },
            { recId: 0, description: 'Waiting period', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 },
            { recId: 0, description: 'Benefit period', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 },
            { recId: 0, description: 'Total Cover Recommended', clientId: this.selectedClient, owner: 'Client', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 }
          ];
         
        }
        else {
          this.needsAnalysis.forEach((x: any) => {
            x.life = Number(x.life);
            x.tpd = Number(x.tpd);
            x.trauma = Number(x.trauma);

            if (x.description == 'Total Cover Recommended' || x.description == 'Amount of gross income to cover' || x.description == 'Waiting period' || x.description == 'Benefit period') {
              x.incomeProtection = x.incomeProtection.toString();
            }
            else {
              x.incomeProtection = Number(x.incomeProtection);

            }
          });
          this.clientNeedsAnalysis = this.needsAnalysis;
        }

        this.calculateClientTotal();
        }, err => {
            if (err.status == 404)
                this.router.navigate(['/home']);
        });

    

      if (this.clientDetails.maritalStatus == 'M') {
        this.needsAnalysisService.getPartnerNeedsAnalysis(this.selectedClient).subscribe((data: any) => {
          this.needsAnalysis = data;
          if (this.needsAnalysis.length <= 0) {      
            this.partnerNeedsNanlysis = [
              { recId: 0, description: 'Total liabilities to be paid out (e.g.- mortgage)', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1 },
              { recId: 0, description: 'Children/s education expenses', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1 },
              { recId: 0, description: 'Gross income to be replaced', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1 },
              { recId: 0, description: 'For how long that income needs to be replaced', clientId: this.selectedClient, owner: 'Partner', life: "", tpd: "", trauma: "", incomeProtection: "", isDefault: 1 },
              { recId: 0, description: 'Funeral expenses', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1 },
              { recId: 0, description: 'Medical expenses', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: 0, isDefault: 1 },
              { recId: 0, description: 'Amount of gross income to cover', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 },
              { recId: 0, description: 'Waiting period', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 },
              { recId: 0, description: 'Benefit period', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 },
              { recId: 0, description: 'Total Cover Recommended', clientId: this.selectedClient, owner: 'Partner', life: 0, tpd: 0, trauma: 0, incomeProtection: "", isDefault: 1 }


            ];
          }
          else {
            this.needsAnalysis.forEach((x: any) => {
              x.life = Number(x.life);
              x.tpd = Number(x.tpd);
              x.trauma = Number(x.trauma);

              if (x.description == 'Total Cover Recommended' || x.description == 'Amount of gross income to cover' || x.description == 'Waiting period' || x.description == 'Benefit period') {
                x.incomeProtection = x.incomeProtection.toString();
              }
              else {
                x.incomeProtection = Number(x.incomeProtection);

              }
            });
            this.partnerNeedsNanlysis = this.needsAnalysis;


          }

          this.calculatePartnerTotal();

        }, err => {
          if (err.status == 404)
            this.router.navigate(['/home']);
          });

      
      }
    }

  addFieldValue(cfDetails: any, type: number) {

    if (cfDetails.clientId == 0) {
      cfDetails.clientId = this.selectedClient;
    }

    if (type == 0) {
      this.clientNeedsAnalysis.push(cfDetails);
      this.clientNeeds = {
        recId: 0,
        description: '',
        clientId: 0,
        owner: 'Client',
        life: "",
        tpd: "",
        trauma: "",
        incomeProtection: "",
        isDefault: 0
      }
      
      this.calculateClientTotal();
    }
    else {
      this.partnerNeedsNanlysis.push(cfDetails);
      this.partnerNeeds = {
        recId: 0,
        description: '',
        clientId: 0,
        owner: 'Partner',
        life: "",
        tpd: "",
        trauma: "",
        incomeProtection: "",
        isDefault: 0
      }

      this.calculatePartnerTotal();
    }

  }

  calculateClientTotal()
  {
    this.totalClientNeeds = {
      life: 0,
      tpd: 0,
      trauma: 0,
      incomeProtection: 0,
    }

    this.clientNeedsAnalysis.forEach((x: any) => {
      if (x.description != "For how long that income needs to be replaced" && x.description != "Total Cover Recommended") {
        this.totalClientNeeds.life += (isNaN(Number(x.life))) ? 0 : Number(x.life);
        this.totalClientNeeds.tpd += (isNaN(Number(x.tpd))) ? 0 : Number(x.tpd);
        this.totalClientNeeds.trauma += (isNaN(Number(x.trauma))) ? 0 : Number(x.trauma);
        this.totalClientNeeds.incomeProtection += (isNaN(Number(x.incomeProtection))) ? 0 : Number(x.incomeProtection);
      }
    });

  }

  calculatePartnerTotal() {
    this.totalPartnerNeeds = {
      life: 0,
      tpd: 0,
      trauma: 0,
      incomeProtection: 0,
    }

    this.partnerNeedsNanlysis.forEach((x: any) => {
      if (x.description != "For how long that income needs to be replaced" && x.description != "Total Cover Recommended") {
        this.totalPartnerNeeds.life += (isNaN(Number(x.life))) ? 0 : Number(x.life);
        this.totalPartnerNeeds.tpd += (isNaN(Number(x.tpd))) ? 0 : Number(x.tpd);
        this.totalPartnerNeeds.trauma += (isNaN(Number(x.trauma))) ? 0 : Number(x.trauma);
        this.totalPartnerNeeds.incomeProtection += (isNaN(Number(x.incomeProtection))) ? 0 : Number(x.incomeProtection);
      }
    });

  }

  deleteFieldValue(item: any, index: any, type: number) {
    var id$ = item.recId;
    if (id$ != 0) {


      this.needsAnalysisService.delete(id$).subscribe((data) => {

        if (type == 0) {
          this.clientNeedsAnalysis.splice(index, 1);
          this.calculateClientTotal();
        }
        else {
          this.partnerNeedsNanlysis.splice(index, 1);
          this.calculatePartnerTotal();
        }
      });
  
    }
    else {
      if (type == 0) {
        this.clientNeedsAnalysis.splice(index, 1);
        this.calculateClientTotal();
      }
      else {
        this.partnerNeedsNanlysis.splice(index, 1);
        this.calculatePartnerTotal();
      }
    }
  }



  SaveNeedsAnalysis() {

    this.clientNeedsAnalysis.forEach((x: any) => {
      x.life = x.life.toString();
      x.tpd = x.tpd.toString();
      x.trauma = x.trauma.toString();
      x.incomeProtection = x.incomeProtection.toString();
    })

    this.partnerNeedsNanlysis.forEach((x: any) => {
      x.life = x.life.toString();
      x.tpd = x.tpd.toString();
      x.trauma = x.trauma.toString();
      x.incomeProtection = x.incomeProtection.toString();
    })

    this.needsAnalysisService.updateNeedsAnalysis(this.clientNeedsAnalysis, this.partnerNeedsNanlysis, this.selectedClient, this.clientDetails.maritalStatus == 'M' ? 1 : 0 ).subscribe((data: any) => {

    });
  }
  
}
