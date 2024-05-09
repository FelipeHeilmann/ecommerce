﻿using Domain.Abstractions;
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

    public Order() { }

    public string GetStatus()
    {
        return Status.Value;
    }

    public static Order Create(Guid customerId)
    {
        return new Order(Guid.NewGuid(), customerId, new CreatedStatus(), DateTime.UtcNow, DateTime.UtcNow, null, null);
    }

    public LineItem AddItem(Guid productId, Money price, int quantity)
    {
        UpdatedAt = DateTime.UtcNow;

        var existLineItemIndex = Items.ToList().FindIndex(li => li.ProductId == productId);

        if (existLineItemIndex != -1)
        {
            Items.ToList()[existLineItemIndex].AddQuantity();

            return Items.ToList()[existLineItemIndex];
        }

        var lineItem = new LineItem(Guid.NewGuid(), Id, productId, price, quantity);

        _items.Add(lineItem);


        return lineItem;

    }

    public Result<LineItem> RemoveItem(Guid lineItemId)
    {
        if (HasOneItem()) return Result.Failure<LineItem>(OrderErrors.OrderHasOneLineItem);

        var lineItem = _items.FirstOrDefault(item => item.Id == lineItemId);

        if (lineItem == null) return Result.Failure<LineItem>(OrderErrors.LineItemNotFound);

        UpdatedAt = DateTime.UtcNow;

        var deleted = lineItem.DeleteItem();
        if (deleted)
        {
            _items.Remove(lineItem);
        }

        return Result.Success(lineItem);
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