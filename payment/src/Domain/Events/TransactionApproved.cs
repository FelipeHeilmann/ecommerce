using Domain.Abstractions;
using MediatR;

namespace Domain.Events;

public class TransactionApproved : INotification, IDomainEvent
{
    public string EventName => "TransactionApproved";

    public object Data { get; set; }

    public TransactionApproved(Guid id , Guid orderId, DateTime approvedAt)
    {

        Data = new TransactionApprovedData(id, orderId, approvedAt); 
    }
}

public record TransactionApprovedData(Guid Id, Guid OrderId, DateTime ApprovedAt);
