using Application.Abstractions.Messaging;
using Domain.Categories;
using Domain.Shared;

namespace Application.Categories.GetAll;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, ICollection<Output>>
{
    private ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<ICollection<Output>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        var output = new List<Output>();

        foreach (var category in categories)
        {
            output.Add(new Output(
                              category.Id,
                              category.Name,
                              category.Description
                        ));  
        }
        
        return output;
    }
}
