﻿using Domain.Abstractions;
using Domain.Events;
using Domain.Transactions.VO;

namespace Domain.Transactions.Entity;

public class Transaction : Observable
{
    public TransactionStatus _status;
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public double Amount { get; private set; }
    public Guid PaymentServiceId { get; private set; }
    public string PaymentType { get; private set; }
    public string Status => _status.Value;
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public DateTime? RejectedAt { get; private set; }

    private Transaction(Guid id, Guid orderId, Guid customerId, Guid paymentServiceId, double amount, string paymentType, string status, DateTime createdAt, DateTime? approvedAt, DateTime? rejecteddAt)
    {
        Id = id;
        OrderId = orderId;
        PaymentType = paymentType;
        _status = TransactionStatusFactory.Create(this, status);
        CustomerId = customerId;
        CreatedAt = createdAt;
        ApprovedAt = approvedAt;
        RejectedAt = rejecteddAt;
        PaymentServiceId = paymentServiceId;
        Amount = amount;
    }

    public static Transaction Create(Guid orderId, Guid customerId, Guid paymentServiceId, double amount, string paymentType)
    {
        return new Transaction(Guid.NewGuid(), orderId, customerId, paymentServiceId, amount, paymentType, "waiting_payment", DateTime.UtcNow, null, null);
    }

    public static Transaction Restore(Guid id, Guid orderId ,Guid customerId, Guid paymentServiceId, double amount, string paymentType, string status, DateTime createdAt, DateTime? approvedAt, DateTime? rejectAt)
    {
        return new Transaction(id, orderId, customerId, paymentServiceId, amount, paymentType, status, createdAt, approvedAt, rejectAt);
    }

    public void Approve()
    {
        _status.Approve();
        ApprovedAt = DateTime.UtcNow;
        Notify(new TransactionStatusChanged(Id, OrderId, ApprovedAt.Value, Status));
    }

    public void Reject()
    {   
        _status.Reject();
        RejectedAt = DateTime.UtcNow;
        Notify(new TransactionStatusChanged(Id, OrderId, RejectedAt.Value, Status));
    }

    public void Refund()
    {
        _status.InRefound();
    }

    public void ApplyRefund()
    {
        _status.Refound();
    }
}
