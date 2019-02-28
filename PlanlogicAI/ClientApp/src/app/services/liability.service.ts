import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Liability, LiabilityDetails } from '../models/Liability';

@Injectable()
export class LiabilityService {

    private readonly cFEndpoint = '/api/liability';
    constructor(private http: HttpClient) { }


    create(liability: Liability, details: LiabilityDetails[], id: number) {

        var Indata = { 'liability': liability, 'liabilitydd': details };
        return this.http.put(this.cFEndpoint + '/' + id, Indata);
    }

    getLiabilities(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    getLiabilityDetails(id: number, liabilityId: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + liabilityId);
    }

    getAllLiabilityDetails(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 0 + '/' + 0);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
