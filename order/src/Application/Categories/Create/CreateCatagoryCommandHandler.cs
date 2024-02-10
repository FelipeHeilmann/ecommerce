using Application.Abstractions;
using Application.Data;
using Domain.Categories;
using Domain.Shared;

namespace Application.Categories.Create;

public class CreateCatagoryCommandHandler : ICommandHandler<CreateCategoryCommand, Result<Category>>
{
    private ICategoryRepository _categoryRepository;
    private IUnitOfWork _unitOfWork;

    public CreateCatagoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Category>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;
        var category = Category.Create(request.Name, request.Description);

        if (category.IsFailure) return Result.Failure<Category>(category.Error);

        _categoryRepository.Add(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category;

    }
}
