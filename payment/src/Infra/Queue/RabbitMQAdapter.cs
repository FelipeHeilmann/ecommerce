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
    private readonly IConfiguration _configuration;

    public RabbitMQAdapter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void On()
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

    public void Publish(object message, string queue)
    {
        var channel = _connection.CreateModel();

        channel.QueueDeclare(queue: queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        string jsonMessage = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "",
                             routingKey: queue,
                             basicProperties: null,
                             body: body);
        channel.Close();
        _connection.Close();
    }
    public object Consume(string queue)
    {
        var messages = new List<object>();

        using var channel = _connection.CreateModel();

        channel.QueueDeclare(queue: "order-purchased",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine(message);

            var jsonObject = JsonSerializer.Deserialize<object>(message);

            messages.Add(jsonObject);
        };
        channel.BasicConsume(queue: "order-purchased",
                             autoAck: true,
                             consumer: consumer);

        return messages;
    }
}
