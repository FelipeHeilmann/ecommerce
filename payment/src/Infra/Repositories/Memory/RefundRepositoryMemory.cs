using Domain.Refunds;

namespace Infra.Repositories.Memory;
public class RefundRepositoryMemory : IRefundRepository
{
    private List<Refund> refunds;

    public RefundRepositoryMemory()
    {
        refunds = [];
    }

    public Task<ICollection<Refund>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult<ICollection<Refund>>(refunds.ToList());
    }

    public Task<Refund?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        return Task.FromResult(refunds.FirstOrDefault(refund => refund.Id == id));
    }

    public IQueryable<Refund> GetQueryable(CancellationToken cancellation)
    {
        return refunds.AsQueryable();
    }

    public Task Add(Refund entity)
    {
        refunds.Add(entity);
        return Task.CompletedTask;
    }

    public Task Update(Refund entity)
    {
        var index = refunds.FindIndex(refund => refund.Id == entity.Id);
        if(index == -1){
            return Task.CompletedTask;
        }
        refunds[index] = entity;
        return Task.CompletedTask;
    }

    public Task Delete(Refund entity)
    {
        refunds.Remove(entity);
        return Task.CompletedTask;
    }
}
