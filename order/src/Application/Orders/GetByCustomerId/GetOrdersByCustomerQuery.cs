using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.GetByCustomerId;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<ICollection<Order>>;
