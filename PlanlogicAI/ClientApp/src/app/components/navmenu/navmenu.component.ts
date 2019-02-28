import { Component } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart, NavigationEnd, Event, NavigationError } from '@angular/router';
import { Client, BasicDetails } from './../../models/Client';
import { ClientService } from './../../services/client.service';
import * as $ from 'jquery';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    clientDetails: any;
   // clientDetails: BasicDetails;
    display = 'none';
    showLoadingIndicator = true;
    constructor(private route: ActivatedRoute,
        private router: Router, private clientService: ClientService) {
        this.router.events.subscribe((routerEvent: Event) => {
            if (routerEvent instanceof NavigationStart) {
                this.showLoadingIndicator = true;
            }

            if (routerEvent instanceof NavigationEnd || routerEvent instanceof NavigationError) {
                this.showLoadingIndicator = false;
            }
        });
    }


    //TODO : incorporate  start date and period - Default set to current year (2018) and (10) for period
    ngOnInit() {
        var client = JSON.parse(localStorage.getItem('ClientDetails') || '{}');
        if (client != null) {
            this.clientDetails = client;
        }

        $(document).ready(function () {
            $('.StrategyOptimizerchild').hide();
            $('.ProposedProjectionchild').hide();
            $('.Projectionchild').hide();
           
        });
    }

    ProjectionDate(val: string) {
         var client = JSON.parse(localStorage.getItem('ClientDetails') || '{}');
         if (client != null) {
            this.clientDetails = client;
            if (val == "optimizer") {
                if (this.clientDetails.startDate != 0 && this.clientDetails.period != 0) {
                    this.router.navigate(['/counter/current-projections/strategyOptimizer']);
                   

                }
                else {
                    this.onSubmit();
                   // this.display = 'block';
                }
            }
            else if (val == "bDetail") {
                this.router.navigate(['/counter/basic-details']);
            }

            else if (val == "projection") {
                if (this.clientDetails.startDate != 0 && this.clientDetails.period != 0) {
                    $('.Projectionchild').slideToggle();
                }
                else {
                    this.onSubmit();
                   // this.display = 'block';
                }
            }
            else if (val == "proposedprojection") {
                if (this.clientDetails.startDate != 0 && this.clientDetails.period != 0) {
                    $('.ProposedProjectionchild').slideToggle();
                }
                else {
                    this.onSubmit();
                  //  this.display = 'block';
                }
            }
            else if (val == "strategyOptimizer") {
                if (this.clientDetails.startDate != 0 && this.clientDetails.period != 0) {
                    $('.StrategyOptimizerchild').slideToggle();
                }
                else {
                    this.onSubmit();
                  //  this.display = 'block';
                }
            }
            else if (val == "productComparison") {
                if (this.clientDetails.startDate != 0 && this.clientDetails.period != 0) {
                    $('.ProductComparisonchild').slideToggle();
                }
                else {
                    this.onSubmit();
                  //  this.display = 'block';
                }
            }
         
        }

    }

    onSubmit() {
        this.clientDetails.startDate = 2018;
        this.clientDetails.period = 10;

        this.clientService.createBasicDetails(this.clientDetails, {}, this.clientDetails.clientId).subscribe((data) => {
            localStorage.removeItem('ClientDetails');
            localStorage.setItem('ClientDetails', JSON.stringify(data));
            this.display = 'none';
        });
    }
  
    onCloseHandled() {
        this.display = 'none';
    }


          
        }


