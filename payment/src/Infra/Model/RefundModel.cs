using Domain.Refunds;

namespace Infra.Model;

public class RefundModel
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public double Amount { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PayedAt { get; set; }

    public RefundModel(Guid id, Guid transactionId, double amount, string status, DateTime createdAt, DateTime? payedAt)
    {
        Id = id;
        TransactionId = transactionId;
        Amount = amount;
        Status = status;
        CreatedAt = createdAt;
        PayedAt = payedAt;
    }

    private RefundModel() { }

    public static RefundModel FromAggregate(Refund refund)
    {
        return new RefundModel(refund.Id, refund.TransactionId, refund.Amount, refund.Status, refund.CreatedAt, refund.PayedAt);
    }

    public Refund ToAggregate()
    {
        return new Refund(Id, TransactionId, Amount, Status, CreatedAt, PayedAt);
    }
}
