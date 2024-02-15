namespace Domain.Transactions;

public enum TransactionStatus
{
    WaitingPayment = 1,
    Approved = 2,
    Refused = 3,
    AwaitingRefund = 4,
    Refunded = 5,
}
