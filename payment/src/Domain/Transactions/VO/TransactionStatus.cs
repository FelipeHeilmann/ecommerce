using Domain.Transactions.Entity;

namespace Domain.Transactions.VO;

public abstract class TransactionStatus
{
    public abstract string Value { get; protected set; }
    public Transaction Transaction { get; set; }
    public TransactionStatus(Transaction transaction) => Transaction = transaction;
    public abstract void Approve();
    public abstract void Reject();
    public abstract void InRefound();
    public abstract void Refound();
    public abstract void Cancel();
}

public class TransactionStatusFactory
{
    public static TransactionStatus Create(Transaction transaction, string status)
    {
        if(status == "waiting_payment") return new WaitingPaymentStatus(transaction);
        if(status == "approved") return new ApprovedStatus(transaction);
        if(status == "rejected") return new RejectedStatus(transaction);
        if(status == "refounding") return new RefundingStatus(transaction);
        if(status == "refounded") return new RefundedStatus(transaction);
        throw new ArgumentException("Invalid status");
    }
}
