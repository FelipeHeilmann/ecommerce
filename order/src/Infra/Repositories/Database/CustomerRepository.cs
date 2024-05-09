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
        return await _context.Set<Customer>().FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Set<Customer>().AnyAsync(c => c.Email == email, cancellationToken);
    }
}
