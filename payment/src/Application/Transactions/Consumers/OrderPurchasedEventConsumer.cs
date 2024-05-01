using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Abstractions.Queue;
using Domain.Events;
using Application.Transactions.MakePaymentRequest;
using Application.Abstractions.Gateway;
using Application.Data;
using Domain.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Application.Gateway;

namespace Application.Transactions.Consumers;

public class OrderPurchasedEventConsumer : BackgroundService
{
    private readonly ILogger<OrderPurchasedEventConsumer> _logger;
    private readonly INotifyGateway _notifyGateway;
    private readonly IServiceProvider _serviceProvider;
    private readonly IQueue _queue;

    public OrderPurchasedEventConsumer(ILogger<OrderPurchasedEventConsumer> logger, IQueue queue, IServiceProvider serviceProvider, INotifyGateway notifyGateway)
    {
        _logger = logger;
        _queue = queue;
        _serviceProvider = serviceProvider;
        _notifyGateway = notifyGateway;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<OrderPurchasedEvent>("order-purchased", async message => {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var paymentGateway = scope.ServiceProvider.GetRequiredService<IPaymentGateway>();
                    var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                    var queue = scope.ServiceProvider.GetRequiredService<IQueue>();

                    var command = new CreatePaymentCommand(message);
                    var commandHandler = new CreatePaymentCommandHandler(paymentGateway, transactionRepository, unitOfWork);

                    await commandHandler.Handle(command, stoppingToken);

                    await _notifyGateway.SendPaymentRecivedMail(new PaymenRecivedRequest(message.CustomerName, message.CustomerEmail, message.OrderId, message.Items.Sum(item => item.Amount * item.Quantity)));
                }
            });

            await Task.Delay(1000, stoppingToken);
        }
    }
}


