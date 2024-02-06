namespace Domain.Orders;

public enum OrderStatus
{
    Created = 1,
    WaitingPayment = 2,
    PaymentRefused = 3,
    InvoiceEmmited = 4,
    SeparateForShipping = 5,
    Shipped = 6,
    Canceled = 7,
}
