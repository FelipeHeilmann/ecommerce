using Application.Abstractions.Messaging;
using Application.Orders.Model;

namespace Application.Orders.Checkout;

public record CheckoutOrderCommand(List<OrderItemRequest> OrderItens, Guid CustomerId, Guid ShippingAddressId, Guid BillingAddressId, string? CouponName, string PaymentType, string? CardToken, int Installments) : ICommand<Guid>;
