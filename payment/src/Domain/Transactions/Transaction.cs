namespace Domain.Transactions;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public double Amount { get; private set; }
    public Guid PaymentServiceId { get; private set; }
    public PaymentType PaymentType { get; private set; }
    public TransactionStatus Status { get; private set; }   
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public DateTime? RefusedAt { get; private set; } 

    public Transaction(Guid id, Guid orderId, Guid customerId, Guid paymentServiceId, double amount ,PaymentType paymentType, TransactionStatus status, DateTime createdAt, DateTime? approvedAt, DateTime? refusedAt, DateTime? extornedAt)
    {
        Id = id;
        OrderId = orderId;
        PaymentType = paymentType;
        Status = status;
        CustomerId = customerId;
        CreatedAt = createdAt;
        ApprovedAt = approvedAt;
        RefusedAt = refusedAt;
        PaymentServiceId = paymentServiceId;
        Amount = amount;
    }

    public static Transaction Create(Guid orderId, Guid customerId, Guid paymentServiceId, double amount ,string paymentTypeString)
    {
        var paymentType = ConvertToPaymentType(paymentTypeString);

        return new Transaction(Guid.NewGuid(), orderId, customerId, paymentServiceId, amount, paymentType, TransactionStatus.WaitingPayment, DateTime.UtcNow, null, null, null);

    }

    public static PaymentType ConvertToPaymentType(string paymentMethod)
    {
        switch (paymentMethod.ToLower())
        {
            case "credit": return PaymentType.CreditCard;
            case "debit": return PaymentType.DebitCard;
            case "billet": return PaymentType.Billet;
            case "pix": return PaymentType.Pix;
            default: throw new ArgumentException("Invalid payment method", nameof(paymentMethod));
        }
    }

    public void Approve()
    {
        if (Status != TransactionStatus.WaitingPayment) throw new ArgumentException();

        Status = TransactionStatus.Approved;
        ApprovedAt = DateTime.UtcNow;
    }

    public void Refuse()
    {
        if (Status != TransactionStatus.WaitingPayment) throw new ArgumentException();

        Status = TransactionStatus.Refused;
        RefusedAt = DateTime.UtcNow;
    }

    public void Refund()
    {
        Status = TransactionStatus.AwaitingRefund;
    }

    public void ApplyRefund()
    {
        Status = TransactionStatus.Refused;
    }
}
