import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';

@Injectable()
export class ProductSwitchingService {

    private readonly cFEndpoint = '/api/productSwitching';
    private readonly docEndpoint = '/api/documentGenerator';
    private readonly rpEndpoint = '/api/riskProfile';

    constructor(private http: HttpClient) { }


    generateWord(reportDetails: any) {
                var details : any[] = [];
        var fileName = "";
        reportDetails.forEach(function (obj: any) {
            fileName = obj.clientDetails.familyName.trim() + "," + obj.clientDetails.clientName.trim() + ".docx";
                    var Indata = { 'clientRiskProfile': obj.clientRiskProfile, 'partnerRiskProfile': obj.partnerRiskProfile, 'jointRiskProfile': obj.jointRiskProfile, 'clientWeights': obj.clientWeights, 'partnerWeights': obj.partnerWeights, 'jointWeights': obj.jointWeights, 'clientDetails': obj.clientDetails, 'income': obj.cashFlowIncome, 'expenses': obj.cashFlowExpenses, 'lifestyleAssets': obj.lifestyleAssets, 'currentAssests': obj.currentAssests, 'liabilities': obj.liabilities, 'proposedAssets': obj.proposedAssets, 'alternativeAssets': obj.alternativeAssets, 'currentOriginalAssests': obj.currentOriginalAssests }
                    details.push(Indata);
                });
        return this.http.post(this.docEndpoint, details, { responseType: 'blob' });

               // return this.http.post(this.docEndpoint, details);
    }

    getAllProposedProducts() {
        return this.http.get(this.cFEndpoint);
    }

    getClientCurrentProducts(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id);
    }

    getClientProposedProducts(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 1);
    }

  
    retainProduct(product: any, recId: number) {
       
        return this.http.put(this.cFEndpoint + '/' + recId, product);
    }

    getUnderlyingFunds(id: number) {
        return this.http.get(this.cFEndpoint + '/' + id + '/' + 1 + '/' + 1);
    }

    getUnderlyingFundsProposed(id: number, productId) {
    return this.http.get(this.cFEndpoint + '/' + id + '/' + productId + '/' + 1 + '/' + 1);
    }

    rebalanceProduct(product: any, recId: number, currentRecId: number) {

        return this.http.put(this.cFEndpoint + '/' + recId + '/' + currentRecId, product);
    }

    createFund(fund: any, recId: number, currentRecId: number, percentage: number) {

        return this.http.put(this.cFEndpoint + '/' + recId + '/' + currentRecId + '/' + percentage, fund);
    }

    delete(id: number) {
        return this.http.delete(this.cFEndpoint + '/' + id);
    }

    getRiskProfiles( ) {
        return this.http.get(this.rpEndpoint);
    }

}
