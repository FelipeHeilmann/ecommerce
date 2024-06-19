namespace Application.Abstractions.Query;

public class OrderQueryModel
{
 
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public ICollection<LineItemQueryModel> Items { get; set; } = new List<LineItemQueryModel>();
    public PaymentQueryModel? Payment { get; set; }
    public AddressQueryModel? Address { get; set; }
    public Guid CustomerId { get; set; }
    public double Total { get; set; }
}

public class LineItemQueryModel
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class AddressQueryModel
{
    public Guid Id { get; set; }
    public string ZipCode { get; set; } = string.Empty;
    public string Street {  get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string? Complement {  get; set; }
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class PaymentQueryModel
{
    public string PaymentType { get; set; } = string.Empty;
    public int Installments { get; set; }
    public DateTime? PayedAt { get; set; }
}