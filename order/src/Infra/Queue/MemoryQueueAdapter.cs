using Application.Abstractions.Queue;

namespace Infra.Queue;

public class MemoryQueueAdapter : IQueue
{
    public Task PublishAsync<T>(T message, string queueName)
    {
        return Task.CompletedTask;
    }

    public Task SubscribeAsync<T>(string queueName, string routingKey, Func<T, Task> callback)
    {
        return Task.CompletedTask;
    }
}
