import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class NewProductService {

    private readonly cFEndpoint = '/api/newProduct';
    constructor(private http: HttpClient) { }

    //createNewProduct(newProduct: any, replacement: any[], id: number) {
    //     var Indata = { 'proposedProduct': newProduct, 'currentProducts': replacement };
    //     return this.http.put(this.cFEndpoint + '/' + id, Indata);
    //}

    createNewProduct(product: any, clientId: number) {

        return this.http.put(this.cFEndpoint + '/' + clientId, product);
    }

    rollFundsIn(newProduct: any, replacement: any[], id: number, currentId: number) {
        var Indata = { 'proposedProduct': newProduct, 'currentProducts': replacement };
        return this.http.put(this.cFEndpoint + '/' + id + '/' + currentId, Indata);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
