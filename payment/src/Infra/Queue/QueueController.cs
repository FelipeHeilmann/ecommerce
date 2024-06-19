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
    private readonly CreatePaymentCommandHandler _createPaymentCommandHandler;

    public QueueController(IQueue queue, IServiceProvider serviceProvider, CreatePaymentCommandHandler createPaymentCommandHandler)
    {
        _queue = queue;
        _createPaymentCommandHandler = createPaymentCommandHandler;        
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<OrderCheckedout>("orderCheckedout.proccessPayment", "order.checkedout", async @event => {
                var command = new CreatePaymentCommand(@event);

                await _createPaymentCommandHandler.Handle(command, stoppingToken);          
            });
        }
    }
}
