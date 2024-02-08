using Application.Abstractions;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Query.GetById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<Order>;
