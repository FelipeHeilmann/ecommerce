namespace NotifyWorker.Gateway;

public interface IOrderGateway
{
    Task<CustomerGatewayResponse> GetCustomerById(Guid id);
    Task<ProductGatewayResponse> GetProductById(Guid id);
    Task<OrderGatewayResponse> GetOrderById(Guid id);
    Task<CustomerGatewayResponse> GetCustomerByOrderId(Guid id);
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

public class OrderGatewayResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public ICollection<LineItemGatewayResponse> Items { get; set; }
    public AddressGatewayResponse? Address { get; set; }
    public Guid CustomerId { get; set; }
    public double Total { get; set; }
    public DateTime? PayedAt { get; set; }
}

public class LineItemGatewayResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
}

public class AddressGatewayResponse
{
    public Guid Id { get; set; }
    public string ZipCode { get; set; }
    public string Street { get; set; }
    public string Neighborhood { get; set; }
    public string Number { get; set; }
    public string? Complement { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}