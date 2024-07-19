namespace NotifyWorker.Queue;

public interface IQueue
{
    Task SubscribeAsync<T>(string exchange, string routingKey, Func<T, Task> callback);
    Task PublishAsync<T>(T message, string queueName);
}
