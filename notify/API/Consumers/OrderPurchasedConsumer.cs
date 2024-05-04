
using API.Events;
using API.Gateway;
using Application.Abstractions.Queue;

namespace API.Consumers
{
    public class OrderPurchasedConsumer : BackgroundService
    {
        private readonly IQueue _queue;
        private readonly IServiceProvider _serviceProvider;

        public OrderPurchasedConsumer(IQueue queue, IServiceProvider serviceProvider)
        {
            _queue = queue;
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _queue.SubscribeAsync<OrderPurchasedEvent>("order-purchased-notification", "order.purchased", async message => {
                    using (var scope = _serviceProvider.CreateAsyncScope())
                    {
                        var mailerGateway = scope.ServiceProvider.GetRequiredService<IMailerGateway>();

                        var mailData = new Maildata()
                        {
                            EmailToEmail = message.CustomerEmail,
                            EmailToName = message.CustomerName,
                            EmailBody = Templates.OrderPurchased(message.CustomerName, message.OrderId, message.Items.Sum(item => item.Amount * item.Quantity)),
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
