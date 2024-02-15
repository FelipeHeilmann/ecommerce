namespace Domain.Refunds;

public class Refund
{
    public Guid Id { get; private set; }
    public Guid TransactionId { get; private set; }
    public double Amount { get; private set; }
    public RefundStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PayedAt { get; private set; }

    public Refund(Guid id, Guid transactionId, double amount, RefundStatus status, DateTime createdAt, DateTime? payedAt)
    {
        Id = id;
        TransactionId = transactionId;
        Amount = amount;
        Status = status;
        CreatedAt = createdAt;
        PayedAt = payedAt;
    }

    public static Refund Create(Guid transactionId, double amount) 
    { 
        return new Refund(Guid.NewGuid(),transactionId, amount, RefundStatus.WaitingRefund, DateTime.UtcNow, null);
    }

    public void Pay()
    {
        Status = RefundStatus.RefundPayed;
        PayedAt = DateTime.UtcNow;
    }
}
