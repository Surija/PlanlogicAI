import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-current-position',
    templateUrl: './current-position.component.html',
    styleUrls: ['./current-position.component.css']
})
export class CurrentPositionComponent implements OnInit {

    constructor(private route: ActivatedRoute,
        private router: Router) { }

  ngOnInit() {
  }

    onPrevious() {
        this.router.navigate(['counter/current-projections/basic-details']);
    }

    onNext() {
        this.router.navigate(['counter/current-projections/cashflow']);
    }
}
