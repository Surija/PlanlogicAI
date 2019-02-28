import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/map'; 
//import { FinancialAsset, FinancialAssetDetails } from '../models/FinancialAsset';

@Injectable()
export class FinancialAssetService {

    private readonly fAEndpoint = '/api/financialAsset';
    constructor(private http: HttpClient) { }


    getFinancialAssets(id: number) {
        return this.http.get(this.fAEndpoint + '/' + id);
       
    }


}
