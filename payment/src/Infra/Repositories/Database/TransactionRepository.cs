using Domain.Transactions;
using Infra.Context;

namespace Infra.Repositories.Database;

public class TransactionRepository : Repository<Transaction> , ITransactionRepository
{
    public TransactionRepository(ApplicationContext context) : base(context) { }
}
