import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Investment, InvestmentDetails } from '../models/Investment';

@Injectable()
export class InvestmentService {

    private readonly cFEndpoint = '/api/investment';
    constructor(private http: HttpClient) { }


    create(investment: Investment, details: InvestmentDetails[], id: number) {

        var Indata = { 'investmentDetails': investment, 'cw': details };
        return this.http.put(this.cFEndpoint + '/' + id, Indata);
    }

    getInvestments(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    getInvestmentDetails(id: number, investmentId: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + investmentId);
    }

    getAllInvestmentDetails(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 0 + '/' + 0);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
