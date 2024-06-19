using Application.Abstractions.Messaging;
using Domain.Categories.Error;
using Domain.Categories.Repository;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Products.GetAll;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, ICollection<Output>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<ICollection<Output>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        var output = new List<Output>();

        foreach (var product in products)
        {
            CategoryOutput? category = null;

            if(product.CategoryId is not null) 
            {
                var databaseCategory = await _categoryRepository.GetByIdAsync(product.CategoryId.Value, cancellationToken);
                if(databaseCategory == null) return Result.Failure<ICollection<Output>>(CategoryErrors.CategoryNotFound);
                category = new CategoryOutput(databaseCategory.Id, databaseCategory.Name, databaseCategory.Description);
            }

            output.Add(new Output(
                            product.Id,
                            product.Name,
                            product.Description,
                            product.ImageUrl,
                            product.Amount,
                            product.Currency,
                            product.Sku,
                            category
                        ));
        }

        return output;
    }
}
