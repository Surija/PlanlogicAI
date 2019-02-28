import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CentrelinkAssumptionsService {

    private readonly cFEndpoint = '/api/centrelinkAssumptions';
    constructor(private http: HttpClient) { }

    getQualifyingAge() {
        return this.http.get(this.cFEndpoint + '/' + 0);
    }

  
}
