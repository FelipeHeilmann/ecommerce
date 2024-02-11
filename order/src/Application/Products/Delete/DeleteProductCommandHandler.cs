using Application.Abstractions;
using Application.Data;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Delete;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product == null) return Result.Failure(ProductErrors.ProductNotFound);

        _repository.Delete(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product);
    }
}
