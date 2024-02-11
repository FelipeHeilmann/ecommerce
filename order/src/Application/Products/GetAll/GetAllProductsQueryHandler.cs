using Application.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.GetAll;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, ICollection<Product>>
{
    private readonly IProductRepository _repository;

    public GetAllProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<Product>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(cancellationToken);

        return products.ToList();
    }
}
