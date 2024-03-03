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
    CreditCard: CreditCard | null
    Boleto: Boleto | null
    Pix: Pix | null
    PaymentType: string
}

type CreditCard = {
    StatementDescriptor: string,
    OperationType: string,
    Intallments: number,
    CardToken: string
}

type Boleto = {
    Bank: string,
    DueAt: string,
    Instructions: string
}

type Pix = {
    expiresIn: number
}
