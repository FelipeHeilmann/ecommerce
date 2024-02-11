using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.RemoveItem;

public record RemoveLineItemCommand(Guid OrderId, Guid LineItemId) : ICommand<Order>;
