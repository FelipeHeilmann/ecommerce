using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Addresses;
using Domain.Customers;
using Domain.Orders;
using Domain.Orders.DomainEvents;
using Domain.Shared;
using Application.Orders.Model;
using Application.Abstractions.Gateway;

namespace Application.Orders.Checkout;

public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand, object>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutOrderCommandHandler
    (
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork
,
        IPaymentGateway paymentGateway)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }

    public async Task<Result<object>> Handle(CheckoutOrderCommand command, CancellationToken cancellationToken)
    {

        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken, "Items");

        if (order == null) return Result.Failure<PaymentSystemTransactionResponse>(OrderErrors.OrderNotFound);

        var billingAddress = await _addressRepository.GetByIdAsync(command.BillingAddressId, cancellationToken);

        if (billingAddress == null) return Result.Failure<PaymentSystemTransactionResponse>(AddressErrors.NotFound);

        var shippingAddress = await _addressRepository.GetByIdAsync(command.ShippingAddressId, cancellationToken);
        
        if (shippingAddress == null) return Result.Failure<PaymentSystemTransactionResponse>(AddressErrors.NotFound);

        var checkout = order.Checkout(shippingAddress.Id, billingAddress.Id);

        if(checkout.IsFailure) return Result.Failure<PaymentSystemTransactionResponse>(checkout.Error);

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId, cancellationToken);

        _orderRepository.Update(order);

        var orderPurchased = new OrderPurchasedEvent(
            order.Id,
            order.CalculateTotal(),
            order.Items.Select(li => new LineItemOrderPurchasedEvent(li.Id, li.ProductId, li.Quantity, li.Price.Amount)),
            customer!.Id,
            customer!.Name.Value,
            customer!.Email.Value,
            customer!.CPF.Value,
            customer!.Phone.Value,
            command.PaymentType,
            command.CardToken,
            command.Installments,
            billingAddress.ZipCode.Value,
            billingAddress.Number,
            billingAddress.Complement
         );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _paymentGateway.ProccessPayment(orderPurchased);
    }
}
