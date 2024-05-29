using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.OrderPaymentStatusChanged;

public class OrderPaymentStatusChangedCommandHandler : ICommandHandler<OrderPaymentStatusChangedCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderPaymentStatusChangedCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
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

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
