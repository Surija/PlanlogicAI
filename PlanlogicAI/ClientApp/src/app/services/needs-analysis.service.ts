import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class NeedsAnalysisService {
    private readonly cFEndpoint = '/api/needsAnalysis';
    constructor(private http: HttpClient) { }

    //getAllAlternativeProducts() {
    //    return this.http.get(this.cFEndpoint);
    //}

  getPartnerNeedsAnalysis(id: number) {
    return this.http.get(this.cFEndpoint + '/' + id + '/' + 1);
  }
  getNeedsAnalysis(id: number) {
    return this.http.get(this.cFEndpoint + '/' + id + '/' + 0);
    }

  updateNeedsAnalysis(clientNeedsAnalysis: any, partnerNeedsAnalysis: any, clientId: number, isMarried: number) {
    var NeedsAnalysisData = { 'clientNeedsAnalysis': clientNeedsAnalysis, 'partnerNeedsAnalysis': partnerNeedsAnalysis, 'clientId': clientId, 'isMarried': isMarried}
    return this.http.put(this.cFEndpoint + '/' + clientId, NeedsAnalysisData);
  }

  delete(id: number) {
    return this.http.delete(this.cFEndpoint + '/' + id);
  }
    //getUnderlyingFundsAlternative(id: number) {
    //    return this.http.get(this.cFEndpoint + '/' + id + '/' + 1 + '/' + 1 + '/' + 1);
    //}

    //createFund(fund: any, recId: number, currentRecId: number) {

    //    return this.http.put(this.cFEndpoint + '/' + recId + '/' + currentRecId, fund);
    //}

    //createNewProduct(newProduct: any, id: number) {
    //    return this.http.put(this.cFEndpoint + '/' + id, newProduct);
            
    //}

    //delete(id: number) {
    //    return this.http.delete(this.cFEndpoint + '/' + id);
    //}
    //deleteProduct(id: number, client: number) {
    //    return this.http.delete(this.cFEndpoint + '/' + id + '/' + client);
    //}

}
