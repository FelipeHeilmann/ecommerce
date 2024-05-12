using Domain.Customers.Entity;
using Domain.Orders.Entity;
using Domain.Orders.Repository;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infra.Repositories.Database;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationContext context) : base(context) { }

    public async Task<Order?> GetCart(CancellationToken cancellationToken, string? includes = null)
    {
        var query = _context.Set<Order>().AsQueryable();
        if (!string.IsNullOrEmpty(includes))
        {
            query = query.Include(includes);
        }
        return query.ToList().FirstOrDefault(o => o.Status.Value == "cart");
    }

    public async Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken, string? includes = null)
    {
        var query = _context.Set<Order>().AsQueryable();

        if (!string.IsNullOrEmpty(includes))
        {
            query = query.Include(includes);
        }

        return await query.Where(o => o.CustomerId == customerId).ToListAsync(cancellationToken);
    }
}
