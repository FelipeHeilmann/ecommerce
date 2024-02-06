namespace Domain.Shared;

public interface IRepositoryBase<T>
{
    public Task<ICollection<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(Guid id);
    public void Add(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}
