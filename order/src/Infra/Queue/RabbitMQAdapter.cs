using Application.Abstractions.Queue;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
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
        var factory = new ConnectionFactory { 
            HostName = rabbitSettings["Host"], 
            UserName = rabbitSettings["Username"] ,
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

    public void Consume(string queue)
    {
        throw new NotImplementedException();
    }
}
