using Application.Abstractions.Messaging;

namespace Application.Orders.AddItemToCart;

public record AddItemToCartCommand(Guid CustomerId, Guid ProductId, int Quantity) : ICommand;
