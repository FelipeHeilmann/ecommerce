using Application.Abstractions.Messaging;
using Application.Data;
using Application.Gateway;
using Domain.Customers;
using Domain.Orders;
using Domain.Products;
using Domain.Shared;

namespace Application.Orders.Create;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly INotifyGateway _notifyGateway;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, INotifyGateway notifyGateway, ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _notifyGateway = notifyGateway;
        _customerRepository = customerRepository;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderItemList = command.request.OrderItens;

        var order = Order.Create(command.request.CustomerId);

        var customer = await _customerRepository.GetByIdAsync(command.request.CustomerId, cancellationToken);

        if (customer == null)
        {
            return Result.Failure<Guid>(CustomerErrors.CustomerNotFound);
        }

        List<ItemsMail> itemsMail = new List<ItemsMail>();

        foreach (var item in orderItemList)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
            {
                return Result.Failure<Guid>(ProductErrors.ProductNotFound);
            }
            order.AddItem(item.ProductId, product.Price, item.Quantity);
            itemsMail.Add(new ItemsMail(product.Name, product.Price.Amount, item.Quantity));
        }

        _orderRepository.Add(order);

        await _notifyGateway.SendOrderCreatedMail(new OrderCreatedMail(order.Id, DateTime.UtcNow, customer.Name.Value, customer.Email.Value, itemsMail));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order.Id);
    }
}
