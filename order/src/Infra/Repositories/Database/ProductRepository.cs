using Domain.Products.Entity;
using Domain.Products.Repository;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationContext context) : base(context) { }
    public async Task<ICollection<Product>> GetByIdsAsync(List<Guid> Ids, CancellationToken cancellationToken)
    {
        return await _context.Set<Product>().Where(p => Ids.Contains(p.Id)).ToListAsync(cancellationToken);
    }
}
