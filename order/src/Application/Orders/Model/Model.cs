namespace Application.Orders.Model;

public record AddItemRequest(Guid ProductId, int Quantity);
public record CheckoutOrderRequest(List<OrderItemRequest> Items, string PaymentType, int Installments, string? CardToken, Guid ShippingAddressId, Guid BillingAddressId);
public record OrderItemRequest(Guid ProductId, int Quantity);


