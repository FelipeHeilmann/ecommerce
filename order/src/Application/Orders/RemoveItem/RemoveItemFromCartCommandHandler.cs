using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.RemoveItem;

public class RemoveItemFromCartCommandHandler : ICommandHandler<RemoveItemFromCartCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveItemFromCartCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
    {
        _orderRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetCart(cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        order.RemoveItem(command.LineItemId);

        if (order.Items.Count == 0)
        {
            _orderRepository.Delete(order);
        }
        else
        {
            _orderRepository.Update(order);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
