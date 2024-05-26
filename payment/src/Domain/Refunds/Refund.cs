namespace Domain.Refunds;

public class Refund
{
    public Guid Id { get; private set; }
    public Guid TransactionId { get; private set; }
    public double Amount { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PayedAt { get; private set; }

    public Refund(Guid id, Guid transactionId, double amount, string status, DateTime createdAt, DateTime? payedAt)
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
        return new Refund(Guid.NewGuid(),transactionId, amount, "in_proggress", DateTime.UtcNow, null);
    }

    public void Pay()
    {
        Status = "payed";
        PayedAt = DateTime.UtcNow;
    }
}
