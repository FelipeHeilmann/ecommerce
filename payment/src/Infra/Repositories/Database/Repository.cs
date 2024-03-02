﻿using Domain.Shared;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public abstract class Repository<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    private readonly ApplicationContext _context;

    protected Repository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (!string.IsNullOrEmpty(include)) query = query.Include(include);

        return await query.ToListAsync();

    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (!string.IsNullOrEmpty(include)) query = query.Include(include);

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);
    }

    public IQueryable<TEntity> GetQueryable(CancellationToken cancellation)
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}
