namespace ProjectionWorker.Queue;

public interface IQueue
{
    Task SubscribeAsync<T>(string queueName, string routingKey, Func<T, Task> callback);
    Task PublishAsync<T>(T message, string routingKey);
}
