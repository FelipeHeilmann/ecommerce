using Application.Abstractions.Messaging;
using Domain.Categories.Error;
using Domain.Categories.Repository;
using Domain.Products.Entity;
using Domain.Products.Error;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Products.Update;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    public UpdateProductCommandHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _productRepository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product == null) return Result.Failure<Product>(ProductErrors.ProductNotFound);

        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category == null) return Result.Failure<Product>(CategoryErrors.CategoryNotFound);

        product.Update(command.Name, command.Description, command.ImageUrl, command.Currency, command.Price, command.Sku, category.Id);

        await _productRepository.Update(product);

        return Result.Success();
    }
}
