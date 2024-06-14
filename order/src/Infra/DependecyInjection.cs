using Application;
using Application.Abstractions.Query;
using Application.Abstractions.Queue;
using Application.Abstractions.Services;
using Application.Orders.Consumer;
using Application.Orders.OrderPaymentUrl;
using Domain.Addresses.Repository;
using Domain.Categories.Repository;
using Domain.Coupons.Repository;
using Domain.Customers.Repository;
using Domain.Orders.Repository;
using Domain.Products.Repository;
using Infra.Authenication;
using Infra.Context;
using Infra.Database;
using Infra.Implementations;
using Infra.Queue;
using Infra.Repositories.Database;
using Infra.Repositories.Memory;
using MediatR;
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

        services.AddScoped<IDatabaseConnection, NpgsqlAdapter>();
        services.AddScoped<ICustomerRepository, CustomerRepositoryDatabase>();
        services.AddScoped<IOrderRepository, OrderRepositoryMemory>();
        services.AddScoped<IProductRepository, ProductRepositoryDatabase>();
        services.AddScoped<ICategoryRepository, CategoryRepositoryDatabase>();
        services.AddScoped<IAddressRepository, AddressRepositoryDatabase>();
        services.AddScoped<ICouponRepository, CouponRepositoryDatabase>();
        services.AddScoped<IOrderQueryContext, MongoOrderContext>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddHostedService<OrderPaymentUrlConsumer>();
        services.AddHostedService<TransactionStatusConsumer>();
    }
}
