using Application.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Command;

public class GetProductByIdCommandHandler : ICommandHandler<GetProductByIdCommand, Result<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Product>> Handle(GetProductByIdCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId);

        if (product == null) return Result.Failure<Product>(ProductErrors.ProductNotFound);

        return Result.Success(product);
    }
}
