using Domain.Categories.Entity;
using Domain.Categories.Repository;
using Infra.Context;

namespace Infra.Repositories.Database;

public class CategoryRepository :Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationContext applicationContext) : base(applicationContext) { }
}
