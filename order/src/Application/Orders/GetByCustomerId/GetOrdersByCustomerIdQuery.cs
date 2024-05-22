using Application.Abstractions.Messaging;
using Application.Abstractions.Query;

namespace Application.Orders.GetByCustomerId;

public record GetOrdersByCustomerIdQuery(Guid CustomerId) : IQuery<ICollection<OrderQueryModel>>;

public record Output(Guid Id, Guid CustomerId, string Status, IEnumerable<ItemsOutput> Items, Guid? BillingAddressId, Guid? ShippingAddressId);
public record ItemsOutput(Guid ProductId, double Price, int Quantity);
