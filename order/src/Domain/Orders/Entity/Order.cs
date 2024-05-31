using Domain.Abstractions;
using Domain.Orders.Error;
using Domain.Orders.Event;
using Domain.Orders.Events;
using Domain.Orders.VO;
using Domain.Products.VO;

namespace Domain.Orders.Entity;

public class Order : Observable
{
    private ICollection<LineItem> _items;
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Status => _status.Value;
    public OrderStatus _status { private get; set; }
    public IReadOnlyCollection<LineItem> Items => _items.ToList();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Guid? BillingAddressId { get; private set; }
    public Guid? ShippingAddressId { get; private set; }

    public Order(Guid id, Guid customerId, string status, ICollection<LineItem> items, DateTime createdAt, DateTime updatedAt, Guid? billingAddressId, Guid? shippingAddressId)
    {
        Id = id;
        CustomerId = customerId;
        _status =  OrderStatusFactory.Create(this,status);
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        BillingAddressId = billingAddressId;
        ShippingAddressId = shippingAddressId;
        _items = items;
    }


    public static Order Create(Guid customerId, bool cart = false)
    {
        return new Order(Guid.NewGuid(), customerId, cart ? "cart" : "created", new List<LineItem>(), DateTime.UtcNow, DateTime.UtcNow, null, null);
    }

    public void AddItem(Guid productId, Money price, int quantity)
    {
        UpdatedAt = DateTime.UtcNow;

        var existingLineItem = Items.FirstOrDefault(li => li.ProductId == productId);

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
        if (HasOneItem() && Status != "cart") throw new CannotRemoveItem();

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

    public void Cancel()
    {
        _status.Cancel();
        UpdatedAt = DateTime.UtcNow;
        Notify(new OrderCancelled(new OrderCancelledData(Id)));
    }

    public void Checkout(Guid shippingAddressId, Guid billingAddressId, string paymentType, string? cardToken, int installments)
    {
        ShippingAddressId = shippingAddressId;
        BillingAddressId = billingAddressId;
        UpdatedAt = DateTime.UtcNow;
        _status.Checkout();

         Notify(new OrderCheckedout(
              Id,
              CalculateTotal(),
              Items.Select(li => new LineItemOrderCheckedout(li.Id, li.ProductId, li.Quantity, li.Price.Amount)),
              CustomerId,
              paymentType,
              cardToken,
              installments,
              shippingAddressId
         ));
    }

    public void ApprovPayment()
    {
        _status.Approve();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RejectPayment()
    {
        _status.Reject();
        UpdatedAt = DateTime.UtcNow;
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
