using Application.Abstractions;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.RemoveItem;

public record RemoveLineItemCommand(Guid OrderId, Guid LineItemId) : ICommand<Result<Order>>;
