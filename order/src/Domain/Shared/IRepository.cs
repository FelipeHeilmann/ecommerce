namespace Domain.Shared;

public interface IRepositoryBase<T>
{
    public Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken, string? include = null);
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null);
    public void Add(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}
