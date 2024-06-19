using Application.Abstractions.Queue;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Infra.Queue;

public class RabbitMQAdapter : IQueue
{
    private IConnection _connection;

    public RabbitMQAdapter(IConfiguration configuration)
    {
        var rabbitSettings = configuration.GetSection("MessageBroker");
        var factory = new ConnectionFactory
        {
            HostName = rabbitSettings["Host"],
            UserName = rabbitSettings["Username"],
            Password = rabbitSettings["Password"]
        };
        _connection = factory.CreateConnection();
    }

    public async Task SubscribeAsync<T>(string queueName, string routingKey, Func<T, Task> callback)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "order.events", type: ExchangeType.Topic);
            channel.QueueDeclare(queue: queueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            channel.QueueBind(queue: queueName,
                              exchange: "order.events",
                              routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                await callback(message ?? throw new ArgumentException());

                channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            await Task.Delay(-1);
        }
    }

    public Task PublishAsync<T>(T message, string routingKey)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "order.events", type: ExchangeType.Topic);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: "order.events",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
            return Task.CompletedTask;                                
        }
    }
}
