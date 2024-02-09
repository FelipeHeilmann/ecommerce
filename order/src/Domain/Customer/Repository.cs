using Domain.Shared;

namespace Domain.Customer;

public interface ICustomerRepository :IRepositoryBase<Customer> 
{
    Task<bool> IsEmailUsedAsync(string  email);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
