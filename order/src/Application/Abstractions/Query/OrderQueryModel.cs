namespace Application.Abstractions.Query;

public class OrderQueryModel
{
 
    public Guid Id { get; set; }
    public string Status { get; set; }
    public ICollection<LineItemQueryModel> Items { get; set; }
    public Guid CustomerId { get; set; }
    public double Total { get; set; }
    public DateTime? PayedAt { get; set; }
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
