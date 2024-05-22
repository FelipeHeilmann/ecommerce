using Domain.Addresses.Entity;
using Domain.Addresses.Repository;
using Infra.Context;
using Infra.Models.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationContext _context;
    public AddressRepository(ApplicationContext context) 
    { 
        _context = context;
    }

    public IQueryable<Address> GetQueryable(CancellationToken cancellationToken)
    {
       var addresses = new List<Address>();
       foreach(var addressModel in _context.Set<AddressModel>().ToList()) 
       { 
            addresses.Add(addressModel.ToAggregate()); 
       }
       return addresses.AsQueryable();
    }

    public async Task<ICollection<Address>> GetAllAsync(CancellationToken cancellationToken)
    {
        var addresses = new List<Address>();
        foreach (var addressModel in await _context.Set<AddressModel>().ToListAsync())
        {
            addresses.Add(addressModel.ToAggregate());
        }
        return addresses.ToList();
    }

    public async Task<ICollection<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var addresses = await _context.Set<AddressModel>().Where(model => model.CustomerId == customerId)
                                                          .Select(model => model.ToAggregate())
                                                          .ToListAsync(cancellationToken);

        return addresses;
    }

    public async Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var addressModel = await _context.Set<AddressModel>().FirstOrDefaultAsync(model => model.Id == id);
        return addressModel?.ToAggregate();
    }

    public void Add(Address entity)
    {
        _context.Set<AddressModel>().Add(AddressModel.FromAggregate(entity));
    }

    public void Update(Address entity)
    {
        _context.Set<AddressModel>().Update(AddressModel.FromAggregate(entity));
    }
    public void Delete(Address entity)
    {
        _context.Set<AddressModel>().Remove(AddressModel.FromAggregate(entity));
    }
}
