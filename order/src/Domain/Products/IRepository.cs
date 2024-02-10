using Domain.Shared;

namespace Domain.Products;

public interface IProductRepository : IRepositoryBase<Product> 
{
    public Task<ICollection<Product>> GetByIdsAsync(List<Guid> Ids, CancellationToken cancellationToken);
}

public interface ICategoryRepository : IRepositoryBase<Category>
{
}
