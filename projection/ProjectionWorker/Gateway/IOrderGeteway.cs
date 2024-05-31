namespace ProjectionWorker.Gateway;

public interface IOrderGeteway
{
    Task<ProductGatewayResponse> GetProductById(Guid id);
    Task<AddressGatewayResponse> GetAddressById(Guid id);
}

public record ProductGatewayResponse(
      Guid Id,
      string Name,
      string Description,
      double Amount,
      string currency,
      string ImageUrl
 );

public record AddressGatewayResponse(
    Guid Id, 
    string ZipCode, 
    string Street, 
    string Neighborhood, 
    string Number, 
    string? Complement, 
    string State, 
    string City, 
    string Country
);

