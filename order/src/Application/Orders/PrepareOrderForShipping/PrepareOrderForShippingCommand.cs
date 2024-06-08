using Application.Abstractions.Messaging;

namespace Application.Orders.PrepareOrderForShipping;

public record PrepareOrderForShippingCommand(Guid OrderId): ICommand;
