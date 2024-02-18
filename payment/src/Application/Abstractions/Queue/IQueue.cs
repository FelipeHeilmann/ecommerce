namespace Application.Abstractions.Queue;

public interface IQueue
{
    void Connect();
    Task SubscribeAsync<T>(string queueName, Func<T, Task> callback);
    Task PublishAsync<T>(T message, string queueName);
}
