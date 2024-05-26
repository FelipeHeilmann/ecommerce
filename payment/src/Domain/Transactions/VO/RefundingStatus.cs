using Domain.Transactions.Entity;

namespace Domain.Transactions.VO;

public class RefundingStatus : TransactionStatus
{
    public override string Value { get; protected set; }

    public RefundingStatus(Transaction transaction): base(transaction) 
    {
        Value = "refounding";
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
        Transaction._status = new RefundedStatus(Transaction);
    }

    public override void Reject()
    {
        throw new Exception("Invalid status");
    }
}
