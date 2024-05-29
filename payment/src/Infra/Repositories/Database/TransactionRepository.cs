using Domain.Transactions.Entity;
using Domain.Transactions.Repository;
using Infra.Context;
using Infra.Model;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationContext _context;
    public TransactionRepository(ApplicationContext context) => _context = context;

    public IQueryable<Transaction> GetQueryable(CancellationToken cancellation)
    {
       var transactions = new List<Transaction>();
       foreach(var transactionModel in _context.Set<TransactionModel>().ToList())
       {
            transactions.Add(transactionModel.ToAggregate());
       }
        return transactions.AsQueryable();
    }
    public async Task<ICollection<Transaction>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        var transactions = new List<Transaction>();
        foreach (var transactionModel in await _context.Set<TransactionModel>().ToListAsync())
        {
            transactions.Add(transactionModel.ToAggregate());
        }
        return transactions.ToList();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        var model = await _context.Set<TransactionModel>().FirstOrDefaultAsync(transaction => transaction.Id == id);
        return model?.ToAggregate();
    }

    public async Task<Transaction> GetByGatewayServiceId(Guid id, CancellationToken cancellationToken)
    {
        var model = await _context.Set<TransactionModel>().FirstOrDefaultAsync(transaction => transaction.PaymentServiceId == id, cancellationToken);
        if (model == null) throw new Exception("Transaction not found");
        return model.ToAggregate();
    }

    public void Add(Transaction entity)
    {
        _context.Set<TransactionModel>().Add(TransactionModel.FromAggregate(entity));
    }
    public void Update(Transaction entity)
    {
        var existingEntity = _context.Set<TransactionModel>().Local.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Detached;
        }
        _context.Set<TransactionModel>().Update(TransactionModel.FromAggregate(entity));
    }

    public void Delete(Transaction entity)
    {
        _context.Set<TransactionModel>().Remove(TransactionModel.FromAggregate(entity));
    }
}
