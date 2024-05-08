using Domain.Customers.Entity;

namespace Application.Abstractions.Services;

public interface IJwtProvider
{
    string Generate(Customer customer);
}
