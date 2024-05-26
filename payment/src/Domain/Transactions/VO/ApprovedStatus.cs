using Domain.Transactions.Entity;

namespace Domain.Transactions.VO;

public class ApprovedStatus : TransactionStatus
{
    public override string Value { get; protected set; }

    public ApprovedStatus(Transaction transaction) : base(transaction) 
    {
        Value = "approved";
    }

    public override void Approve()
    {
        throw new Exception("Invalid status");
    }

    public override void InRefound()
    {
        Transaction._status = new RefundingStatus(Transaction);
    }

    public override void Reject()
    {
        throw new Exception("Invalid status");
    }

    public override void Refound()
    {
        throw new Exception("Invalid status");
    }

    public override void Cancel()
    {
        throw new Exception("Invalid status");
    }
}
