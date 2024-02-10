using Domain.Customer;
using Domain.Orders;
using Domain.Products;
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
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
