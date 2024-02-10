using Domain.Orders;
using Domain.Products;
using System.Collections;

namespace Infra.Repositories.Memory;

public class CategoryRepositoryMemory : ICategoryRepository 
{
    private readonly List<Category> _context = new();

    public Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult<ICollection<Category>>((_context));
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult(_context.FirstOrDefault(c => c.Id == id));
    }
    public void Add(Category entity)
    {
        _context.Add(entity);
    }

    public void Update(Category entity)
    {
        var index = _context.FindIndex(o => o.Id == entity.Id);

        if (index == -1)
        {
            return;
        }

        _context[index] = entity;
    }
    public void Delete(Category entity)
    {
        _context.Remove(entity);
    }
}
