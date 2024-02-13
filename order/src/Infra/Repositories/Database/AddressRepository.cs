using Domain.Addresses;
using Infra.Context;

namespace Infra.Repositories.Database;

public class AddressRepository : Repository<Address>, IAddressRepository 
{
    public AddressRepository(ApplicationContext applicationContext) : base(applicationContext) { }
}
