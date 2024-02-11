using Application.Abstractions.Messaging;
using Application.Orders.Model;
using Domain.Orders;

namespace Application.Orders.Create;

public record class CreateOrderCommand(OrderRequestModel request) : ICommand<Order>;
