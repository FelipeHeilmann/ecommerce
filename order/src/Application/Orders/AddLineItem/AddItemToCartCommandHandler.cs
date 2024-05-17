using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Products.Error;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Orders.AddLineItem;

public class AddItemToCartCommandHandler : ICommandHandler<AddItemToCartCommand>
{
    private IOrderRepository _orderRepository;
    private IProductRepository _productRepository;
    private IUnitOfWork _unitOfWork;

    public AddItemToCartCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        Order? order;

        var existingCart = await _orderRepository.GetCart(cancellationToken);

        order = existingCart ?? Order.Create(command.CustomerId, true);

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product == null) return Result.Failure(ProductErrors.ProductNotFound);

        order.AddItem(product.Id, product.Price, command.Quantity);

        if (existingCart == null)
        {
            _orderRepository.Add(order);
        }
        else
        {
            _orderRepository.Update(order);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
