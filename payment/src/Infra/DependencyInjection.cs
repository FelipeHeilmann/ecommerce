using Application.Abstractions.Queue;
using Application.Transactions.Consumers;
using Infra.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependecyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IQueue, RabbitMQAdapter>(provider =>
        {
            var rabbitMQAdapter = new RabbitMQAdapter(configuration);
            rabbitMQAdapter.Connect();
            return rabbitMQAdapter;
        });

       services.AddHostedService<OrderPurchasedEventConsumer>();
    }
}
