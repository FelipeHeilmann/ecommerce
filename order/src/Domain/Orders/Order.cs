using Domain.Products;
using Domain.Shared;

namespace Domain.Orders;

public class Order
{
    private readonly ICollection<LineItem> _items = new List<LineItem>();
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<LineItem> Items => _items.ToList();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Order(Guid id, Guid customerId ,OrderStatus status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Order Create(Guid customerId)
    {
        return new Order(Guid.NewGuid(), customerId,OrderStatus.Created, DateTime.UtcNow, DateTime.UtcNow);
    }

    public LineItem AddItem(Guid productId, Money price ,int quantity)
    {
        UpdatedAt = DateTime.UtcNow;
        
        var existLineItemIndex = Items.ToList().FindIndex(li => li.ProductId == productId);

        if (existLineItemIndex != -1)
        {
            Items.ToList()[existLineItemIndex].AddQuantity();

            return Items.ToList()[existLineItemIndex];
        }

        var lineItem = new LineItem(Guid.NewGuid(), Id ,productId, price ,quantity);

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
        if (deleted) { 
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

        return total;
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
        if (CustomerId == null) return Result.Failure(OrderErrors.OrderDoesnotHaveCustomerId);
        Status = OrderStatus.Canceled;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Process(Guid customerId)
    {
        if(Status != OrderStatus.Created) return Result.Failure(OrderErrors.OrderStatusCouldNotBeProccessed);
        UpdatedAt = DateTime.UtcNow;
        Status = OrderStatus.WaitingPayment;
        CustomerId = customerId;

        return Result.Success();

    }

    public int CountItens()
    {
        var itensTotal = 0;
        foreach (var lineItem in _items)
        {
            itensTotal += lineItem.Quantity;
        }

        return itensTotal;
    }
    private bool HasOneItem() => _items.Count == 1;
}
