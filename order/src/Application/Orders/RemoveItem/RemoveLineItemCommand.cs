using Application.Abstractions.Messaging;
using Domain.Orders.Entity;

namespace Application.Orders.RemoveItem;

public record RemoveLineItemCommand(Guid OrderId, Guid LineItemId) : ICommand<Order>;
