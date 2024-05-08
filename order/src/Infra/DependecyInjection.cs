using Application.Abstractions.Queue;
using Application.Abstractions.Services;
using Application.Data;
using Domain.Addresses.Repository;
using Domain.Categories.Repository;
using Domain.Customers.Repository;
using Domain.Orders.Repository;
using Domain.Products.Repository;
using Infra.Authenication;
using Infra.Context;
using Infra.Data;
using Infra.Implementations;
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

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
