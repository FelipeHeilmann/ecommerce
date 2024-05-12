using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.RemoveItem;

public class RemoveLineItemCommandHandler : ICommandHandler<RemoveLineItemCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveLineItemCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
    {
        _orderRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveLineItemCommand command, CancellationToken cancellationToken)
    {
        var cart = await _orderRepository.GetCart(cancellationToken, "Items");

        if (cart == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        cart.RemoveItem(command.LineItemId);

        if (cart.Items.Count == 0) _orderRepository.Delete(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
