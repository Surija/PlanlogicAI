import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/map'; 


@Injectable()
export class FeeService {

    private readonly cFEndpoint = '/api/fee';
    constructor(private http: HttpClient) { }

  createFees(fee: any[], headerId: number, headerType: string) {
    return this.http.put(this.cFEndpoint + '/' + headerId + '/' + headerType, fee);
    }

    

  getFeeDetails(headerId: number, headerType: string) {
    return this.http.get(this.cFEndpoint + '/' + headerId + '/' + headerType);
  }

  getPlatformDetails(clientId: number) {
    return this.http.get(this.cFEndpoint + '/' + clientId);
  }

    delete(id: number, productId: number) {
        return this.http.delete(this.cFEndpoint + '/' + id + '/' + productId);
    }

}
