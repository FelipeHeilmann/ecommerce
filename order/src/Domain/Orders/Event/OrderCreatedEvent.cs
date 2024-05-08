using MediatR;

namespace Domain.Orders.Events;


public class OrderCreatedEvent : INotification
{
    public Guid OrderId { get; set; }

    public OrderCreatedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}

