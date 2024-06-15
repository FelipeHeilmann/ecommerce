using Application.Abstractions.Messaging;
using Domain.Categories.Error;
using Domain.Categories.Repository;
using Domain.Products.Error;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Products.GetById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Output>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Output>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.ProductId, cancellationToken);

        if (product == null) return Result.Failure<Output>(ProductErrors.ProductNotFound);

        CategoryOutput? category = null;
        
        if(product.CategoryId is not null) 
        {
            var databaseCategory = await _categoryRepository.GetByIdAsync(product.CategoryId.Value, cancellationToken);
            if(databaseCategory is null) return Result.Failure<Output>(CategoryErrors.CategoryNotFound);
            category = new CategoryOutput(databaseCategory.Id, databaseCategory.Name, databaseCategory.Description);
        }

        return new Output(
                        product.Id,
                        product.Name,
                        product.Description,
                        product.ImageUrl,
                        product.Amount,
                        product.Currency,
                        product.Sku,
                        category
                    );
    }
}
    

