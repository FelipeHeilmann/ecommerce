using Application.Abstractions.Messaging;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetById;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, Output>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Output>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(query.OrderId, cancellationToken, "Items");

        if (order == null) return Result.Failure<Output>(OrderErrors.OrderNotFound);

        return new Output(
            order.Id,
            order.CustomerId,
            order.Status.ToString(),
            order.Items.Select(line => new ItemsOutput(line.ProductId, line.Price.Amount, line.Quantity)),
            order.BillingAddressId,
            order.ShippingAddressId
        );
    }
}
