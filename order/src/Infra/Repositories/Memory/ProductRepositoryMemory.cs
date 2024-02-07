using Domain.Products;

namespace Infra.Repositories.Memory
{
    public class ProductRepositoryMemory : IProductRepository
    {
        private readonly List<Product> _context = new();
        private readonly List<Category> _categoryContext = new() { new Category(Guid.Parse("de1ab44a-ef05-42da-a0e8-6137368018fc"), "Descricao", "Nome") };

        public Task<ICollection<Product>> GetAllAsync()
        {
            return Task.FromResult<ICollection<Product>>(_context);
        }
        public Task<Product?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_context.ToList().FirstOrDefault(p => p.Id == id));
        }
        public Task<ICollection<Product>> GetByIdsAsync(List<Guid> Ids)
        {
            var products = _context.Where(p => Ids.Contains(p.Id)).ToList();
            return Task.FromResult<ICollection<Product>>(products);
        }
        public void Add(Product entity)
        {
            _context.Add(entity);
        }

        public void Update(Product entity)
        {
            var index = _context.FindIndex(p => p.Id == entity.Id);

            if (index == -1)
            {
                return;
            }

            _context[index] = entity;

        }
        public void Delete(Product entity)
        {
            _context.Remove(entity);
        }

        public Task<Category?> GetCategoryById(Guid id)
        {
            return Task.FromResult(_categoryContext.FirstOrDefault(c => c.Id == id));
        }
    }
}
