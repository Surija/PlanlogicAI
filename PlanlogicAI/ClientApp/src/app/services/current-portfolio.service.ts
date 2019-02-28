import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CurrentPortfolioService {

    private readonly cFEndpoint = '/api/currentPortfolio';
    constructor(private http: HttpClient) { }

    createProduct(product: any, clientId: number, owner: string) {

        return this.http.put(this.cFEndpoint + '/' + clientId + '/' + owner, product);
    }

    getProducts() {
        return this.http.get(this.cFEndpoint);
    }

    getCurrentProducts(id: number, owner: string) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + owner);
    }

    getCurrentFunds(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }


}
