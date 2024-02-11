using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.Cancel;

public record CancelOrderCommand(Guid OrderId) : ICommand<Order>;
