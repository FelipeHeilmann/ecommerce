using Application.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Command;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Result<Product>>
{
    private readonly IProductRepository _repository;

    public UpdateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Product>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;
        var product = await _repository.GetByIdAsync(request.ProductId);

        if (product == null) return Result.Failure<Product>(ProductErrors.ProductNotFound);

        var category = await _repository.GetCategoryById(request.CategoryId);

        if (category == null) return Result.Failure<Product>(ProductErrors.CategoryNotFound);

        var result = product.Update(request.Name, request.Description, request.ImageUrl, request.Currency, request.Price, request.Sku, category);

        if(result.IsFailure) return Result.Failure<Product>(result.Error);

        return Result.Success(result.Data);
    }
}
