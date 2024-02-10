using Domain.Orders;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infra.Repositories.Database;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationContext context) : base(context) { }
    public async Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        return await _context.Set<Order>().Where(o => o.CustomerId == customerId).ToListAsync(cancellationToken);
    }
}
