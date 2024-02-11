using Application.Abstractions;
using Domain.Orders;

namespace Application.Orders.GetCart;

public record GetCartQuery : IQuery<Order>;

