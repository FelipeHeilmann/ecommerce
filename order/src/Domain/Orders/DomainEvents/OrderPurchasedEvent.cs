namespace Domain.Orders.DomainEvents;

public record OrderPurchasedEvent(
    Guid OrderId,
    double total,
    IEnumerable<LineItemOrderPurchasedEvent> Items, 
    Guid customerId,
    string CustomerName,
    string CustomerEmail,
    string CustomerDocument,
    string PaymentType,
    string? CardToken,
    int Installment, 
    string AddressZipCode,
    string AddressNumber,
    string? AddressLine
);

public record LineItemOrderPurchasedEvent(Guid Id, Guid ProductId, int Quantity, double Amount);
