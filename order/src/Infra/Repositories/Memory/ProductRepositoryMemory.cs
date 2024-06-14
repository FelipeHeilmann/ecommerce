using Domain.Categories.Entity;
using Domain.Products.Entity;
using Domain.Products.Repository;

namespace Infra.Repositories.Memory
{
    public class ProductRepositoryMemory : IProductRepository
    {
        private List<Product> _products;

        public ProductRepositoryMemory()
        {
            _products = [];
        }

        public IQueryable<Product> GetQueryable(CancellationToken cancellation)
        {
            return _products.AsQueryable();
        }

        public Task<ICollection<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<ICollection<Product>>(_products);
        }
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_products.ToList().FirstOrDefault(p => p.Id == id));
        }
        public Task<ICollection<Product>> GetByIdsAsync(List<Guid> Ids, CancellationToken cancellationToken)
        {
            var products = _products.Where(p => Ids.Contains(p.Id)).ToList();
            return Task.FromResult<ICollection<Product>>(products);
        }
        public Task Add(Product entity)
        {
            _products.Add(entity);
            return Task.CompletedTask;
        }

        public Task Update(Product entity)
        {
            var index = _products.FindIndex(p => p.Id == entity.Id);

            if (index == -1)
            {
                return Task.CompletedTask;;
            }

            _products[index] = entity;
            return Task.CompletedTask;

        }
        public Task Delete(Product entity)
        {
            _products.Remove(entity);
            return Task.CompletedTask;
        }

    }
}
