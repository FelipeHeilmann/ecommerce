export type TransactionRequest = {
    closed: boolean
    code: string
    items: CreateOrderItemModel[]
    customer: CreateCustomerModel,
    payments: OrderPaymentType
}

type CreateOrderItemModel = {
    amount: number
    description: string
    quantity: number
    code: string
}

type CreateCustomerModel = {
    name: string
    email: string
    document: string
    type: string
    address: CreateAddressModel
    phone: CreateCustomerPhoneModel
}

type CreateAddressModel = {
    street: string
    number: string
    zipCode: string
    neighborhood: string
    city: string
    state: string
    country: string
    complement: string | null,
    line1: string | null,
    line2: string | null
}

type CreateCustomerPhoneModel = {
    countryCode: string
    areaCode: string
    number: string
}


type OrderPaymentType = {
    creditCard: CreditCard | null
    boleto: Boleto | null
    pix: Pix | null
    paymentType: string
}[]

type CreditCard = {
    statementDescriptor: string,
    pperationType: string,
    intallments: number,
    cardToken: string
}

type Boleto = {
    bank: string,
    dueAt: string,
    instructions: string
}

type Pix = {
    expiresIn: number
}
