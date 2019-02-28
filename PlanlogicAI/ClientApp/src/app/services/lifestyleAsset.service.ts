import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map'; 
import { LifestyleAsset } from '../models/LifestyleAsset';

@Injectable()
export class LifestyleAssetService {

    private readonly cFEndpoint = '/api/lifestyleAsset';
    constructor(private http: HttpClient) { }


    create(lAsset: LifestyleAsset, id: number) {
        return this.http.put(this.cFEndpoint + '/' + id, lAsset);
    }

    getLifestyleAssets(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
