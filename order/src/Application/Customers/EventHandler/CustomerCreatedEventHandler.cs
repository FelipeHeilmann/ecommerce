using Application.Gateway;
using Domain.Customers;
using MediatR;

namespace Application.Customers.EventHandler;

public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
{
    private readonly INotifyGateway _notifyGateway;

    public CustomerCreatedEventHandler(INotifyGateway notifyGateway)
    {
        _notifyGateway = notifyGateway;
    }

    public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _notifyGateway.SendWelcomeMail(notification.Name, notification.Email);
    }
}
