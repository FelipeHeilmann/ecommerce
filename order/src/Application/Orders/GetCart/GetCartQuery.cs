using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.GetCart;

public record GetCartQuery : IQuery<Order>;

