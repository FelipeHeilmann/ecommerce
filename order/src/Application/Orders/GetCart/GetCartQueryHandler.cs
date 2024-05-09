using Application.Abstractions.Messaging;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.GetCart;

public class GetCartQueryHandler : IQueryHandler<GetCartQuery, Output>
{
    private readonly IOrderRepository _repository;

    public GetCartQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Output>> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetAllAsync(cancellationToken);

        var cart = orders.FirstOrDefault(o => o.GetStatus() == "created");

        if (cart == null) return Result.Failure<Output>(OrderErrors.CartNotFound);

        return new Output(
            cart.Id,
            cart.CustomerId,
            cart.GetStatus(),
            cart.Items.Select(line => new ItemsOutput(line.ProductId, line.Price.Amount, line.Quantity)),
            cart.BillingAddressId,
            cart.ShippingAddressId
        );
    }
}
