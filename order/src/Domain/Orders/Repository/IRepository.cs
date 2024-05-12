using Domain.Orders.Entity;
using Domain.Shared;

namespace Domain.Orders.Repository;

public interface IOrderRepository : IRepositoryBase<Order>
{
    public Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken, string? includes = null);
    public Task<Order?> GetCart(CancellationToken cancellationToken, string? includes = null);
}


