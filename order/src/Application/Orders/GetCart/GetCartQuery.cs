using Application.Abstractions.Messaging;
using Domain.Orders.Entity;

namespace Application.Orders.GetCart;

public record GetCartQuery : IQuery<Output>;
public record Output(Guid Id, Guid CustomerId, string Status, IEnumerable<ItemsOutput> Items, Guid? BillingAddressId, Guid? ShippingAddressId);
public record ItemsOutput(Guid ProductId, double Price, int Quantity);


