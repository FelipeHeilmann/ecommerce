using Domain.Orders.Entity;
using Domain.Products.VO;

namespace Infra.Models.Orders;

public class OrderModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public ICollection<LineItemModel> Items { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? BillingAddressId { get; set; }
    public Guid? ShippingAddressId { get; set; }

    public OrderModel(Guid id, Guid custmerId, string status, ICollection<LineItemModel> items,DateTime createdAt, DateTime updatedAt, Guid? billingAddressId, Guid? shippingAddressId)
    {
        Id = id;
        CustomerId = custmerId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        BillingAddressId = billingAddressId;
        ShippingAddressId = shippingAddressId;
        Items = items;
    }

    public OrderModel() { }

    public Order ToAggregate()
    {
        var lineItems = Items.Select(li => new LineItem(li.Id, li.OrderId, li.ProductId, new Money(li.Currency, li.Amount), li.Quantity)).ToList();
        return new Order(Id, CustomerId, Status, lineItems, CreatedAt, UpdatedAt, BillingAddressId, ShippingAddressId); 
    }

    public static OrderModel FromAggreate(Order order)
    {
        var items = order.Items.Select(li => new LineItemModel(li.Id, li.OrderId, li.ProductId, li.Price.Currency, li.Price.Amount, li.Quantity)).ToList();    
        return new OrderModel(order.Id, order.CustomerId ,order.GetStatus(), items ,order.CreatedAt, order.UpdatedAt, order.BillingAddressId, order.ShippingAddressId);
    }
}
