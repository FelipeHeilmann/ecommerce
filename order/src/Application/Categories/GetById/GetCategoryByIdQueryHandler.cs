using Application.Abstractions.Messaging;
using Application.Categories.GetById;
using Domain.Categories.Error;
using Domain.Categories.Repository;
using Domain.Shared;

namespace Application.Categories.GetAll;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, GetById.Output>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<GetById.Output>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(query.CategoryId, cancellationToken);

        if (category == null) return Result.Failure<GetById.Output>(CategoryErrors.CategoryNotFound);

        return new GetById.Output(
                                category.Id,
                                category.Name,
                                category.Description
                            );
    }

}
