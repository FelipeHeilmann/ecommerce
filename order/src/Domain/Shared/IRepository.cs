namespace Domain.Shared;

public interface IRepositoryBase<T>
{
    public Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
    public IQueryable<T> GetQueryable(CancellationToken cancellationToken);
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task Add(T entity);
    public Task Update(T entity);
    public Task Delete(T entity);
}
