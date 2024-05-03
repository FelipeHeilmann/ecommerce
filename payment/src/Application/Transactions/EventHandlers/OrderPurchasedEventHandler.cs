using Application.Gateway;
using Domain.Events;
using MediatR;

namespace Application.Transactions.EventHandlers;

public class OrderPurchasedEventHandler : INotificationHandler<OrderPurchasedEvent>
{
    private readonly INotifyGateway _notifyGateway;

    public OrderPurchasedEventHandler(INotifyGateway notifyHandler)
    {
        _notifyGateway = notifyHandler;
    }

    public async Task Handle(OrderPurchasedEvent notification, CancellationToken cancellationToken)
    {
        await _notifyGateway.SendPaymentRecivedMail(new PaymenRecivedRequest(notification.CustomerName, notification.CustomerEmail, notification.OrderId, notification.Items.Sum(item => item.Amount * item.Quantity)));
    }
}
