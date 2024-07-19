using Application.Abstractions.Messaging;

namespace Application.Orders.ShipOrder;

public record ShipOrderCommand(Guid OrderId) : ICommand;