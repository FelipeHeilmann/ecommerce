using Application.Abstractions.Queue;
using Application.Data;
using Application.Orders.OrderPaymentStatusChanged;
using Domain.Orders.Event;
using Domain.Orders.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Consumer;

public class TransactionStatusConsumer : BackgroundService
{
    private readonly ILogger<TransactionStatusChanged> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IQueue _queue;

    public TransactionStatusConsumer(ILogger<TransactionStatusChanged> logger, IQueue queue, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    protected  override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<TransactionStatusChanged>("transactionApproved.updateOrder", "transaction.status.changed", async message =>
        {
            using (var scope = _serviceProvider.CreateAsyncScope()) 
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var orderPaymentStatusChangedCommand = new OrderPaymentStatusChangedCommand(message.OrderId, message.Status);

                var orderPaymentStatusChangedCommandHandler = new OrderPaymentStatusChangedCommandHandler(orderRepository, unitOfWork);

                await orderPaymentStatusChangedCommandHandler.Handle(orderPaymentStatusChangedCommand, stoppingToken);
            }
        });
    }
}
