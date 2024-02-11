using Application.Abstractions;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetById;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, Order>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Order>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(query.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        return Result.Success(order);
    }
}
