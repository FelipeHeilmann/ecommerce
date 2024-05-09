using Domain.Customers.Entity;
using Domain.Customers.Repository;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationContext context)
        : base(context) { }
    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var customers = await _context.Set<Customer>().ToListAsync();
        return customers.FirstOrDefault(c => c.Email.Value == email);
    }

    public async Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
    {
        var customers = await _context.Set<Customer>().ToListAsync(cancellationToken);
        return customers.Any(c => c.Email.Value == email);
    }
}
