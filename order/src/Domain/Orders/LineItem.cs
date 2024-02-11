using Domain.Products;

namespace Domain.Orders;

public class LineItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Money Price { get; private set; }
    public int Quantity { get; private set; }

    public LineItem(Guid id, Guid orderId ,Guid productId, Money price, int quantity)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Price = price;
        Quantity = quantity;  
    }

    public LineItem()
    {
    }

    public bool DeleteItem()
    {
        if (Quantity == 1) return true;
        if (Quantity > 1) Quantity--;
        return false;
    }
};
