export interface Pension {

    pensionId: number,
    clientId: number,
    type: string,
    pensionType: string,
    name: string,
    owner: string,
    value: number,
    taxFreeComponent: number,
    taxableComponent: number,
    growth: number,
    income: number,
    franked: number,
    pensionRebootFromType: string,
    pensionRebootFromDate: number,
    endDateType: string,
    endDate: number,
    totalBalance: number,
    productFees: number
  
}

export interface PensionDetails {

    pensionId: number,
    clientId: number,
    type: string,
    amount: number,
    fromDateType: string,
    fromDate: number,
    toDateType: string,
    toDate: number
  
}