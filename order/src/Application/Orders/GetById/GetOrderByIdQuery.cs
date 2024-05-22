using Application.Abstractions.Messaging;
using Application.Abstractions.Query;

namespace Application.Orders.GetById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderQueryModel>;
