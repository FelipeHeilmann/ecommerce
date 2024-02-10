using Application.Abstractions;
using Application.Data;
using Domain.Orders;
using Domain.Products;
using Domain.Shared;

namespace Application.Orders.Command;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<Order>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Order>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderItemList = command.request.OrderItens;

        var order = Order.Create(command.request.CustomerId);

        foreach (var item in orderItemList)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if(product == null)
            {
                return Result.Failure<Order>(ProductErrors.ProductNotFound);
            }
            order.AddItem(item.ProductId, product.Price ,item.Quantity);
        }

        _orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order);
    }
}
