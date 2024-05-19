using Application.Abstractions.Messaging;

namespace Application.Orders.RemoveItemRemoveItemFromCart;

public record RemoveItemFromCartCommand(Guid LineItemId) : ICommand;
