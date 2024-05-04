using Application.Abstractions.Queue;
using Domain.Customers;
using MediatR;

namespace Application.Customers.EventHandler;

public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
{
    private readonly IQueue _queue;

    public CustomerCreatedEventHandler(IQueue queue)
    {
        _queue = queue;
    }

    public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _queue.PublishAsync(new { Name = notification.Name, Email = notification.Email }, "customers");
    }
}
