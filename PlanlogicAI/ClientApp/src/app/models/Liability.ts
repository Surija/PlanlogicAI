export interface Liability {

    liabilityId: number,
    clientId: number,
    type:string,
    name: string,
    deductibility : number,
    owner: string,
    principal: number,
    repaymentType : string,
    repayment : number,
    interestRate: number,
    term: number,
    commenceOnDateType: string,
    commenceOnDate: number,
    repaymentDateType: string,
    repaymentDate: number,
    associatedAsset: string,
    creditLimit: number
}

export interface LiabilityDetails {

    liabilityId: number,
    clientId: number,
    amount: number,
    fromDateType: string,
    fromDate: number,
    toDateType: string,
    toDate: number
  
}