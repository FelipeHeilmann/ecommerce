using Application.Abstractions;
using Domain.Orders;

namespace Application.Orders.Query.GetByCustomerId;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<ICollection<Order>>;
