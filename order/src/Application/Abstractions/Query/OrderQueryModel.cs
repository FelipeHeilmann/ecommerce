namespace Application.Abstractions.Query;

public class OrderQueryModel
{
 
    public Guid Id { get; set; }
    public string Status { get; set; }
    public ICollection<LineItemQueryModel> Items { get; set; }
    public PaymentQueryModel? Payment { get; set; }
    public AddressQueryModel? Address { get; set; }
    public Guid CustomerId { get; set; }
    public double Total { get; set; }
}

public class LineItemQueryModel
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
}

public class AddressQueryModel
{
    public Guid Id { get; set; }
    public string ZipCode { get; set; }
    public string Street {  get; set; }
    public string Neighborhood { get; set; }
    public string Number { get; set; }
    public string? Complement {  get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}

public class PaymentQueryModel
{
    public string PaymentType { get; set; }
    public int Installments { get; set; }
    public DateTime? PayedAt { get; set; }
}