using Domain.Products;

namespace Domain.Orders;

public class LineItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Money Price { get; private set; }
    public int Quantity { get; private set; }
    public Product? Product { get; private set; } = null;

    public LineItem(Guid guid, Guid productId, Money price, int quantity)
    {
        Id = guid;
        ProductId = productId;
        Price = price;
        Quantity = quantity;    
    }

    public bool DeleteItem()
    {
        if (Quantity == 1) return true;
        if (Quantity > 1) Quantity--;
        return false;
    }
};
