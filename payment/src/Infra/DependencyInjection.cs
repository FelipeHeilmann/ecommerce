using Application.Abstractions.Gateway;
using Application.Abstractions.Queue;
using Application.Data;
using Application.Transactions.Consumers;
using Domain.Refunds;
using Domain.Transactions;
using Infra.Context;
using Infra.Data;
using Infra.Gateway.Payment;
using Infra.Queue;
using Infra.Repositories.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependecyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddDbContext<ApplicationContext>(opt =>
        {
            opt
            .UseNpgsql(configuration.GetConnectionString("Database"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });

        services.AddSingleton<IQueue, RabbitMQAdapter>(provider =>
        {
            var rabbitMQAdapter = new RabbitMQAdapter(configuration);
            rabbitMQAdapter.Connect();
            return rabbitMQAdapter;
        });

        services.AddSingleton<IPaymentGateway, PaymentGatewayFake>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();   
        services.AddScoped<IRefundRepository, RefundRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

       services.AddHostedService<OrderPurchasedEventConsumer>();
    }
}
