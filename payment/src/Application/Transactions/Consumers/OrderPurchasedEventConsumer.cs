using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Abstractions.Queue;
using Domain.Events;

namespace Application.Transactions.Consumers;

public class OrderPurchasedEventConsumer : BackgroundService
{
    private readonly ILogger<OrderPurchasedEventConsumer> _logger;
    private readonly IQueue _queue;

    public OrderPurchasedEventConsumer(ILogger<OrderPurchasedEventConsumer> logger, IQueue queue)
    {
        _logger = logger;
        _queue = queue;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<OrderPurchasedEvent>("order-purchased", async message =>
            {
                _logger.LogInformation("Message received: {0}", message);
               
            });

            await Task.Delay(1000, stoppingToken);
        }
    }
}


