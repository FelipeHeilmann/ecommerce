using Application.Abstractions.Messaging;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.Delivery;

public class DeliveryOrderCommandHandler : ICommandHandler<DeliveryOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public DeliveryOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(DeliveryOrderCommand request, CancellationToken cancellationToken)
    {
       var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

       if(order == null) return Result.Failure(OrderErrors.OrderNotFound);

       order.Delivery();

       await _orderRepository.Update(order);

       return Result.Success();
    }
}
