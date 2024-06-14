using Application.Abstractions.Messaging;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Products.Error;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Orders.AddItemToCart;

public class AddItemToCartCommandHandler : ICommandHandler<AddItemToCartCommand>
{
    private IOrderRepository _orderRepository;
    private IProductRepository _productRepository;

    public AddItemToCartCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        Order? order;

        var existingCart = await _orderRepository.GetCart(cancellationToken);

        order = existingCart ?? Order.Create(command.CustomerId, null, true);

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product == null) return Result.Failure(ProductErrors.ProductNotFound);

        order.AddItem(product.Id, product.Currency, product.Amount, command.Quantity);

        if (existingCart == null)
        {
            await _orderRepository.Add(order);
        }
        else
        {
            await _orderRepository.Update(order);
        }

        return Result.Success();
    }
}
