using NotifyWorker.Events;
using NotifyWorker.Gateway;
using NotifyWorker.Model;
using Application.Abstractions.Queue;

namespace NotifyWorker.Consumers;

public class OrderCheckedoutConsumer : BackgroundService
{
    private readonly IQueue _queue;
    private readonly IServiceProvider _serviceProvider;

    public OrderCheckedoutConsumer(IQueue queue, IServiceProvider serviceProvider)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {

    await _queue.SubscribeAsync<OrderCheckedout>("orderCheckedout.notification", "order.checkedout", async message =>
    {
        using (var scope = _serviceProvider.CreateAsyncScope())
        {
            using (var client = new HttpClient())
            {
                var mailerGateway = scope.ServiceProvider.GetRequiredService<IMailerGateway>();

                var orderGateway = scope.ServiceProvider.GetRequiredService<IOrderGateway>();

                var customer = await orderGateway.GetCustomerById(message.CustomerId);

                var items = new List<OrderChecedoutItem>();
                foreach (var item in message.Items) 
                { 
                    var product = await orderGateway.GetProductById(item.ProductId);
                    items.Add(new OrderChecedoutItem(item.Id, product.Name, item.Quantity, item.Price));
                }

                var mailModel = new OrderCheckedoutMailModel(customer.Name, message.OrderId, DateTime.Now, items);

                var mailData = new Maildata()
                {
                    EmailToEmail = customer.Email,
                    EmailToName = customer.Name,
                    EmailSubject = "Order Created",
                    EmailBody = Templates.OrderCheckedout(mailModel)
                };

                await mailerGateway.Send(mailData);
            }
        }
    });

    }
}