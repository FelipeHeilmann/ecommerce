using API.Events;
using API.Gateway;
using Application.Abstractions.Queue;

namespace API.Consumers
{
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
            while (!stoppingToken.IsCancellationRequested)
            {
                await _queue.SubscribeAsync<OrderCheckedoutEvent>("orderCheckedout.notification", "order.checkedout", async message =>
                {
                    using (var scope = _serviceProvider.CreateAsyncScope())
                    {
                        var mailerGateway = scope.ServiceProvider.GetRequiredService<IMailerGateway>();

                        var mailData = new Maildata()
                        {
                            EmailToEmail = message.Email,
                            EmailToName = message.Name,
                            EmailSubject = "Order Created",
                            EmailBody = Templates.OrderCreated(message)
                        };

                        await mailerGateway.Send(mailData);
                    }
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}