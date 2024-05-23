using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Event;
using Domain.Orders.Repository;
using Domain.Shared;
using MediatR;

namespace Application.Orders.Cancel;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, Order>
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CancelOrderCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork, IMediator mediator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<Order>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        order.Cancel();

        _repository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new OrderCanceledEvent(order.Id));

        return Result.Success(order);
    }
}
