using Application.Transactions.Consumers;
using Microsoft.Extensions.Hosting;


namespace Application.BackgroundTasks;

public class QueueConsumerHostedService : BackgroundService
{
    private readonly OrderPurchasedEventConsumer _consumer;

    public QueueConsumerHostedService(OrderPurchasedEventConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Consume messages from the queue
            _consumer.Consume();
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Adjust delay as needed
        }
    }
}
