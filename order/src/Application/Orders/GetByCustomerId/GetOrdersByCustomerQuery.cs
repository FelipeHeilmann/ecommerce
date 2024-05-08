using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.GetByCustomerId;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<ICollection<Output>>;

public record Output(Guid Id, Guid CustomerId, string Status, IEnumerable<ItemsOutput> Items, Guid? BillingAddressId, Guid? ShippingAddressId);
public record ItemsOutput(Guid ProductId, double Price, int Quantity);
