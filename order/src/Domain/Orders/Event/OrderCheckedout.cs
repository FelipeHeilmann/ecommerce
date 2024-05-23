using Domain.Abstractions;
using MediatR;

namespace Domain.Orders.Events;

public class OrderCheckedout : INotification, IDomainEvent
{
    public string EventName => "OrderCheckedout";

    public object Data { get; set; }

    public OrderCheckedout(OrderCheckedoutData data)
    {
        Data = data;
    }
}


public record OrderCheckedoutData(
    Guid OrderId,
    double Total,
    IEnumerable<LineItemOrderPurchased> Items,
    Guid CustomerId,
    string PaymentType,
    string? CardToken,
    int Installment,
    Guid AddressId
);

public record LineItemOrderPurchased(Guid Id, Guid ProductId, int Quantity, double Price);
