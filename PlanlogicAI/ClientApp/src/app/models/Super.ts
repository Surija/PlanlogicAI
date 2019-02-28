export interface Super {

    superId: number,
    clientId: number,
    type:string,
    name: string,
    owner: string,
    value: number,
    taxFreeComponent: number,
    taxableComponent: number,
    growth: number,
    income: number,
    franked: number,
    insuranceCost: number,
    startDateType: string,
    startDate: number,
    endDateType: string,
    endDate: number,
    superSalary: number,
    increaseToLimit: string,
    sgrate: string,
    productFees: number
  
}

export interface SuperDetails {

    superId: number,
    clientId: number,
    type: string,
    amount: number,
    increaseToLimit: string,
    fromDateType: string,
    fromDate: number,
    toDateType: string,
    toDate: number
  
}