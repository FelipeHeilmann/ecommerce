using Domain.Shared;

namespace Domain.Customers;

public interface ICustomerRepository :IRepositoryBase<Customer> 
{
    Task<bool> IsEmailUsedAsync(Email email, CancellationToken cancellationToken);
    Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
}
