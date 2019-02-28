export interface Client {
    clientId: number,
    familyName: string
}
export interface BasicDetails {
    clientId: number,
    familyName: string,
    clientName: string,
    clientDob: Date
    clientEmpStatus: string,
    clientRetirementYear: number,
    clientRiskProfile: string,
    clientPrivateHealthInsurance: string,
    maritalStatus: string,
    partnerName: string,
    partnerDob: Date,
    partnerEmpStatus: string,
    partnerRetirementYear: number,
    partnerRiskProfile: string,
    jointRiskProfile: string,
    partnerPrivateHealthInsurance: string,
    startDate: number,
    period: number,
    noOfDependents: number,
}