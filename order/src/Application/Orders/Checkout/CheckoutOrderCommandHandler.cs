using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Shared;
using Application.Abstractions.Queue;
using Domain.Orders.Repository;
using Domain.Orders.Error;
using Domain.Addresses.Error;
using Domain.Addresses.Repository;
using Domain.Customers.Repository;

namespace Application.Orders.Checkout;

public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IQueue _queue;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutOrderCommandHandler
    (
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork,
        IQueue queue)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _queue = queue;
    }

    public async Task<Result> Handle(CheckoutOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure(OrderErrors.OrderNotFound);

        var billingAddress = await _addressRepository.GetByIdAsync(command.BillingAddressId, cancellationToken);

        if (billingAddress == null) return Result.Failure(AddressErrors.NotFound);

        var shippingAddress = await _addressRepository.GetByIdAsync(command.ShippingAddressId, cancellationToken);
        
        if (shippingAddress == null) return Result.Failure(AddressErrors.NotFound);

        order.Register("OrderPurchased", async domainEvent =>
        {
            await _queue.PublishAsync(domainEvent.Data, "order.purchased");
        });
        
        order.Checkout(shippingAddress.Id, billingAddress.Id, command.PaymentType, command.CardToken, command.Installments);

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
      
        return Result.Success();
    }
}
