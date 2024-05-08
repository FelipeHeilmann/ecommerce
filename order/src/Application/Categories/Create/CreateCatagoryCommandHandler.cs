using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Categories.Entity;
using Domain.Categories.Repository;
using Domain.Shared;

namespace Application.Categories.Create;

public class CreateCatagoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private ICategoryRepository _categoryRepository;
    private IUnitOfWork _unitOfWork;

    public CreateCatagoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;
        var category = Category.Create(request.Name, request.Description);

        if (category.IsFailure) return Result.Failure<Guid>(category.Error);

        _categoryRepository.Add(category.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Value.Id;

    }
}
