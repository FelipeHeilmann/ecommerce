using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.RemoveItem;

public class RemoveLineItemCommandHandler : ICommandHandler<RemoveLineItemCommand, Order>
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveLineItemCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Order>> Handle(RemoveLineItemCommand command, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        var removed = order.RemoveItem(command.LineItemId);

        if (removed.IsFailure) return Result.Failure<Order>(removed.Error);

        _repository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order);
    }

}
