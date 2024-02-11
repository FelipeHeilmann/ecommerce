using Application.Abstractions.Messaging;

namespace Application.Orders.AddLineItem;

public record AddLineItemCommand(Guid OrderId, Guid ProductId, int Quantity) : ICommand;
