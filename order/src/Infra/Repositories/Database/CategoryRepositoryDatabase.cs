using System.Data;
using Domain.Categories.Entity;
using Domain.Categories.Repository;
using Infra.Database;

namespace Infra.Repositories.Database;

public class CategoryRepositoryDatabase : ICategoryRepository
{
    private readonly IDatabaseConnection _connection;
    public CategoryRepositoryDatabase(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public async Task<ICollection<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _connection.Query("select * from categories", null, MapCategory);
        return categories.ToList();
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _connection.Query("select * from categories where id = @Id", new {Id = id}, MapCategory);
        return category.FirstOrDefault();
    }

    public IQueryable<Category> GetQueryable(CancellationToken cancellationToken)
    {
        var categories = _connection.Query("select * from categories", null, MapCategory).Result;
        return categories.AsQueryable();
    }

    public async Task Add(Category entity)
    {
       await _connection.Query<Task>("insert into categories (id, name, description) values (@Id, @Name, @Description)", entity);
    }

    public async Task Update(Category entity)
    {
       await _connection.Query<Task>("update categories set name = @Name, description = @Description where id = @Id", entity);
    }

    public async Task Delete(Category entity)
    {
        await _connection.Query<Task>("delete from catagories where id = @Id", new { entity.Id });
    }

    private Category MapCategory(IDataReader reader)
    {
        return Category.Restore(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("name")),
            reader.GetString(reader.GetOrdinal("description"))
        );
    }
}
