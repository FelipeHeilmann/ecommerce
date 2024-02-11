using Application.Abstractions.Messaging;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<Order>;
