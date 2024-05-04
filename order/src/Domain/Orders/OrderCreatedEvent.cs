using MediatR;

namespace Domain.Orders;


public class OrderCreatedEvent : INotification
{
    public Guid OrderId { get; set; }

    public OrderCreatedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}

