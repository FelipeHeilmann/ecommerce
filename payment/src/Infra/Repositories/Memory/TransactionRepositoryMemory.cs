using Domain.Shared;
using Domain.Transactions;

namespace Infra.Repositories.Memory;

public class TransactionRepositoryMemory : IRepositoryBase<Transaction>, ITransactionRepository
{
    private readonly List<Transaction> _context = new();

    public Task<ICollection<Transaction>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult<ICollection<Transaction>>(_context);
    }
    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult(_context.FirstOrDefault(t => t.Id == id));
    }
    public IQueryable<Transaction> GetQueryable(CancellationToken cancellation)
    {
        return _context.AsQueryable();
    }
    public void Add(Transaction entity)
    {
        _context.Add(entity);
    }
    public void Update(Transaction entity)
    {
        var index = _context.FindIndex(a => a.Id == entity.Id);

        if (index == -1)
        {
            return;
        }

        _context[index] = entity;
    }
    public void Delete(Transaction entity)
    {
        _context.Remove(entity);
    }
}
