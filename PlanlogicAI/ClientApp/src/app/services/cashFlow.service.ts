import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map'; 
import { CashFlow } from '../models/CashFlow';

@Injectable()
export class CashFlowService {

    private readonly cFEndpoint = '/api/cashFlow';
    constructor(private http: HttpClient) { }


    create(cashFlow: CashFlow, id: number, type: string) {
        return this.http.put(this.cFEndpoint + '/' + id + '/' + type, cashFlow);
    }

    getCashFlows(id: number, type: string) {
        return this.http.get<CashFlow[]>(this.cFEndpoint + '/' + id + '/' + type);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
