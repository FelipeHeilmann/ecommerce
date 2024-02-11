using Application.Abstractions;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<Order>;
