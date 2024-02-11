using Application.Abstractions.Messaging;
using Application.Orders.Model;

namespace Application.Orders.Create;

public record class CreateOrderCommand(OrderRequestModel request) : ICommand<Guid>;
