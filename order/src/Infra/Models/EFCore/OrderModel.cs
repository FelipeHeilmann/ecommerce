using Domain.Orders.Entity;
using Domain.Products.VO;

namespace Infra.Models.EFCore;

public class OrderModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public ICollection<LineItemModel> Items { get; set; }
    public double Total {  get; set; }
    public Guid? CouponId { get; set; }
    public CouponModel? Coupon { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? BillingAddressId { get; set; }
    public Guid? ShippingAddressId { get; set; }

    public OrderModel(Guid id, Guid custmerId, string status, ICollection<LineItemModel> items, double total, CouponModel? coupon ,DateTime createdAt, DateTime updatedAt, Guid? billingAddressId, Guid? shippingAddressId)
    {
        Id = id;
        CustomerId = custmerId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        BillingAddressId = billingAddressId;
        ShippingAddressId = shippingAddressId;
        Coupon = coupon;
        CouponId = coupon?.Id;
        Items = items;
        Total = total;
    }

    public OrderModel() { }

    public Order ToAggregate()
    {
        var lineItems = Items.Select(li => li.ToAggregate()).ToList();
        return new Order(Id, CustomerId, Status, lineItems, Coupon?.ToAggregate() ,CreatedAt, UpdatedAt, BillingAddressId, ShippingAddressId);
    }

    public static OrderModel FromAggregate(Order order)
    {
        var items = order.Items.Select(li => LineItemModel.FromAggregate(li)).ToList();
        var coupon = order.Coupon is not null ? CouponModel.FromAggregate(order.Coupon) : null;
        return new OrderModel(order.Id, order.CustomerId, order.Status, items, order.CalculateTotal() ,coupon, order.CreatedAt, order.UpdatedAt, order.BillingAddressId, order.ShippingAddressId);
    }
}
