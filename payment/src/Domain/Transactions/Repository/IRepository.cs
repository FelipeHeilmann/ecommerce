using Domain.Shared;
using Domain.Transactions.Entity;

namespace Domain.Transactions.Repository;

public interface ITransactionRepository : IRepositoryBase<Transaction> 
{
    public Task<Transaction?> GetByGatewayServiceId(Guid id, CancellationToken cancellation);
}
