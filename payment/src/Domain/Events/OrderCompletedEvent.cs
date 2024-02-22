namespace Domain.Events;
public record OrderPurchasedEvent(
    Guid OrderId,
    double total,
    IEnumerable<LineItemOrderPurchasedEvent> Items,
    Guid CustomerId,
    string CustomerName,
    string CustomerEmail,
    string CustomerDocument,
    string CustomerPhone, 
    string PaymentType,
    string? CardToken,
    int Installment,
    string BillingAddressZipCode,
    string BillingAddressNumber,
    string? BillingAddressLine,
    string ShippingAddressZipCode,
    string ShippingAddressNumber,
    string? ShippingAddressLine
   );

public record LineItemOrderPurchasedEvent(Guid Id, Guid ProductId, int Quantity, double Amount);
