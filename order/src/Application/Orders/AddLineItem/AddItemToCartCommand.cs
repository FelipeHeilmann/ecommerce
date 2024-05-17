using Application.Abstractions.Messaging;

namespace Application.Orders.AddLineItem;

public record AddItemToCartCommand(Guid CustomerId, Guid ProductId, int Quantity) : ICommand;
