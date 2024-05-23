using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Customers.Error;
using Domain.Customers.Repository;
using Domain.Orders.Entity;
using Domain.Orders.Events;
using Domain.Orders.Repository;
using Domain.Products.Error;
using Domain.Products.Repository;
using Domain.Shared;
using MediatR;

namespace Application.Orders.Create;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, IMediator mediator, ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _customerRepository = customerRepository;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
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

        _orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new OrderCheckedout(order.Id),cancellationToken);

        return Result.Success(order.Id);
    }
}
