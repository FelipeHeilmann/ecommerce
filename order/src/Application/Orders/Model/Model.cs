namespace Application.Orders.Model;

public record OrderRequest(List<OrderItemRequest> OrderItens, Guid CustomerId);

public record OrderItemRequest(Guid ProductId, int Quantity);

public record AddItemRequest(Guid ProductId, int Quantity);

public record CheckoutOrderRequest(string PaymentType, int Installments, string? CardToken, Guid ShippingAddressId, Guid BillingAddressId);

public record PaymentSystemTransactionResponse(Guid OrderId, string? PaymentUrl);


