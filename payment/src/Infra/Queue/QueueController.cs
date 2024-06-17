using Application.Abstractions.Gateway;
using Application.Abstractions.Queue;
using Application.Transactions.CreatePaymentCommand;
using Domain.Events;
using Domain.Transactions.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infra.Queue;

public class QueueController : BackgroundService
{
    private readonly IQueue _queue;
    private readonly IServiceProvider _serviceProvider;

    public QueueController(IQueue queue, IServiceProvider serviceProvider)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<OrderCheckedout>("orderCheckedout.proccessPayment", "order.checkedout", async @event => {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var paymentGateway = scope.ServiceProvider.GetRequiredService<IPaymentGateway>();
                    var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                    var queue = scope.ServiceProvider.GetRequiredService<IQueue>();
                    var orderGateway = scope.ServiceProvider.GetRequiredService<IOrderGateway>();

                    var command = new CreatePaymentCommand(@event);
                    var commandHandler = new CreatePaymentCommandHandler(paymentGateway, transactionRepository, queue, orderGateway);

                    await commandHandler.Handle(command, stoppingToken);
                }
            });

            await Task.Delay(1000, stoppingToken);
        }
    }
}
