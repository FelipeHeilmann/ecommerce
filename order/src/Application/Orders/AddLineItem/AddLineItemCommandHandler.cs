using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Orders;
using Domain.Products;
using Domain.Shared;

namespace Application.Orders.AddLineItem;

public class AddLineItemCommandHandler : ICommandHandler<AddLineItemCommand>
{
    private IOrderRepository _orderRepository;
    private IProductRepository _productRepository;
    private IUnitOfWork _unitOfWork;

    public AddLineItemCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddLineItemCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if(order == null) return Result.Failure(OrderErrors.OrderNotFound);

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product == null) return Result.Failure(ProductErrors.ProductNotFound);

        order.AddItem(product.Id, product.Price, command.Quantity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
