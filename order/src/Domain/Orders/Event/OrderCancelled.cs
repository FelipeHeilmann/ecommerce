using Domain.Abstractions;
using MediatR;

namespace Domain.Orders.Event;

public class OrderCancelled : INotification, IDomainEvent
{
    public string EventName => "OrderCancelled";

    public object Data { get; set; }

    public OrderCancelled(OrderCancelledData data)
    {
        Data = data;
    }
}

public record OrderCancelledData(Guid OrderId);
