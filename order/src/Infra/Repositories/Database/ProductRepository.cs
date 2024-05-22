using Domain.Products.Entity;
using Domain.Products.Repository;
using Infra.Context;
using Infra.Models.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationContext _context;
    public ProductRepository(ApplicationContext context) 
    {
        _context = context;
    }

    public IQueryable<Product> GetQueryable(CancellationToken cancellationToken)
    {
        var products = new List<Product>();
        foreach(var productModel in _context.Set<ProductsModel>().Include(model => model.Category).ToList()) 
        {
            products.Add(productModel.ToAggregate());
        }
        return products.AsQueryable();
    }

    public async Task<ICollection<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = new List<Product>();
        foreach (var productModel in await _context.Set<ProductsModel>().Include(model => model.Category)
                                                                        .ToListAsync(cancellationToken))
        {
            products.Add(productModel.ToAggregate());
        }
        return products.ToList();
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var productModel = await _context.Set<ProductsModel>().Include(model => model.Category)
                                                              .FirstOrDefaultAsync(model => model.Id == id, cancellationToken);
        return productModel?.ToAggregate();
    }

    public async Task<ICollection<Product>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await _context.Set<ProductsModel>().Include(model => model.Category)
                                                  .Where(p => ids.Contains(p.Id))
                                                  .Select(model => model.ToAggregate())
                                                  .ToListAsync(cancellationToken);
    }

    public void Add(Product entity)
    {
        _context.Add(ProductsModel.FromAggregate(entity));
    }

    public void Update(Product entity)
    {
        _context.Update(ProductsModel.FromAggregate(entity));
    }
    public void Delete(Product entity)
    {
        _context.Remove(ProductsModel.FromAggregate(entity));
    }
}
