using Domain.Refunds;
using Infra.Context;

namespace Infra.Repositories.Database;

public class RefundRepository : Repository<Refund>, IRefundRepository
{
    public RefundRepository(ApplicationContext context) : base(context) { }
}
