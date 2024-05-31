using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Shared;
using Domain.Orders.Repository;
using Domain.Addresses.Error;
using Domain.Addresses.Repository;
using Domain.Customers.Repository;
using Domain.Customers.Error;
using Domain.Orders.Entity;
using Domain.Products.Error;
using Domain.Products.Repository;
using MediatR;
using Domain.Orders.Events;
using Application.Abstractions.Queue;

namespace Application.Orders.Checkout;

public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IProductRepository _productRepository;
    private readonly IQueue _queue;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutOrderCommandHandler
    (
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork,
        IQueue queue)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _addressRepository = addressRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _queue = queue;
    }

    public async Task<Result<Guid>> Handle(CheckoutOrderCommand command, CancellationToken cancellationToken)
    {
        var orderItemList = command.OrderItens;

        var order = Order.Create(command.CustomerId);

        var customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);

        if (customer == null)
        {
            return Result.Failure<Guid>(CustomerErrors.CustomerNotFound);
        }

        foreach (var item in orderItemList)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
            {
                return Result.Failure<Guid>(ProductErrors.ProductNotFound);
            }
            order.AddItem(item.ProductId, product.Price, item.Quantity);
        }

        var billingAddress = await _addressRepository.GetByIdAsync(command.BillingAddressId, cancellationToken);

        if (billingAddress == null) return Result.Failure<Guid>(AddressErrors.NotFound);

        var shippingAddress = await _addressRepository.GetByIdAsync(command.ShippingAddressId, cancellationToken);
        
        if (shippingAddress == null) return Result.Failure<Guid>(AddressErrors.NotFound);

        order.Register("OrderCheckedout", async domainEvent =>
        {
            await _queue.PublishAsync(domainEvent.Data, "order.checkedout");
        });

        order.Checkout(shippingAddress.Id, billingAddress.Id, command.PaymentType, command.CardToken, command.Installments);

        _orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
