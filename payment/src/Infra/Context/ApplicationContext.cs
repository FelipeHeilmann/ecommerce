using Domain.Refunds;
using Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Refund> Refund { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}
