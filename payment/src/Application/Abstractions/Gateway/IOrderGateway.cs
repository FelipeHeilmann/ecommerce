using System.Reflection.Emit;

namespace Application.Abstractions.Gateway;

public interface IOrderGateway
{
    Task<CustomerGatewayResponse> GetCustomerById(Guid id);

    Task<AddressGatewayResponse> GetAddressById(Guid id);
}

public record CustomerGatewayResponse(
    Guid Id,
    string Name,
    string Email,
    string CPF,
    string Phone,
    DateTime BirthDate,
    DateTime CreatedAt
);

public record AddressGatewayResponse(
    Guid Id,
    Guid CustomerId,
    string ZipCode,
    string Street,
    string Neighborhood,
    string Number,
    string? Complement,
    string City,
    string State,
    string Country 
);

 
