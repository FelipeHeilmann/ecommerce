using Application.Abstractions.Queue;
using Application.Orders.Delivery;
using Application.Orders.OrderPaymentStatusChanged;
using Application.Orders.ShipOrder;
using Domain.Orders.Event;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infra.Queue;

public class QueueController : BackgroundService
{
    private readonly IQueue _queue;
    private readonly ILogger<OrderPaymentUrlEvent> _logger;
    private readonly OrderPaymentStatusChangedCommandHandler _orderPaymentStatusChangedCommandHandler;
    private readonly ShipOrderCommandHandler _shipOrderCommandHandler;
    private readonly DeliveryOrderCommandHandler _deliveryOrderCommandHandler;

    public QueueController(IQueue queue, OrderPaymentStatusChangedCommandHandler orderPaymentStatusChangedCommandHandler, ShipOrderCommandHandler shipOrderCommandHandler, DeliveryOrderCommandHandler deliveryOrderCommandHandler ,ILogger<OrderPaymentUrlEvent> logger)
    {
        _queue = queue;
        _orderPaymentStatusChangedCommandHandler = orderPaymentStatusChangedCommandHandler;
        _shipOrderCommandHandler = shipOrderCommandHandler;
        _deliveryOrderCommandHandler = deliveryOrderCommandHandler;
        _logger = logger;
    }

   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<TransactionStatusChanged>("transactionApproved.updateOrder", "transaction.status.changed", async @event =>
        {
            var orderPaymentStatusChangedCommand = new OrderPaymentStatusChangedCommand(@event.OrderId, @event.Status);
            await _orderPaymentStatusChangedCommandHandler.Handle(orderPaymentStatusChangedCommand, stoppingToken);
        });

        await _queue.SubscribeAsync<OrderPaymentUrlEvent>("orderPurchased.url", "payment.url", @event =>
        {
            _logger.LogInformation($"The payment from order {@event.OrderId} was sent to the gateway and this is the url {@event.Url}");
            _logger.LogInformation("The payment url is sent to the web socket now");
            return Task.CompletedTask;
        });

        await _queue.SubscribeAsync<OrderShipped>("orderShipped.updateOrder", "order.shipped", async @event => {
            var shipOrderCommand = new ShipOrderCommand(@event.OrderId);
            await _shipOrderCommandHandler.Handle(shipOrderCommand, stoppingToken);
        });

        await _queue.SubscribeAsync<OrderDelivered>("orderDelivered.updateOrder", "order.delivered", async @event => {
            var deliveryOrderCommand = new DeliveryOrderCommand(@event.OrderId);
            await _deliveryOrderCommandHandler.Handle(deliveryOrderCommand, stoppingToken);
        });
    }
}
