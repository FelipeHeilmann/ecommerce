using Domain.Transactions.Entity;

namespace Domain.Transactions.VO;

public class RefundedStatus : TransactionStatus
{
    public override string Value { get; protected set; }

    public RefundedStatus(Transaction transaction) : base(transaction)
    {
        Value = "refounded";
    }

    public override void Approve()
    {
        throw new Exception("Invalid status");
    }

    public override void Cancel()
    {
        throw new Exception("Invalid status");
    }

    public override void InRefound()
    {
        throw new Exception("Invalid status");
    }

    public override void Refound()
    {
        throw new Exception("Invalid status");
    }

    public override void Reject()
    {
        throw new Exception("Invalid status");
    }
}