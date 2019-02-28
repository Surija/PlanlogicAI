import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { Property } from '../models/Property';

@Injectable()
export class PropertyService {

    private readonly cFEndpoint = '/api/property';
    constructor(private http: HttpClient) { }


    create(property: Property, id: number) {
        return this.http.put(this.cFEndpoint + '/' + id, property);
    }

    getProperties(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
}
