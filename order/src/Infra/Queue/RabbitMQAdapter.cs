using Application.Abstractions.Queue;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Infra.Queue;

public class RabbitMQAdapter : IQueue
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;

    public RabbitMQAdapter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Connect()
    {
        var rabbitSettings = _configuration.GetSection("MessageBroker");
        var factory = new ConnectionFactory
        {
            HostName = rabbitSettings["Host"],
            UserName = rabbitSettings["Username"],
            Password = rabbitSettings["Password"]
        };
        _connection = factory.CreateConnection();
    }

    public async Task SubscribeAsync<T>(string queueName, Func<T, Task> callback)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                await callback(message);

                channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            await Task.Delay(-1);
        }
    }

    public async Task PublishAsync<T>(T message, string queueName)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}