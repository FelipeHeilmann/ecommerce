using Domain.Abstractions;

namespace Domain.Orders.Events;

public class OrderPurchased : IDomainEvent
{
    public string EventName => "OrderPurchased";

    public object Data { get; set; }

    public OrderPurchased(OrderPurchasedData data)
    {
        Data = data;
    }
}


public record OrderPurchasedData(
    Guid OrderId,
    double Total,
    IEnumerable<LineItemOrderPurchased> Items,
    Guid CustomerId,
    string PaymentType,
    string? CardToken,
    int Installment,
    Guid AddressId
);

public record LineItemOrderPurchased(Guid Id, Guid ProductId, int Quantity, double Amount);
