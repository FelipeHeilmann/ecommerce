using Domain.Products.VO;

namespace Domain.Orders.Entity;

public class LineItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Money Price { get; private set; }
    public int Quantity { get; private set; }

    public LineItem(Guid id, Guid orderId, Guid productId, Money price, int quantity)
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

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public void DecreaseQuantity()
    {
        Quantity--;
    }
         
};
