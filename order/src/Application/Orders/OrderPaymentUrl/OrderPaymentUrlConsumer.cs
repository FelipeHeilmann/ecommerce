using Application.Abstractions.Queue;
using Domain.Orders.Event;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Orders.OrderPaymentUrl;

public class OrderPaymentUrlConsumer : BackgroundService
{
    private readonly IQueue _queue;
    private readonly ILogger<OrderPaymentUrlEvent> _logger;
    public OrderPaymentUrlConsumer(ILogger<OrderPaymentUrlEvent> logger, IQueue queue)
    {
        _logger = logger;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<OrderPaymentUrlEvent>("orderPurchased.url", "payment.url", async message =>
            {
                _logger.LogInformation($"The payment from order {message.OrderId} was sent to the gateway and this is the url {message.Url}");
                _logger.LogInformation("The payment url is sent to the web socket now");
            });
        }
    }
}
