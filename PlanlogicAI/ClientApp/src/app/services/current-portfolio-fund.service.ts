import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CurrentPortfolioFundService {

    private readonly cFEndpoint = '/api/currentPortfolioFund';
    constructor(private http: HttpClient) { }

    createFund(fund: any, recId: number, percentage : number) {

        return this.http.put(this.cFEndpoint + '/' + recId + '/' + percentage, fund);
    }
    getAllProductReplacement() {
        return this.http.get(this.cFEndpoint);
    }

    getProductReplacement(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }

}
