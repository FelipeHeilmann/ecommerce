using MediatR;

namespace Domain.Orders;


public class OrderCreatedEvent : INotification
{
    public OrderCreatedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }

}

