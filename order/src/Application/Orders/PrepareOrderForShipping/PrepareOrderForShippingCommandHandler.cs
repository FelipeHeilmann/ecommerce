using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.PrepareOrderForShipping;

public class PrepareOrderForShippingCommandHandler: ICommandHandler<PrepareOrderForShippingCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PrepareOrderForShippingCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PrepareOrderForShippingCommand command, CancellationToken cancellationToken)
    {
       var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
       if (order == null) return Result.Failure(OrderErrors.OrderNotFound);
       order.Prepare();
       _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
