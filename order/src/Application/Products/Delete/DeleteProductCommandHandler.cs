using Application.Abstractions.Messaging;
using Domain.Products.Error;
using Domain.Products.Repository;
using Domain.Shared;

namespace Application.Products.Delete;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product == null) return Result.Failure(ProductErrors.ProductNotFound);

        await _repository.Delete(product);

        return Result.Success(product);
    }
}
