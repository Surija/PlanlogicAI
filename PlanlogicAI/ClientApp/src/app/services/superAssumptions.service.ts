import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class SuperAssumptionsService {

    private readonly cFEndpoint = '/api/superAssumptions';
    constructor(private http: HttpClient) { }

    getSuperAssumptions() {
        return this.http.get(this.cFEndpoint);
    }

    getSgcrates() {
        return this.http.get(this.cFEndpoint + '/' + 0);
    }

  
}
