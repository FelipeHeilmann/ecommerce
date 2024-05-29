using Application.Abstractions.Queue;
using Domain.Orders.Events;
using MediatR;

namespace Application.Orders.EventsHandler;

public class PublishOrderChekedoutOnQueue : INotificationHandler<OrderCheckedout>
{
    private readonly IQueue _queue;
   

    public PublishOrderChekedoutOnQueue(IQueue queue)
    {
        _queue = queue;
    }

    public async Task Handle(OrderCheckedout notification, CancellationToken cancellationToken)
    {
        var order = (OrderCheckedoutData)notification.Data;

        await _queue.PublishAsync(order, "order.checkedout");
    }
}
