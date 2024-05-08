using Domain.Addresses.Entity;
using Domain.Addresses.Repository;

namespace Infra.Repositories.Memory;

public class AddressRepositoryInMemory : IAddressRepository
{
    private readonly List<Address> _context = new();

    public Task<ICollection<Address>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult<ICollection<Address>>(_context);
    }
    public IQueryable<Address> GetQueryable(CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult(_context.FirstOrDefault(a => a.Id == id));
    }

    public Task<ICollection<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<Address>>(_context.Where(a => a.CustomerId == customerId).ToList());
    }

    public void Add(Address entity)
    {
        _context.Add(entity);
    }

    public void Update(Address entity)
    {
        var index = _context.FindIndex(a => a.Id == entity.Id);

        if (index == -1)
        {
            return;
        }

        _context[index] = entity;
    }

    public void Delete(Address entity)
    {
        _context.Remove(entity);
    }
}
