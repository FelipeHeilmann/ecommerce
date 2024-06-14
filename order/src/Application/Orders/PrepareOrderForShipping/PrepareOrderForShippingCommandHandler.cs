using Application.Abstractions.Messaging;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.PrepareOrderForShipping;

public class PrepareOrderForShippingCommandHandler: ICommandHandler<PrepareOrderForShippingCommand>
{
    private readonly IOrderRepository _orderRepository;

    public PrepareOrderForShippingCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(PrepareOrderForShippingCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (order == null) return Result.Failure(OrderErrors.OrderNotFound);
        order.Prepare();
        await _orderRepository.Update(order);
        return Result.Success();
    }
}
