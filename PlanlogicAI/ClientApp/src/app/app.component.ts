import { Component } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart, NavigationEnd, Event, NavigationError } from '@angular/router';

@Component({
  selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
    showLoadingIndicator = true;
    constructor(private route: ActivatedRoute,
        private router: Router) {
        this.router.events.subscribe((routerEvent: Event) => {
            if (routerEvent instanceof NavigationStart) {
                this.showLoadingIndicator = true;
            }

            if (routerEvent instanceof NavigationEnd || routerEvent instanceof NavigationError) {
                this.showLoadingIndicator = false;
            }
        });
    }

}
