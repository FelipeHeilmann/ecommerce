namespace Domain.Transactions;

public enum TransactionStatus
{
    WaitingPayment = 1,
    Approved = 2,
    Reject = 3,
    AwaitingRefund = 4,
    Refunded = 5,
}
