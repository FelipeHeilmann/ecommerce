using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Checkout;

public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Order>> Handle(CheckoutOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        order.Checkout();

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order);
    }
}
