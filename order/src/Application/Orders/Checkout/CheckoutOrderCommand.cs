using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.Checkout;

public record CheckoutOrderCommand(Guid OrderId, Guid ShippingAddressId, Guid BillingAddressId) : ICommand<Order>;
