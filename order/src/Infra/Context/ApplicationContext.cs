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

    DbSet<Customer> Customer {  get; set; }
    DbSet<Category> Category { get; set; }
    DbSet<Product> Product { get; set; }
    DbSet<Order> Order { get; set; }
    DbSet<LineItem> LineItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Observable>();
        modelBuilder.Ignore<Observer>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}
