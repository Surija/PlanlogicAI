import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Super, SuperDetails } from '../models/Super';

@Injectable()
export class SuperService {

    private readonly cFEndpoint = '/api/super';
    constructor(private http: HttpClient) { }


    create(superAsset: Super, details: SuperDetails[], id: number) {

        var Indata = { 'super': superAsset, 'superDetails': details };
        return this.http.put(this.cFEndpoint + '/' + id, Indata);
    }

    getSupers(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    getSuperDetails(id: number, superId: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + superId);
    }

    getAllSuperDetails(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 0 + '/' + 0);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
