import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'; 
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../../services/client.service';

@Component({
    selector: 'counter',
    templateUrl: './counter.component.html'
})
export class CounterComponent  {
    id: number = 0;
    constructor(private route: ActivatedRoute,
        private router: Router,
        private clientService: ClientService) {


        route.params.subscribe(c => {
            this.id = +c['id'];
                 });
    }

    ngOnInit() {
        //this.clientService.getClient(this.id).subscribe(
        //    clients => {
        //        localStorage.clear();
        //        localStorage.setItem('selectedClient', JSON.stringify(clients));
        //        //console.log('selectedClient' + localStorage.getItem('selectedClient'));
        //    }
        //);
     
    }
}
