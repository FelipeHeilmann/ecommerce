using Application.Abstractions.Messaging;
using Domain.Orders.Entity;

namespace Application.Orders.GetCart;

public record GetCartQuery : IQuery<Order>;

