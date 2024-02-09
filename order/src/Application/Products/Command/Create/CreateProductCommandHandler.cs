using Application.Abstractions;
using Application.Data;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Command;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var category = await _repository.GetCategoryById(command.request.CategoryId, cancellationToken);

        if (category == null)
        {
            return Result.Failure<Guid>(ProductErrors.CategoryNotFound);
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

        _repository.Add(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id);
    }
}
