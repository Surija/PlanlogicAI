 import { Injectable } from '@angular/core';
import { Headers, URLSearchParams, Request, RequestOptions } from '@angular/http';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map'; 


@Injectable()
export class ProductService {
     

    private readonly cFEndpoint = '/api/product';
    constructor(private http: HttpClient) { }

     //create(excelData: any, id: number) {
    //    return this.http.put(this.cFEndpoint + '/' + 0, excelData).map(res => res.json());
    //}

    create(platform: any) {
        return this.http.post(this.cFEndpoint, platform);
    }

    createProducts(product: any, platformId: number) {
        return this.http.put(this.cFEndpoint + '/' + platformId, product);
    }
   

    getPlatforms() {
        return this.http.get(this.cFEndpoint);
    }

    getProductDetails(platformId: number) {
        return this.http.get(this.cFEndpoint + '/' + platformId);
    }

    delete(id: number, platformId: number) {
        return this.http.delete(this.cFEndpoint + '/' + id + '/' + platformId);
    }
}
