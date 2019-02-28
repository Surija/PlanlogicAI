import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/map'; 
import { Client } from '../models/Client';

@Injectable()
export class ClientService {

    private readonly clientsEndpoint = '/api/clients';
    constructor(private http: HttpClient) { }


    create(client: any) {
        return this.http.post(this.clientsEndpoint, client);
    }


    getClient(id: number) {
        return this.http.get(this.clientsEndpoint + '/' + id);
                  
    }

    getBasicDetails(id: number) {
        return this.http.get(this.clientsEndpoint + '/' + id + '/' + '1');
            
    }

     getClients() {
         return this.http.get(this.clientsEndpoint);
       
}

    createBasicDetails(basicDetails: any, client: any, id: number) {
        var Indata = { 'basicDetails': basicDetails, 'client': client };
        return this.http.put(this.clientsEndpoint + '/' + id, Indata);
            
}

    delete(id: number) {
        return this.http.delete(this.clientsEndpoint + '/' + id);
           
}

}
