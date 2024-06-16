namespace Domain.Shared;

public interface IRepositoryBase<T>
{
    public Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken, string? include = null);
    public IQueryable<T> GetQueryable(CancellationToken cancellation);
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null);
    public Task Add(T entity);
    public Task Update(T entity);
    public Task Delete(T entity);
}
