using Application.Abstractions.Messaging;
using Domain.Products.Error;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Products.GetById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Output>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Output>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(query.ProductId, cancellationToken);

            if (product == null) return Result.Failure<Output>(ProductErrors.ProductNotFound);

            return new Output(
                            product.Id,
                            product.Name,
                            product.Description,
                            product.ImageUrl,
                            product.Price.Amount,
                            product.Price.Currency,
                            product.Sku.Value,
                            new CategoryOutput(product.Category.Id, product.Category.Name, product.Category.Description)
                        );
        }
    }
}
