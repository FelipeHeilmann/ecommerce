
using API.Events;
using API.Gateway;
using Application.Abstractions.Queue;

namespace API.Consumers
{
    public class OrderPurchasedConsumer : BackgroundService
    {
        private readonly IQueue _queue;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOrderGateway _orderGateway;

        public OrderPurchasedConsumer(IQueue queue, IServiceProvider serviceProvider, IOrderGateway orderGateway)
        {
            _queue = queue;
            _serviceProvider = serviceProvider;
            _orderGateway = orderGateway;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _queue.SubscribeAsync<OrderPurchasedEvent>("orderPurchased.notification", "order.purchased", async message => {
                    using (var scope = _serviceProvider.CreateAsyncScope())
                    {
                        var mailerGateway = scope.ServiceProvider.GetRequiredService<IMailerGateway>();

                        var customer = await _orderGateway.GetCustomerById(message.CustomerId);

                        var mailData = new Maildata()
                        {
                            EmailToEmail = customer.Email,
                            EmailToName = customer.Name,
                            EmailBody = Templates.OrderPurchased(customer.Name, message.OrderId, message.Items.Sum(item => item.Amount * item.Quantity)),
                            EmailSubject = "Payment Recived"
                        };

                        await mailerGateway.Send(mailData);
                    }
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
