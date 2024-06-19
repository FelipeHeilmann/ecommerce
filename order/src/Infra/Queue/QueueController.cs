using Application.Abstractions.Queue;
using Application.Orders.OrderPaymentStatusChanged;
using Domain.Orders.Event;
using Domain.Orders.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infra.Queue;

public class QueueController : BackgroundService
{
    private readonly IQueue _queue;
    private readonly OrderPaymentStatusChangedCommandHandler _orderPaymentStatusChangedCommandHandler;
    private readonly ILogger<OrderPaymentUrlEvent> _logger;

    public QueueController(IQueue queue, OrderPaymentStatusChangedCommandHandler orderPaymentStatusChangedCommandHandler, ILogger<OrderPaymentUrlEvent> logger)
    {
        _queue = queue;
        _orderPaymentStatusChangedCommandHandler = orderPaymentStatusChangedCommandHandler;
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
    }
}
