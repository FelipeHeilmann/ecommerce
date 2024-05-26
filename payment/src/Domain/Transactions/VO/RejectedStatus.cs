using Domain.Transactions.Entity;

namespace Domain.Transactions.VO;

public class RejectedStatus : TransactionStatus
{
    public override string Value { get; protected set; }

    public RejectedStatus(Transaction transaction): base(transaction)
    {
        Value = "rejected";
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
