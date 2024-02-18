namespace Domain.Orders.DomainEvents;

public record OrderCompletedEvent(
    Guid OrderId,
    double total,
    IEnumerable<LineItemOrderCompletedEvent> Items, 
    string CustomerName,
    string CustomerEmail,
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

public record LineItemOrderCompletedEvent(Guid Id, Guid ProductId, int Quantity, double Amount);
