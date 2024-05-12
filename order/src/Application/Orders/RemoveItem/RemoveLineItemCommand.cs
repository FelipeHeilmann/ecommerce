using Application.Abstractions.Messaging;

namespace Application.Orders.RemoveItem;

public record RemoveLineItemCommand(Guid LineItemId) : ICommand;
