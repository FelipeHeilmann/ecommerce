using Application.Data;

namespace Infra.Data;

public class UnitOfWorkMemory : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
