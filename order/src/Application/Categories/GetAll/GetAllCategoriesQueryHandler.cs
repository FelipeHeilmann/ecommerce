using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Categories;
using Domain.Shared;

namespace Application.Categories.GetAll;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, ICollection<Category>>
{
    private ICategoryRepository _categoryRepository;
    private IUnitOfWork _unitOfWork;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ICollection<Category>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        return Result.Success(categories); 
    }
}
