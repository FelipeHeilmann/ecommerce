using Application.Data;

namespace Infra.Data;

public class UnitOfWorkMemory : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(1);
    }
}
