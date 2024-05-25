using Application.Abstractions.Query;
using Domain.Orders.Event;
using MediatR;

namespace Application.Orders.EventsHandler;

public class UpdateOrderOnProjectionDataBase : INotificationHandler<OrderCancelled>
{
    private readonly IOrderQueryContext _context;

    public UpdateOrderOnProjectionDataBase(IOrderQueryContext context)
    {
        _context = context;
    }

    public async Task Handle(OrderCancelled notification, CancellationToken cancellationToken)
    {
        var data = (OrderCancelledData)notification.Data;

        var order = await _context.GetById(data.OrderId);

        order.Status = "canceled";

        await _context.Update(order);   
    }
}
