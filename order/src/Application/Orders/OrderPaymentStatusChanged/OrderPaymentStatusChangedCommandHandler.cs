using Application.Abstractions.Messaging;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.OrderPaymentStatusChanged;

public class OrderPaymentStatusChangedCommandHandler : ICommandHandler<OrderPaymentStatusChangedCommand>
{
    private readonly IOrderRepository _orderRepository;

    public OrderPaymentStatusChangedCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(OrderPaymentStatusChangedCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (order == null) return Result.Failure(OrderErrors.OrderNotFound);

        if(command.Status == "approved")
        {
            order.ApprovPayment();
        }
        else if(command.Status == "rejected")
        {
            order.RejectPayment();
        }

        await _orderRepository.Update(order);

        return Result.Success();
    }
}
