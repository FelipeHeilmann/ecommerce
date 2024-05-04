using Application.Abstractions.Queue;

namespace Infra.Queue;

public class MemoryMQAdapter : IQueue
{
   
    public void Connect()
    {
        return;
    }


    public Task PublishAsync<T>(T message, string queueName)
    {
        return Task.CompletedTask;
    }

    public Task SubscribeAsync<T>(string queueName, string routingKey, Func<T, Task> callback)
    {
        return Task.CompletedTask;
    }
}
