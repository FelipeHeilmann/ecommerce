using Domain.Addresses.Entity;
using Domain.Shared;

namespace Domain.Addresses.Repository;

public interface IAddressRepository : IRepositoryBase<Address>
{
    public Task<ICollection<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
}
