using Application.Abstractions.Messaging;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<Output>;

public record Output(Guid Id, Guid CustomerId, string Status, IEnumerable<ItemsOutput> Items, double Total, Guid? BillingAddressId, Guid? ShippingAddressId);
public record ItemsOutput (Guid ProductId, double Price ,int Quantity);

