using Application.Abstractions.Messaging;
using Domain.Categories.Error;
using Domain.Categories.Repository;
using Domain.Products.Entity;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Products.Create;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _productRepository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            command.Name,
            command.Description,
            command.ImageUrl,
            command.Currency,
            command.Price,
            command.Sku,
            command.CategoryId
        );
        
        await _productRepository.Add(product);

        return Result.Success(product.Id);
    }
}
