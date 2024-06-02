using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace NotifyWorker.Queue;

public class RabbitMQAdapter : IQueue
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private const string EXCHANGE = "order.events";

    public RabbitMQAdapter(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentException();
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

    public async Task SubscribeAsync<T>(string queueName, string routingKey, Func<T, Task> callback)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: EXCHANGE, type: ExchangeType.Topic);
            channel.QueueDeclare(queue: queueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            channel.QueueBind(queue: queueName,
                              exchange: EXCHANGE,
                              routingKey: routingKey);

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

    public async Task PublishAsync<T>(T message, string routingKey)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: EXCHANGE, type: ExchangeType.Topic);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: EXCHANGE,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
