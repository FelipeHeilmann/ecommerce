using Application.Abstractions.Messaging;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Products.GetAll;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, ICollection<Output>>
{
    private readonly IProductRepository _repository;

    public GetAllProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<Output>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(cancellationToken);

        var output = new List<Output>();

        foreach (var product in products)
        {
            output.Add(new Output(
                            product.Id,
                            product.Name,
                            product.Description,
                            product.ImageUrl,
                            product.Price.Amount,
                            product.Price.Currency,
                            product.Sku.Value,
                            new CategoryOutput(product.Category.Id, product.Category.Name, product.Category.Description)
                        ));
        }

        return output;
    }
}
