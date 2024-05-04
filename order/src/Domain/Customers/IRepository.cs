using Domain.Shared;

namespace Domain.Customers;

public interface ICustomerRepository :IRepositoryBase<Customer> 
{
    Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
