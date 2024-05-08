using Application.Abstractions.Messaging;
using Domain.Orders.Entity;

namespace Application.Orders.Cancel;

public record CancelOrderCommand(Guid OrderId) : ICommand<Order>;
