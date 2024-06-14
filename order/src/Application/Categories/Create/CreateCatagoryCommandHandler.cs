using Application.Abstractions.Messaging;
using Domain.Categories.Entity;
using Domain.Categories.Repository;
using Domain.Shared;

namespace Application.Categories.Create;

public class CreateCatagoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private ICategoryRepository _categoryRepository;

    public CreateCatagoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Category.Create(command.Name, command.Description);

        if (category.IsFailure) return Result.Failure<Guid>(category.Error);

        await _categoryRepository.Add(category.Value);

        return category.Value.Id;

    }
}
