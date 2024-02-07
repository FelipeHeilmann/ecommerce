using Application.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Command;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(command.ProductId);

        if (product == null) return Result.Failure(ProductErrors.ProductNotFound);

        _repository.Delete(product);

        return Result.Success(product);
    }
}
