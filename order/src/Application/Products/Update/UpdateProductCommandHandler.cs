using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Categories;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Update;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductRepository repository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = repository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product == null) return Result.Failure<Product>(ProductErrors.ProductNotFound);

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category == null) return Result.Failure<Product>(CategoryErrors.CategoryNotFound);

        var result = product.Update(request.Name, request.Description, request.ImageUrl, request.Currency, request.Price, request.Sku, category);

        if (result.IsFailure) return Result.Failure<Product>(result.Error);

        _productRepository.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
