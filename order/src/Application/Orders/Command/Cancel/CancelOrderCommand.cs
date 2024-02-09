using Application.Abstractions;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Command.Cancel;

public record CancelOrderCommand(Guid OrderId) : ICommand<Result<Order>>;
