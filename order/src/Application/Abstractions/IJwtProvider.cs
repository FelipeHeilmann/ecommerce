using Domain.Customer;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string Generate(Customer customer);
}
