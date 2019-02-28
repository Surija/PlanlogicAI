import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Pension, PensionDetails } from '../models/Pension';

@Injectable()
export class PensionService {

    private readonly cFEndpoint = '/api/pension';
    constructor(private http: HttpClient) { }


    create(pensionAsset: Pension, details: PensionDetails[], id: number) {

        var Indata = { 'pension': pensionAsset, 'pensionDrawdown': details };
        return this.http.put(this.cFEndpoint + '/' + id, Indata);
    }

    getPensions(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    getPensionDetails(id: number, pensionId: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + pensionId);
    }

    getAllPensionDetails(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 0 + '/' + 0);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
