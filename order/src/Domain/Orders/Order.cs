using Domain.Products;

namespace Domain.Orders;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<LineItem> Itens => _itens.ToList();
    private readonly ICollection<LineItem> _itens = new List<LineItem>();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Order(Guid id, Guid customerId, OrderStatus status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Order Create(Guid customerId)
    {
        return new Order(Guid.NewGuid(), customerId, OrderStatus.Created, DateTime.Now, DateTime.Now);
    }

    public LineItem AddItem(Product product, int quantity)
    {
        var lineItem = new LineItem(Guid.NewGuid(), product.Id, product.Price ,quantity, product);

        _itens.Add(lineItem);

        UpdatedAt = DateTime.Now;
        
        return lineItem;

    }

    public void RemoveItem(Guid lineItemId) 
    {
        if (HasOneItem()) return;

        var lineItem = _itens.FirstOrDefault(item => item.Id == lineItemId);

        if (lineItem == null) return;

        if(lineItem.DeleteItem()) _itens.Remove(lineItem);

        UpdatedAt = DateTime.Now;
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

    public void Process()
    {
        UpdatedAt = DateTime.Now;
        Status = OrderStatus.WaitingPayment;
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
