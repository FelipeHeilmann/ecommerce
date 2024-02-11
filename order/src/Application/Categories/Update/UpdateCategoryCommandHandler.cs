using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Categories;
using Domain.Shared;

namespace Application.Categories.Update;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;
        var category = await _categoryRepository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (category == null) return Result.Failure<Category>(CategoryErrors.CategoryNotFound);

        category.Update(request.Name, request.Description);

        _categoryRepository.Update(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
