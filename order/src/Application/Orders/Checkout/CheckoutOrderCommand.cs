using Application.Abstractions.Messaging;

namespace Application.Orders.Checkout;

public record CheckoutOrderCommand(Guid OrderId, Guid ShippingAddressId, Guid BillingAddressId, string PaymentType, string? CardToken, int Installments) : ICommand;
