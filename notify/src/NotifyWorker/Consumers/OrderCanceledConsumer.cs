using NotifyWorker.Events;
using NotifyWorker.Gateway;
using NotifyWorker.Queue;

namespace NotifyWorker.Consumers;

public class OrderCanceledConsumer : BackgroundService
{
    private readonly IQueue _queue;
    private readonly IServiceProvider _serviceProvider;

    public OrderCanceledConsumer(IQueue queue, IServiceProvider serviceProvider)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<OrderCanceled>("orderCanceled.notification", "order.canceled", async message => 
        {
            using (var scope = _serviceProvider.CreateAsyncScope()) 
            {
                var mailGateway = scope.ServiceProvider.GetService<IMailerGateway>();

                var orderGateway = scope.ServiceProvider.GetService<IOrderGateway>();

                var customer = await orderGateway.GetCustomerById(message.CustomerId);
                
                var mailData = new Maildata()
                {
                    EmailToEmail = customer.Email,
                    EmailToName = customer.Name,
                    EmailSubject = "Order canceled",
                    EmailBody = Templates.OrderCanceled(customer.Name, message.OrderId),
                };
                await mailGateway.Send(mailData);
            }
        });
    }
}
