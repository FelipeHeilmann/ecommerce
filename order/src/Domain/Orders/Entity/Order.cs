using Domain.Abstractions;
using Domain.Customers.Entity;
using Domain.Orders.Error;
using Domain.Orders.Events;
using Domain.Orders.VO;
using Domain.Products.VO;
using Domain.Shared;

namespace Domain.Orders.Entity;

public class Order : Observable
{
    private readonly ICollection<LineItem> _items = new List<LineItem>();
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get;  set; }
    public IReadOnlyCollection<LineItem> Items => _items.ToList();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Guid? BillingAddressId { get; private set; }
    public Guid? ShippingAddressId { get; private set; }

    public Order(Guid id, Guid customerId, OrderStatus status, DateTime createdAt, DateTime updatedAt, Guid? billingAddressId, Guid? shippingAddressId)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        BillingAddressId = billingAddressId;
        ShippingAddressId = shippingAddressId;
    }

    private Order() { }

    public string GetStatus()
    {
        return Status.Value;
    }

    public static Order Create(Guid customerId, bool cart = false)
    {
        return new Order(Guid.NewGuid(), customerId, cart ? new CartStatus() : new CreatedStatus(), DateTime.UtcNow, DateTime.UtcNow, null, null);
    }

    public void AddItem(Guid productId, Money price, int quantity)
    {
        UpdatedAt = DateTime.UtcNow;

        var existingLineItem = _items.FirstOrDefault(li => li.ProductId == productId);

        if (existingLineItem != null)
        {
            existingLineItem.AddQuantity(quantity);
        }
        else
        {
            _items.Add(new LineItem(Guid.NewGuid(), Id, productId, price, quantity));
        }
    }


    public void RemoveItem(Guid lineItemId)
    {
        if (HasOneItem() && Status.Value != "cart") throw new CannotRemoveItem();

        var lineItem  = _items.FirstOrDefault(li => li.Id == lineItemId);

        if (lineItem == null) throw new LineItemNotFound();

        UpdatedAt = DateTime.UtcNow;

        if (lineItem.Quantity == 1)
        {
            _items.Remove(lineItem);
        }
        else
        {
            lineItem.DecreaseQuantity();
        }
    }

    public double CalculateTotal()
    {
        double total = 0;
        foreach (var lineItem in _items)
        {
            total += lineItem.Price.Amount * lineItem.Quantity;
        }

        return Math.Round(total, 2);
    }

    public void RestoreLineItens(ICollection<LineItem> lineItens)
    {
        foreach (var item in lineItens)
        {
            _items.Add(item);
        }
    }

    public Result Cancel()
    {
        Status.Cancel(this);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Checkout(Guid shippingAddressId, Guid billingAddressId, string paymentType, string? cardToken, int installments)
    {
        ShippingAddressId = shippingAddressId;
        BillingAddressId = billingAddressId;
        UpdatedAt = DateTime.UtcNow;
        Status.Checkout(this);

        Notify(new OrderPurchased(new OrderPurchasedData(
                Id,
                CalculateTotal(),
                Items.Select(li => new LineItemOrderPurchased(li.Id, li.ProductId, li.Quantity, li.Price.Amount)),
                CustomerId,
                paymentType,
                cardToken,
                installments,
                shippingAddressId
         )));

        return Result.Success();

    }

    public int CountItens()
    {
        var total = 0;
        foreach (var lineItem in _items)
        {
            total += lineItem.Quantity;
        }

        return total;
    }
    private bool HasOneItem() => _items.Count == 1;
}
