using Application.Abstractions;
using Domain.Orders;
using Domain.Products;
using Domain.Shared;

namespace Application.Orders.Command;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<Order>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<Order>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderItemList = command.request.OrderItens;

        var customerId = command.request.CustomerId;

        var order = Order.Create();

        foreach (var item in orderItemList)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if(product == null)
            {
                return Result.Failure<Order>(ProductErrors.ProductNotFound);
            }
            order.AddItem(item.ProductId, product.Price ,item.Quantity);
        }

        _orderRepository.Add(order);

        return Result.Success(order);
    }
}
