using Domain.Addresses.Entity;
using Domain.Addresses.Repository;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    public AddressRepository(ApplicationContext applicationContext) : base(applicationContext) { }

    public async Task<ICollection<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var addresses = await _context.Set<Address>().Where(a => a.CustomerId == customerId).ToListAsync(cancellationToken);

        return addresses;
    }
}
