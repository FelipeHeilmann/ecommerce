namespace API.Gateway;

public interface IOrderGateway
{
    Task<CustomerGatewayResponse> GetCustomerById(Guid id);
}

public record CustomerGatewayResponse(
    Guid Id,
    string Name,
    string Email,
    string CPF,
    string Phone,
    DateOnly BirthDate,
    DateTime CreatedAt
);

