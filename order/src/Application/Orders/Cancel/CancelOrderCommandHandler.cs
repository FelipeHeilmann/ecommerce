using Application.Abstractions.Messaging;
using Application.Abstractions.Queue;
using Application.Data;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.Cancel;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, Order>
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueue _queue;

    public CancelOrderCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork, IQueue queue)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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

        _repository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order);
    }
}
