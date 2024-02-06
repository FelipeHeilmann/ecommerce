using Domain.Shared;

namespace Domain.Products;

public interface IProductRepository : IRepositoryBase<Product> 
{
    public Task<Category?> GetCategoryById(Guid id); 
}
