import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class PensionAssumptionsService {

    private readonly cFEndpoint = '/api/pensionAssumptions';
    constructor(private http: HttpClient) { }

    getMinimumPensionDrawdown() {
        return this.http.get(this.cFEndpoint);
    }

    getPreservationAge() {
        return this.http.get(this.cFEndpoint + '/' + 0);
    }

  
}
