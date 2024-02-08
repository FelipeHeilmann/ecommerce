using Application.Abstractions;
using Application.Orders.Model;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Command;

public record class CreateOrderCommand(OrderRequestModel request) : ICommand<Result<Order>>;
