using Domain.Abstractions;
using MediatR;

namespace Domain.Orders.Events;

public class OrderCheckedout : INotification, IDomainEvent
{
    public string EventName => "OrderCheckedout";

    public object Data { get; set; }

    public OrderCheckedout(Guid orderId,
        double total,
        IEnumerable<LineItemOrderCheckedout> items,
        Guid customerId,
        string paymentType,
        string? cardToken,
        int installment,
        Guid addressId)
    {
        Data = new OrderCheckedoutData(orderId, total, items, customerId, paymentType, cardToken, installment, addressId);
    }
}


public record OrderCheckedoutData(
    Guid OrderId,
    double Total,
    IEnumerable<LineItemOrderCheckedout> Items,
    Guid CustomerId,
    string PaymentType,
    string? CardToken,
    int Installment,
    Guid AddressId
);

public record LineItemOrderCheckedout(Guid Id, Guid ProductId, int Quantity, double Price);
