using Domain.Customer;

namespace Application.Abstractions.Services;

public interface IJwtProvider
{
    string Generate(Customer customer);
}
