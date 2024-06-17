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
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderPaymentUrlEvent> _logger;

    public QueueController(IQueue queue, IServiceProvider serviceProvider, ILogger<OrderPaymentUrlEvent> logger)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<TransactionStatusChanged>("transactionApproved.updateOrder", "transaction.status.changed", async @event =>
            {
                using (var scope = _serviceProvider.CreateAsyncScope()) 
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                    var orderPaymentStatusChangedCommand = new OrderPaymentStatusChangedCommand(@event.OrderId, @event.Status);

                    var orderPaymentStatusChangedCommandHandler = new OrderPaymentStatusChangedCommandHandler(orderRepository);

                    await orderPaymentStatusChangedCommandHandler.Handle(orderPaymentStatusChangedCommand, stoppingToken);
                }
            });

            await _queue.SubscribeAsync<OrderPaymentUrlEvent>("orderPurchased.url", "payment.url", async @event =>
            {
                _logger.LogInformation($"The payment from order {@event.OrderId} was sent to the gateway and this is the url {@event.Url}");
                _logger.LogInformation("The payment url is sent to the web socket now");
            });
        }
    }
}
