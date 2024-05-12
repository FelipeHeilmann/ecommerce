using Application.Abstractions.Messaging;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.GetCart;

public class GetCartQueryHandler : IQueryHandler<GetCartQuery, Output?>
{
    private readonly IOrderRepository _orderRepository;

    public GetCartQueryHandler(IOrderRepository repository)
    {
        _orderRepository = repository;
    }

    public async Task<Result<Output?>> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        var cart = await _orderRepository.GetCart(cancellationToken, "Items");

        return new Output(
            cart?.Id ,
            cart?.CustomerId,
            cart?.GetStatus(),
            cart?.Items?.Select(line => new ItemsOutput(line.Id ,line.ProductId, line.Price.Amount, line.Quantity)),
            cart?.CalculateTotal(),
            cart?.BillingAddressId,
            cart?.ShippingAddressId
        );
    }
}
