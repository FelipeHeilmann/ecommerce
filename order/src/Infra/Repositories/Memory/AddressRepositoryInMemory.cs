using Domain.Address;

namespace Infra.Repositories.Memory;

public class AddressRepositoryInMemory : IAddressRepository
{
    private readonly List<Address> _context = new();

    public Task<ICollection<Address>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult<ICollection<Address>>(_context);
    }

    public Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult(_context.FirstOrDefault(a => a.Id == id));
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
