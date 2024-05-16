using Domain.Orders.Entity;
using Domain.Products.VO;

namespace Infra.Models.Orders;

public class LineItemModel
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string Currency { get; set; }
    public double Amount { get; set; }
    public int Quantity { get; set; }

    public LineItemModel(Guid id, Guid orderId, Guid productId, string currency, double amount, int quantity)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Currency = currency;
        Amount = amount;
        Quantity = quantity;
    }

    public LineItemModel() { }

    public static LineItemModel FromAggregate(LineItem lineItem)
    {
        return new LineItemModel(lineItem.Id, lineItem.OrderId, lineItem.ProductId, lineItem.Price.Currency, lineItem.Price.Amount, lineItem.Quantity);
    }

    public LineItem ToModel()
    {
        return new LineItem(Id, OrderId, ProductId, new Money(Currency, Amount), Quantity);
    }
}
