using Application.Abstractions;
using Domain.Orders;

namespace Application.Orders.Query.GetCart;

public record GetCartQuery : IQuery<Order>;

