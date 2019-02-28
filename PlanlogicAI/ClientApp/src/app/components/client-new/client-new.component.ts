 import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ClientService } from '../../services/client.service';
import { ToastyService } from 'ng2-toasty';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';
import { Client } from './../../models/Client';
import { Validators, FormControl } from '@angular/forms';
import * as $ from 'jquery';
//import 'datatables.net'

@Component({
  selector: 'app-client-new',
  templateUrl: './client-new.component.html',
  styleUrls: ['./client-new.component.css']
})
export class ClientNewComponent implements OnInit {
    client: Client = {
        clientId: 0,
        familyName: ''
    };
    newClient = 'none';
    existingClient = 'none';
    main = 'block';
    clients: Client[] = [];

    constructor(private route: ActivatedRoute,
    private router: Router,
        private clientService: ClientService) {

    }
  


    ngOnInit() {

        this.clientService.getClients().subscribe((clients: any) => {
                this.clients = clients;
                //$('#tblClients').DataTable();
            }
        );

        //$(document).ready(function () {
           
        //});
  }
    //ngAfterViewInit() {
    //    setTimeout(() => {
    //        $('#tblClients').DataTable(
    //        );
    //    }, 2000);
    //}

    openNewClient() {
        this.newClient = 'block';
        this.existingClient = 'none';
        this.main = 'none';
    }
    openExistingClient() {
        this.existingClient = 'block';
        this.main = 'none';
        this.newClient = 'none';
    }
    onCloseHandled() {
        this.newClient = 'none';
        this.existingClient = 'none';
        this.main = 'block';
    }
  submitted = false;

    routeExistingClient(clientId: number) {
        localStorage.clear();
        localStorage.setItem('selectedClient', JSON.stringify(clientId));

        this.clientService.getBasicDetails(clientId).subscribe(
            basicDetails => {
                if (basicDetails != null) {
                    localStorage.setItem('ClientDetails', JSON.stringify(basicDetails));
                    this.router.navigate(['/counter/current-position']);
                }
                else {
                    this.router.navigate(['/counter']);
                }
            }, (err) => {
                this.router.navigate(['/counter']);
            }
        );
      
      
       
    }



    onSubmit(){
        this.clientService.create(this.client).subscribe((data:any) => {
          
            localStorage.clear();
            localStorage.setItem('selectedClient', JSON.stringify(data.clientId));
            this.router.navigate(['/counter']);
        });
    }

} 
