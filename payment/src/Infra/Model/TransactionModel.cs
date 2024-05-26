using Domain.Transactions.Entity;

namespace Infra.Model;

public class TransactionModel
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public double Amount { get; set; }
    public Guid PaymentServiceId { get; set; }
    public string PaymentType { get; set; }
    public string Status { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }

    public TransactionModel(Guid id, Guid orderId, double amount, Guid paymentServiceId, string paymentType, string status, Guid customerId, DateTime createdAt, DateTime? approvedAt, DateTime? rejectedAt)
    {
        Id = id;
        OrderId = orderId;
        Amount = amount;
        PaymentServiceId = paymentServiceId;
        PaymentType = paymentType;
        Status = status;
        CustomerId = customerId;
        CreatedAt = createdAt;
        ApprovedAt = approvedAt;
        RejectedAt = rejectedAt;
    }

    private TransactionModel() { }

    public static TransactionModel FromAggregate(Transaction transaction)
    {
        return new TransactionModel(transaction.Id, transaction.OrderId, transaction.Amount, transaction.PaymentServiceId, transaction.PaymentType, transaction.Status, transaction.CustomerId, transaction.CreatedAt, transaction.ApprovedAt, transaction.RejectedAt);
    }

    public Transaction ToAggregate()
    {
        return new Transaction(Id, OrderId, CustomerId, PaymentServiceId, Amount, PaymentType, Status, CreatedAt ,ApprovedAt, RejectedAt);
    }
}
