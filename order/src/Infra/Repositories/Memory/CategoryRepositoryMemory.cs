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
    public Task Add(Category entity)
    {
        categories.Add(entity);
         return Task.CompletedTask;
    }

    public Task Update(Category entity)
    {
        var index = categories.FindIndex(o => o.Id == entity.Id);

        if (index == -1)
        {
             return Task.CompletedTask;;
        }

        categories[index] = entity;
         return Task.CompletedTask;
    }
    public Task Delete(Category entity)
    {
        categories.Remove(entity);
         return Task.CompletedTask;
    }

    public IQueryable<Category> GetQueryable(CancellationToken cancellation)
    {
        return categories.AsQueryable();
    }
}
