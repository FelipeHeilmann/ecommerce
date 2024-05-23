using Application.Abstractions.Query;
using Domain.Orders.Event;
using MediatR;

namespace Application.Orders.EventsHandler;

public class OrderCanceledEventHandler : INotificationHandler<OrderCanceledEvent>
{
    private readonly IOrderQueryContext _context;

    public OrderCanceledEventHandler(IOrderQueryContext context)
    {
        _context = context;
    }

    public async Task Handle(OrderCanceledEvent notification, CancellationToken cancellationToken)
    {
        var order = await _context.GetById(notification.OrderId);

        order.Status = "canceled";

        await _context.Update(order);   
    }
}
