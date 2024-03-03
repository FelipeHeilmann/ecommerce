using Application.Abstractions.Messaging;
using Application.Orders.Model;
using Domain.Orders;

namespace Application.Orders.Checkout;

public record CheckoutOrderCommand(Guid OrderId, Guid ShippingAddressId, Guid BillingAddressId, string PaymentType, string? CardToken, int Installments) : ICommand<PaymentSystemTransactionResponse>;
