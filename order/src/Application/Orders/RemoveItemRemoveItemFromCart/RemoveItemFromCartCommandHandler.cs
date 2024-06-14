using Application.Abstractions.Messaging;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.RemoveItemRemoveItemFromCart;

public class RemoveItemFromCartCommandHandler : ICommandHandler<RemoveItemFromCartCommand>
{
    private readonly IOrderRepository _orderRepository;

    public RemoveItemFromCartCommandHandler(IOrderRepository repository)
    {
        _orderRepository = repository;
    }

    public async Task<Result> Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetCart(cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        order.RemoveItem(command.LineItemId);

        if (order.Items.Count == 0)
        {
            await _orderRepository.Delete(order);
        }
        else
        {
            await _orderRepository.Update(order);
        }

        return Result.Success();
    }

}
