import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CommonService {

    private readonly cFEndpoint = '/api/common';
    constructor(private http: HttpClient) { }

    getGeneralAssumptions() {
        return this.http.get(this.cFEndpoint);
    }

    getMarginalTaxRates() {
        return this.http.get(this.cFEndpoint + '/' + 0);
    }

    getAssetTypesAssumptions() {
        return this.http.get(this.cFEndpoint + '/' + 0 + '/' + 0);
    }

    getPensionDrawdownAssumptions() {
        return this.http.get(this.cFEndpoint + '/' + 0 + '/' + 0 + '/' + 0);
    }
}
