using MediatR;

namespace Domain.Events;
public class OrderPurchasedEvent : INotification
{
    public Guid OrderId { get; set; }
    public double Total { get; set; }
    public IEnumerable<LineItemOrderPurchasedEvent> Items { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerDocument { get; set; }
    public string CustomerPhone { get; set; }
    public string PaymentType { get; set; }
    public string? CardToken { get; set; }
    public int Installment { get; set; }
    public string AddressZipCode { get; set; }
    public string AddressNumber { get; set; }
    public string? AddressLine { get; set; }

    public OrderPurchasedEvent(Guid orderId, double total, IEnumerable<LineItemOrderPurchasedEvent> items, Guid customerId, string customerName, string customerEmail, string customerDocument, string customerPhone, string paymentType, string? cardToken, int installment, string addressZipCode, string addressNumber, string? addressLine)
    {
        OrderId = orderId;
        Total = total;
        Items = items;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        CustomerDocument = customerDocument;
        CustomerPhone = customerPhone;
        PaymentType = paymentType;
        CardToken = cardToken;
        Installment = installment;
        AddressZipCode = addressZipCode;
        AddressNumber = addressNumber;
        AddressLine = addressLine;
    }
}

public record LineItemOrderPurchasedEvent(Guid Id, Guid ProductId, int Quantity, double Amount);
