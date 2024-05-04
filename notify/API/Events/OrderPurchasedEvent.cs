namespace API.Events;

public record OrderPurchasedEvent(
Guid OrderId,
double total,
IEnumerable<LineItemOrderPurchased> Items,
Guid CustomerId,
string CustomerName,
string CustomerEmail,
string CustomerDocument,
string CustomerPhone,
string PaymentType,
string? CardToken,
int Installment,
string AddressZipCode,
string AddressNumber,
string? AddressLine
);

public record LineItemOrderPurchased(Guid Id, Guid ProductId, int Quantity, double Amount);
