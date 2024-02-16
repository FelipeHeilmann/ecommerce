using Application.Abstractions.Queue;
using Application.Abstractions.Services;
using Application.Data;
using Domain.Addresses;
using Domain.Categories;
using Domain.Customers;
using Domain.Orders;
using Domain.Products;
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

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IEventBus, EventBus>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }
}
