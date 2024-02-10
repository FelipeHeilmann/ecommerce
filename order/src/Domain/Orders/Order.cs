using Domain.Products;
using Domain.Shared;

namespace Domain.Orders;

public class Order
{
    private readonly ICollection<LineItem> _itens = new List<LineItem>();
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<LineItem> Itens => _itens.ToList();
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
        return new Order(Guid.NewGuid(), customerId,OrderStatus.Created, DateTime.Now, DateTime.Now);
    }

    public LineItem AddItem(Guid productId, Money price ,int quantity)
    {
        var lineItem = new LineItem(Guid.NewGuid(), Id ,productId, price ,quantity);

        _itens.Add(lineItem);

        UpdatedAt = DateTime.Now;
        
        return lineItem;

    }

    public Result<LineItem> RemoveItem(Guid lineItemId) 
    {
        if (HasOneItem()) return Result.Failure<LineItem>(OrderErrors.OrderHasOneLineItem);

        var lineItem = _itens.FirstOrDefault(item => item.Id == lineItemId);

        if (lineItem == null) return Result.Failure<LineItem>(OrderErrors.LineItemNotFound);
            
        UpdatedAt = DateTime.Now;

        var deleted = lineItem.DeleteItem();
        if (deleted) { 
            _itens.Remove(lineItem);
        }

        return Result.Success(lineItem);
    }

    public double CalculateTotal()
    {
        double total = 0;
        foreach (var lineItem in _itens)
        {
            total += lineItem.Price.Amount * lineItem.Quantity;
        }

        return total;
    }

    public void RestoreLineItens(ICollection<LineItem> lineItens)
    {
        foreach (var item in lineItens)
        {
            _itens.Add(item);
        }
    }

    public Result Cancel()
    {
        if (CustomerId == null) return Result.Failure(OrderErrors.OrderDoesnotHaveCustomerId);
        Status = OrderStatus.Canceled;
        UpdatedAt = DateTime.Now;

        return Result.Success();
    }

    public Result Process(Guid customerId)
    {
        if(Status != OrderStatus.Created) return Result.Failure(OrderErrors.OrderStatusCouldNotBeProccessed);
        UpdatedAt = DateTime.Now;
        Status = OrderStatus.WaitingPayment;
        CustomerId = customerId;

        return Result.Success();

    }

    public int CountItens()
    {
        var itensTotal = 0;
        foreach (var lineItem in _itens)
        {
            itensTotal += lineItem.Quantity;
        }

        return itensTotal;
    }
    private bool HasOneItem() => _itens.Count == 1;
}
