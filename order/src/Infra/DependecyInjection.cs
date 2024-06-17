using Application;
using Application.Abstractions.Query;
using Application.Abstractions.Queue;
using Application.Abstractions.Services;
using Application.Orders.OrderPaymentStatusChanged;
using Domain.Addresses.Repository;
using Domain.Categories.Repository;
using Domain.Coupons.Repository;
using Domain.Customers.Repository;
using Domain.Orders.Repository;
using Domain.Products.Repository;
using Infra.Authenication;
using Infra.Context;
using Infra.Database;
using Infra.Queue;
using Infra.Repositories.Database;
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

        services.AddTransient<IDatabaseConnection, NpgsqlAdapter>();
        services.AddTransient<ICustomerRepository, CustomerRepositoryDatabase>();
        services.AddTransient<IOrderRepository, OrderRepositoryDatabase>();
        services.AddTransient<IProductRepository, ProductRepositoryDatabase>();
        services.AddTransient<ICategoryRepository, CategoryRepositoryDatabase>();
        services.AddTransient<IAddressRepository, AddressRepositoryDatabase>();
        services.AddTransient<ICouponRepository, CouponRepositoryDatabase>();
        services.AddTransient<IOrderQueryContext, MongoOrderContext>();
        services.AddTransient<OrderPaymentStatusChangedCommandHandler>();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddHostedService<QueueController>();
    }
}
