namespace API.Events;

public record OrderPurchasedEvent(
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
