export interface Investment {

    investmentId: number,
    clientId: number,
    type:string,
    name: string,
    owner: string,
    value: number,
    growth: number,
    income: number,
    franked: number,
    costBase: number,
    reinvest: string,
    centrelink: string,
    startDateType: string,
    startDate: number,
    endDateType: string,
    endDate: number,
    productFees: number
  
}

export interface InvestmentDetails {

    investmentId: number,
    clientId: number,
    value: number,
    fromDateType: string,
    fromDate: number,
    toDateType: string,
    toDate: number
  
}