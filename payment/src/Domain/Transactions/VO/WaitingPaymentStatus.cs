using Domain.Transactions.Entity;

namespace Domain.Transactions.VO;

public class WaitingPaymentStatus : TransactionStatus
{
    public override string Value { get; protected set; }

    public WaitingPaymentStatus(Transaction transaction): base(transaction)
    {
        Value = "waiting_payment";
    }

    public override void Approve()
    {
        Transaction._status = new ApprovedStatus(Transaction);
    }

    public override void Reject()
    {
        Transaction._status = new RejectedStatus(Transaction);
    }

    public override void InRefound()
    {
        throw new Exception("Invalid status");
    }

    public override void Refound()
    {
        throw new Exception("Invalid status");
    }

    public override void Cancel()
    {
        Transaction._status = new CanceledStatus(Transaction);
    }
}
