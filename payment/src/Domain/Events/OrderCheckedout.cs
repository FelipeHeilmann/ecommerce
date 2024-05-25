using MediatR;

namespace Domain.Events;
public class OrderCheckedout : INotification
{
    public Guid OrderId { get; set; }
    public double Total { get; set; }
    public IEnumerable<LineItemOrderCheckedout> Items { get; set; }
    public Guid CustomerId { get; set; }
    public string PaymentType { get; set; }
    public string? CardToken { get; set; }
    public int Installment { get; set; }
    public Guid AddressId { get; set; }



    public OrderCheckedout(Guid orderId, double total, IEnumerable<LineItemOrderCheckedout> items, Guid customerId, string paymentType, string? cardToken, int installment, Guid addressId)
    {
        OrderId = orderId;
        Total = total;
        Items = items;
        CustomerId = customerId;
        PaymentType = paymentType;
        CardToken = cardToken;
        Installment = installment;
        AddressId = addressId;
    }
}

public record LineItemOrderCheckedout(Guid Id, Guid ProductId, int Quantity, double Price);
