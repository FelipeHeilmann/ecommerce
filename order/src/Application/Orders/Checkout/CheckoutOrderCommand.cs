using Application.Abstractions.Messaging;

namespace Application.Orders.Checkout;

public record CheckoutOrderCommand(List<CheckoutItem> OrderItens, Guid CustomerId, Guid ShippingAddressId, Guid BillingAddressId, string? CouponName, string PaymentType, string? CardToken, int Installments) : ICommand<Guid>;

public record CheckoutItem(Guid ProductId, int Quantity);