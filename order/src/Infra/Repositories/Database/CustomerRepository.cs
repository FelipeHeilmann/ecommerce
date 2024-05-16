using Domain.Customers.Entity;
using Domain.Customers.Repository;
using Infra.Context;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationContext _context;
    public CustomerRepository(ApplicationContext context)
    {
        _context = context;
    }

    public IQueryable<Customer> GetQueryable(CancellationToken cancellationToken)
    {
        var customers = new List<Customer>();
        foreach(var customerModel in _context.Set<CustomerModel>().ToList()) 
        {
            customers.Add(customerModel.ToAggregate());
        }
        return customers.AsQueryable();
    }
    public async Task<ICollection<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        var customers = new List<Customer>();
        foreach (var customerModel in await _context.Set<CustomerModel>().ToListAsync(cancellationToken))
        {
            customers.Add(customerModel.ToAggregate());
        }
        return customers.ToList();
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var customerModel = await _context.Set<CustomerModel>().FirstOrDefaultAsync(model => model.Id == id, cancellationToken);
        return customerModel?.ToAggregate();
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var customerModel = await _context.Set<CustomerModel>().FirstOrDefaultAsync(model => model.Email == email, cancellationToken);
        return customerModel?.ToAggregate();
    }


    public async Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Set<CustomerModel>().AnyAsync(model => model.Email == email, cancellationToken);
    }

    public void Add(Customer entity)
    {
        _context.Set<CustomerModel>().Add(CustomerModel.FromAggregate(entity));
    }

    public void Update(Customer entity)
    {
        _context.Set<CustomerModel>().Update(CustomerModel.FromAggregate(entity)); ;
    }

    public void Delete(Customer entity)
    {
        _context.Set<CustomerModel>().Remove(CustomerModel.FromAggregate(entity));
    }
}
