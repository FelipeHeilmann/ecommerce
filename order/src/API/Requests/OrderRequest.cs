namespace API.Requests;

public record AddItemRequest(Guid ProductId, int Quantity);
public record CheckoutOrderRequest(List<OrderItemRequest> Items, string PaymentType, string? CouponName ,int Installments, string? CardToken, Guid ShippingAddressId, Guid BillingAddressId);
public record OrderItemRequest(Guid ProductId, int Quantity);


