using Domain.Refunds;
using Infra.Context;
using Infra.Model;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class RefundRepository : IRefundRepository
{
    private readonly ApplicationContext _context;
    public RefundRepository(ApplicationContext context) => _context = context;

    public IQueryable<Refund> GetQueryable(CancellationToken cancellation)
    {
        var refunds = new List<Refund>();   
        foreach(var refundModel in _context.Set<RefundModel>().ToList())
        {
            refunds.Add(refundModel.ToAggregate());
        }
        return refunds.AsQueryable();
    }
    public async Task<ICollection<Refund>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        var refunds = new List<Refund>();
        foreach (var refundModel in await _context.Set<RefundModel>().ToListAsync())
        {
            refunds.Add(refundModel.ToAggregate());
        }
        return refunds.ToList();
    }
    public async Task<Refund?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        var model = await _context.Set<RefundModel>().FirstOrDefaultAsync(refund => refund.Id == id);
        return model?.ToAggregate();
    }
    public void Add(Refund entity)
    {
        _context.Add(RefundModel.FromAggregate(entity));
    }
    public void Update(Refund entity)
    {
        _context.Update(RefundModel.FromAggregate(entity));
    }
    public void Delete(Refund entity)
    {
        _context.Remove(RefundModel.FromAggregate(entity));
    }
}
