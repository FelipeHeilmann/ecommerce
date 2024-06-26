﻿using Application.Abstractions.Gateway;
using Application.Abstractions.Queue;
using Application.Transactions.CreatePaymentCommand;
using Domain.Refunds;
using Domain.Transactions.Repository;
using Infra.Database;
using Infra.Gateway.Order;
using Infra.Gateway.Payment;
using Infra.Queue;
using Infra.Repositories.Database;
using Infra.Repositories.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependecyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddSingleton<IQueue, RabbitMQAdapter>();

        services.AddSingleton<IPaymentGateway, PaymentGatewayFake>();
        services.AddTransient<ITransactionRepository, TransactionRepositoryDatabase>();
        services.AddTransient<IOrderGateway, OrderGatewayHttp>();
        services.AddTransient<IDatabaseConnection, NpgsqlAdapter>();
        services.AddTransient<IRefundRepository, RefundRepositoryMemory>();
        services.AddTransient<CreatePaymentCommandHandler>();
        services.AddHostedService<QueueController>();
    }
}
