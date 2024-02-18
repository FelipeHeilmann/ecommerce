using Application.Abstractions.Queue;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Transactions.Consumers;

public class OrderPurchasedEventConsumer 
{
    private readonly ILogger<OrderPurchasedEventConsumer> _logger;
    private readonly IQueue _queue;

    //private readonly ITransactionRepository _transactionRepository;

    public OrderPurchasedEventConsumer(ILogger<OrderPurchasedEventConsumer> logger, IQueue queue)
    {
        _logger = logger;
        _queue = queue;
    }

    public void Consume()
    {
        var order = _queue.Consume("order-purchased");

        _logger.LogInformation("Received OrderCompletedEvent: {@Order}", order.ToString());

        return;
    }
}

