import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class InsuranceSwitchingService {

    private readonly cFEndpoint = '/api/insuranceSwitching';
  private readonly docEndpoint = '/api/insuranceDocumentGenerator';
    
    constructor(private http: HttpClient) { }


  generateWord(obj: any) {
    var reportDetails = { 'needsAnalysis': obj.needsAnalysis, 'clientDetails': obj.clientDetails, 'currentInsurance': obj.currentInsurance, 'proposedInsurance': obj.proposedInsurance}

    return this.http.post(this.docEndpoint, reportDetails, { responseType: 'blob' });

    // return this.http.post(this.docEndpoint, details);
  }


    getAllProposedProducts() {
        return this.http.get(this.cFEndpoint);
    }

    getCurrentInsurance(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    getProposedInsurance(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 1);
    }
  
  
    addCurrentInsurance(currentInsurance: any, recId: number) {
        return this.http.put(this.cFEndpoint + '/' + recId, currentInsurance);
    }

    addProposedInsurance(proposedInsurance: any, recId: number) {
        return this.http.put(this.cFEndpoint + '/' + recId + '/' + 1, proposedInsurance);
    }

  retainProduct(product: any, recId: number, clientId: number) {
        return this.http.put(this.cFEndpoint + '/' + recId + '/' + clientId + '/' + 1, product);
    }

    deleteCurrent(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }
    deleteProposed(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id + '/' + 1);
    }
 
}
