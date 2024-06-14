using Application.Abstractions.Messaging;
using Application.Abstractions.Queue;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.Cancel;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, Order>
{
    private readonly IOrderRepository _repository;
    private readonly IQueue _queue;

    public CancelOrderCommandHandler(IOrderRepository repository, IQueue queue)
    {
        _repository = repository;
        _queue = queue;
    }

    public async Task<Result<Order>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        order.Register("OrderCancelled", async domainEvent => {
            await _queue.PublishAsync(domainEvent.Data, "order.canceled");
        });

        order.Cancel();

        await _repository.Update(order);

        return Result.Success(order);
    }
}
