using NotifyWorker.Events;
using NotifyWorker.Gateway;
using NotifyWorker.Queue;

namespace NotifyWorker.Consumers;

public class CustomerCreatedConsumer : BackgroundService
{
    private readonly IQueue _queue;
    private readonly IServiceProvider _serviceProvider;

    public CustomerCreatedConsumer(IQueue queue, IServiceProvider serviceProvider)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<CustomerCreatedEvent>("customerCreated.notification", "customer.created", async message => {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var mailerGateway = scope.ServiceProvider.GetRequiredService<IMailerGateway>();

                var mailData = new Maildata()
                {
                    EmailBody = Templates.Welcome(message.Name),
                    EmailSubject = "Welcome",
                    EmailToEmail = message.Email,
                    EmailToName = message.Name,
                };

                await mailerGateway.Send(mailData);
            }
        });
    }
}
