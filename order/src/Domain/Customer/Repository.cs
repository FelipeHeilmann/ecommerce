using Domain.Shared;

namespace Domain.Customer;

public interface ICustomerRepository :IRepositoryBase<Customer> 
{
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
