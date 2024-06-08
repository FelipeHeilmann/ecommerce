using Domain.Categories.Entity;
using Domain.Categories.Repository;

namespace Infra.Repositories.Memory;

public class CategoryRepositoryMemory : ICategoryRepository
{
    private List<Category> categories;

    public CategoryRepositoryMemory()
    {
        categories = [];
    }

    public Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<Category>>((categories));
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(categories.FirstOrDefault(c => c.Id == id));
    }
    public void Add(Category entity)
    {
        categories.Add(entity);
    }

    public void Update(Category entity)
    {
        var index = categories.FindIndex(o => o.Id == entity.Id);

        if (index == -1)
        {
            return;
        }

        categories[index] = entity;
    }
    public void Delete(Category entity)
    {
        categories.Remove(entity);
    }

    public IQueryable<Category> GetQueryable(CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}
