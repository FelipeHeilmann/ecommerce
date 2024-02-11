using Application.Abstractions.Messaging;
using Application.Categories.GetById;
using Application.Data;
using Domain.Categories;
using Domain.Shared;

namespace Application.Categories.GetAll;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, Category>
{
    private ICategoryRepository _categoryRepository;
    private IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Category>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(query.CategoryId, cancellationToken);

        if (category == null) return Result.Failure<Category>(CategoryErrors.CategoryNotFound);

        return category;
    }

}
