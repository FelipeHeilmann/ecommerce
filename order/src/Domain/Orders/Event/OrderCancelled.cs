using Domain.Abstractions;
using MediatR;

namespace Domain.Orders.Event;

public class OrderCancelled : INotification, IDomainEvent
{
    public string EventName => "OrderCancelled";

    public object Data { get; set; }

    public OrderCancelled(Guid orderId, Guid customerId)
    {
        Data = new OrderCancelledData(orderId, customerId);
    }
}

public record OrderCancelledData(Guid OrderId, Guid CustomerId);
