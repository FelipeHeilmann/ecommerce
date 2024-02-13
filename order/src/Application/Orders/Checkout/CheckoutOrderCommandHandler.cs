using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Addresses;
using Domain.Orders;
using Domain.Orders.DomainEvents;
using Domain.Shared;
using MassTransit;

namespace Application.Orders.Checkout;

public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IAddressRepository addressRepository, IUnitOfWork unitOfWork, IPublishEndpoint publisher)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _addressRepository = addressRepository;
    }

    public async Task<Result<Order>> Handle(CheckoutOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Order>(OrderErrors.OrderNotFound);

        var billingAddress = await _addressRepository.GetByIdAsync(command.BillingAddressId, cancellationToken);

        if (billingAddress == null) return Result.Failure<Order>(AddressErrors.NotFound);

        var shippingAddress = await _addressRepository.GetByIdAsync(command.ShippingAddressId, cancellationToken);
        
        if (shippingAddress == null) return Result.Failure<Order>(AddressErrors.NotFound);

        var checkout = order.Checkout(shippingAddress.Id, billingAddress.Id);

        if(checkout.IsFailure) return Result.Failure<Order>(checkout.Error);

        _orderRepository.Update(order);

        await _publisher.Publish(new OrderCheckoutedEvent(order.Id, order.CustomerId, DateTime.UtcNow, order.CalculateTotal()));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order);
    }
}
