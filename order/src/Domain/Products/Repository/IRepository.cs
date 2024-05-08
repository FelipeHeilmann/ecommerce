using Domain.Products.Entity;
using Domain.Shared;

namespace Domain.Products.Repository;

public interface IProductRepository : IRepositoryBase<Product>
{
    public Task<ICollection<Product>> GetByIdsAsync(List<Guid> Ids, CancellationToken cancellationToken);
}

