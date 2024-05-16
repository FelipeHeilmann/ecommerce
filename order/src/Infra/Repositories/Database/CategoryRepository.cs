using Domain.Categories.Entity;
using Domain.Categories.Repository;
using Infra.Context;
using Infra.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationContext _context;
    public CategoryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public IQueryable<Category> GetQueryable(CancellationToken cancellationToken)
    {
       var categories = new List<Category>();
       foreach(var categoryModel in _context.Set<CategoryModel>().ToList())
       {
            categories.Add(categoryModel.ToAggregate());
       }
       return categories.AsQueryable();
    }

    public async Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        var categories = new List<Category>();
        foreach (var categoryModel in await _context.Set<CategoryModel>().ToListAsync(cancellationToken))
        {
            categories.Add(categoryModel.ToAggregate());
        }
        return categories.ToList();
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        var categoryModel = await _context.Set<CategoryModel>().FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
        return categoryModel?.ToAggregate();
    }

    public void Add(Category entity)
    {
        _context.Set<CategoryModel>().Add(CategoryModel.FromAggregate(entity));
    }

    public void Update(Category entity)
    {
        _context.Set<CategoryModel>().Update(CategoryModel.FromAggregate(entity));
    }

    public void Delete(Category entity)
    {
        _context.Set<CategoryModel>().Remove(CategoryModel.FromAggregate(entity));
    }
}
