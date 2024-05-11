using Domain.Customers.Entity;
using Domain.Customers.Repository;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationContext context)
        : base(context) { }
    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var customers = _context.Customers.ToListAsync(cancellationToken).Result;

        return Task.FromResult(customers.FirstOrDefault(c => c.Email == email));
    }

    public Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
    {
        var customers = _context.Customers.ToListAsync(cancellationToken).Result;

        return Task.FromResult(customers.Any(c => c.Email == email));
    }
}
