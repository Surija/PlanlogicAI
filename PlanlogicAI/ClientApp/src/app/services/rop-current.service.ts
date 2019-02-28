import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ROPCurrentService {
    private readonly cFEndpoint = '/api/ropCurrent';
    constructor(private http: HttpClient) { }

    getAllROPCurrent() {
        return this.http.get(this.cFEndpoint);
    }

 
    getClientROPCurrent(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 1);
    }

  getUnderlyingFundsROPCurrent(id: number, productId: number) {
    return this.http.get(this.cFEndpoint + '/' + id + '/' + productId + '/' + 1 + '/' + 1);
    }

    createFund(fund: any, recId: number, currentRecId: number) {

        return this.http.put(this.cFEndpoint + '/' + recId + '/' + currentRecId, fund);
    }

    createNewProduct(newProduct: any, id: number) {
        return this.http.put(this.cFEndpoint + '/' + id, newProduct);
            
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }

    deleteProduct(id: number, client: number) {
        return this.http.delete(this.cFEndpoint + '/' + id + '/' + client);
    }

}
