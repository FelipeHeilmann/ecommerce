using Domain.Customers.Entity;
using Infra.Models.Addresses;
using Infra.Models.Categories;
using Infra.Models.Orders;
using Infra.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public DbSet<Customer> Customers {  get; set; }
    DbSet<CategoryModel> Categories { get; set; }
    DbSet<ProductsModel> Products { get; set; }
    DbSet<OrderModel> Orders { get; set; }
    DbSet<LineItemModel> LineItems { get; set; }
    DbSet<AddressModel> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}
