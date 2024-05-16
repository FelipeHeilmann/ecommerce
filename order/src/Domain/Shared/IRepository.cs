namespace Domain.Shared;

public interface IRepositoryBase<T>
{
    public Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken);
    public IQueryable<T> GetQueryable(CancellationToken cancellationToken);
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public void Add(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}
