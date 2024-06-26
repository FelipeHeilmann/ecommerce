﻿using Domain.Shared;
using Domain.Transactions.Entity;
using Domain.Transactions.Repository;

namespace Infra.Repositories.Memory;

public class TransactionRepositoryMemory : IRepositoryBase<Transaction>, ITransactionRepository
{
    private readonly ICollection<Transaction> _transactions;

    public TransactionRepositoryMemory()
    {
        _transactions = new List<Transaction>();
    }

    public Task<ICollection<Transaction>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult<ICollection<Transaction>>(_transactions);
    }
    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult(_transactions.FirstOrDefault(t => t.Id == id));
    }

    public Task<Transaction?> GetByGatewayServiceId(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_transactions.FirstOrDefault(t => t.PaymentServiceId == id));
    }
    public IQueryable<Transaction> GetQueryable(CancellationToken cancellation)
    {
        return _transactions.AsQueryable();
    }
    public Task Add(Transaction entity)
    {
        _transactions.Add(entity);
        return Task.CompletedTask;
    }
    public Task Update(Transaction entity)
    {
        var index = _transactions.ToList().FindIndex(a => a.Id == entity.Id);

        if (index == -1)
        {
            return Task.CompletedTask;
        }

        _transactions.ToList()[index] = entity;
        return Task.CompletedTask;
    }
    public Task Delete(Transaction entity)
    {
        _transactions.Remove(entity);
        return Task.CompletedTask;
    }

   
}
