using Domain.Shared;

namespace Domain.Addresses;

public interface IAddressRepository : IRepositoryBase<Address>
{
    public Task<ICollection<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
}
