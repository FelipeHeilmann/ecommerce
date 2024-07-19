using Application.Abstractions.Messaging;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.ShipOrder;

public class ShipOrderCommandHandler : ICommandHandler<ShipOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public ShipOrderCommandHandler(IOrderRepository orderRepository) 
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if(order == null) return Result.Failure(OrderErrors.OrderNotFound);

        order.Ship();

        await _orderRepository.Update(order);

        return Result.Success();
    }
}
