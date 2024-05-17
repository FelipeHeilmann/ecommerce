using Application.Abstractions.Messaging;

namespace Application.Orders.RemoveItem;

public record RemoveItemFromCartCommand(Guid LineItemId) : ICommand;
