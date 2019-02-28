import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class InvestmentFundService {

    private readonly cFEndpoint = '/api/fund';
    constructor(private http: HttpClient) { }


    createFunds(fund: any) {
      return this.http.post(this.cFEndpoint, fund);
    }

    createFundDetails(fund: any, productFund : any) {
    var Indata = { 'fund': fund, 'productFund': productFund };
      return this.http.put(this.cFEndpoint + '/' + productFund.productId, Indata);
    }

    createProductFund(details: any[], id: number, type: number) {

        return this.http.put(this.cFEndpoint + '/' + id + '/' + type, details);
    }

    getSelectedFunds(productId: number) {
        return this.http.get(this.cFEndpoint + '/' + productId);
    }

    getAllInvestments(productId: number) {
        return this.http.get(this.cFEndpoint + '/' + productId + '/' + 0);
    }


}
