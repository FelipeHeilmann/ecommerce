using Domain.Abstractions;

namespace Domain.Events;

public class TransactionStatusChanged  : IDomainEvent
{
    public string EventName => "TransactionStatusChanged";

    public object Data { get; set; }

    public TransactionStatusChanged (Guid id , Guid orderId, DateTime approvedOrRejectedAt, string status)
    {

        Data = new TransactionStatusChangedData(id, orderId, approvedOrRejectedAt, status); 
    }
}

public record TransactionStatusChangedData(Guid Id, Guid OrderId, DateTime ApprovedOrRejectedAt, string Status);
