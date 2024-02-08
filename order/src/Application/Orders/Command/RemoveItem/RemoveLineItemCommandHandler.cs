using Application.Abstractions;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Command.RemoveItem;

public class RemoveLineItemCommandHandler : ICommandHandler<RemoveLineItemCommand, Result<Order>>
{
    private readonly IOrderRepository _repository;

    public RemoveLineItemCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Order>> Handle(RemoveLineItemCommand command, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(command.OrderId);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        var removed = order.RemoveItem(command.LineItemId);

        if(removed.IsFailure) return Result.Failure<Order>(removed.Error);
        _repository.Update(order);

        return Result.Success(order);
    }

}
