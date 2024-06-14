using Application.Abstractions.Messaging;
using Domain.Categories.Entity;
using Domain.Categories.Error;
using Domain.Categories.Repository;
using Domain.Shared;

namespace Application.Categories.Update;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category == null) return Result.Failure<Category>(CategoryErrors.CategoryNotFound);

        category.Update(command.Name, command.Description);

        await _categoryRepository.Update(category);

        return Result.Success();
    }
}
