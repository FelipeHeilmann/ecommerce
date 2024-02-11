using Application.Abstractions;
using Application.Data;
using Domain.Categories;
using Domain.Orders;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Create;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository repository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = repository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.request.CategoryId, cancellationToken);

        if (category == null)
        {
            return Result.Failure<Guid>(CategoryErrors.CategoryNotFound);
        }

        var result = Product.Create(
            command.request.Name,
            command.request.Description,
            command.request.ImageUrl,
            command.request.Currency,
            command.request.Price,
            command.request.Sku,
            category
        );

        if (result.IsFailure) return Result.Failure<Guid>(result.Error);

        var product = result.Data;

        _productRepository.Add(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id);
    }
}
