using Domain.Abstractions;
using Domain.Categories.Entity;
using Domain.Customers.Entity;
using Domain.Orders.Entity;
using Domain.Products.Entity;
using Infra.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public DbSet<Customer> Customers {  get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<LineItem> LineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Observable>();
        modelBuilder.Ignore<Observer>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}
