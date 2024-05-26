namespace API.Gateway;

public interface IOrderGateway
{
    Task<CustomerGatewayResponse> GetCustomerById(Guid id);
    Task<ProductGatewayResponse> GetProductById(Guid id);
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

public record ProductGatewayResponse(
      Guid Id,
      string Name, 
      string Description,
      double Amount,
      string currency
    );

