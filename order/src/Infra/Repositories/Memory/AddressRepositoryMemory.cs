using Domain.Addresses.Entity;
using Domain.Addresses.Repository;

namespace Infra.Repositories.Memory;

public class AddressRepositoryMemory : IAddressRepository
{
    private readonly List<Address> _addresses;

    public AddressRepositoryMemory()
    {
        _addresses = [];
    }

    public Task<ICollection<Address>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<Address>>(_addresses);
    }
    public IQueryable<Address> GetQueryable(CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_addresses.FirstOrDefault(a => a.Id == id));
    }

    public Task<ICollection<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<Address>>(_addresses.Where(a => a.CustomerId == customerId).ToList());
    }

    public Task Add(Address entity)
    {
        _addresses.Add(entity);
        return Task.CompletedTask;
    }

    public Task Update(Address entity)
    {
        var index = _addresses.FindIndex(a => a.Id == entity.Id);

        if (index == -1)
        {
             return Task.CompletedTask;
        }

        _addresses[index] = entity;
        return Task.CompletedTask;
    }

    public Task Delete(Address entity)
    {
        _addresses.Remove(entity);
         return Task.CompletedTask;
    }
}
