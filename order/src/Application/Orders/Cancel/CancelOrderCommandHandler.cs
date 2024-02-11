using Application.Abstractions;
using Application.Data;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Cancel;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, Order>
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Order>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        order.Cancel();

        _repository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order);
    }
}
