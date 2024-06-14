
using System.Data;
using Domain.Products.Entity;
using Domain.Products.Repository;
using Infra.Database;

namespace Infra.Repositories.Database;

public class ProductRepositoryDatabase : IProductRepository
{
    private readonly IDatabaseConnection _connection;

    public ProductRepositoryDatabase(IDatabaseConnection connection)
    {
        _connection = connection;
    } 

    public async Task<ICollection<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
       var products = await _connection.Query("select * from products", null, MapProduct);
       return products.ToList();
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _connection.Query("select * from products where id = @Id", new { Id = id }, MapProduct);
        return product.FirstOrDefault();
    }

    public async Task<ICollection<Product>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        var products = await _connection.Query("select * from products where id in  (" + string.Join(", ", ids) + ")", null, MapProduct);
        return products.ToList();
    }

    public IQueryable<Product> GetQueryable(CancellationToken cancellationToken)
    {
        var products =  _connection.Query("select * from products", null, MapProduct).Result;
        return products.AsQueryable();
    }

    public async Task Add(Product entity)
    {
        await _connection.Query<Task>("insert into products (id, name, description, image_url, currency, amount, sku, created_at, category_id) values (@Id, @Name, @Description, @ImageUrl, @Currency, @Amount, @Sku, @CreatedAt, @CategoryId)", entity);
    }

    public async Task Update(Product entity)
    {
        await _connection.Query<Task>("update products set name = @Name, description = @Description, image_url = @ImageUrl, currency = @Currency, amount = @Amount, sku = @Sku, category_id = @CategoryId, created_at = @CreatedAt where id = @Id", entity);
    }

    public async Task Delete(Product entity)
    {
        await _connection.Query<Task>("delete from products where id = @Id", new { entity.Id });
    }

    private Product MapProduct(IDataReader reader)
    {
        return Product.Restore(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("name")),
            reader.GetString(reader.GetOrdinal("description")),
            reader.GetString(reader.GetOrdinal("image_url")),
            reader.GetString(reader.GetOrdinal("currency")),
            reader.GetDouble(reader.GetOrdinal("amount")),
            reader.GetString(reader.GetOrdinal("sku")),
            reader.IsDBNull(reader.GetOrdinal("category_id")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("category_id")),
            reader.GetDateTime(reader.GetOrdinal("created_at"))
        );
    }
}
