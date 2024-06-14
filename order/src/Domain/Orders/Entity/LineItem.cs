using Domain.Products.VO;

namespace Domain.Orders.Entity;

public class LineItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    private Money _price;
    public string Currency => _price.Currency;
    public double Amount => _price.Amount;
    public int Quantity { get; private set; }

    public LineItem(Guid id, Guid orderId, Guid productId, Money price, int quantity)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        _price = price;
        Quantity = quantity;
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
