﻿using Microsoft.Extensions.Hosting;
using Application.Abstractions.Queue;
using Domain.Events;
using Application.Transactions.MakePaymentRequest;
using Application.Abstractions.Gateway;
using Application.Data;
using Microsoft.Extensions.DependencyInjection;
using Domain.Transactions.Repository;

namespace Application.Transactions.Consumers;

public class OrderCheckedoutEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IQueue _queue;

    public OrderCheckedoutEventConsumer(IQueue queue, IServiceProvider serviceProvider)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<OrderCheckedout>("orderCheckedout.proccessPayment", "order.checkedout", async message => {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var paymentGateway = scope.ServiceProvider.GetRequiredService<IPaymentGateway>();
                    var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                    var queue = scope.ServiceProvider.GetRequiredService<IQueue>();
                    var orderGateway = scope.ServiceProvider.GetRequiredService<IOrderGateway>();

                    var command = new CreatePaymentCommand(message);
                    var commandHandler = new CreatePaymentCommandHandler(paymentGateway, transactionRepository, unitOfWork, queue, orderGateway);

                    await commandHandler.Handle(command, stoppingToken);
                }
            });

            await Task.Delay(1000, stoppingToken);
        }
    }
}

