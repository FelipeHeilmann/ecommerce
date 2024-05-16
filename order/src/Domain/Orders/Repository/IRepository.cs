using Domain.Orders.Entity;
using Domain.Shared;

namespace Domain.Orders.Repository;

public interface IOrderRepository : IRepositoryBase<Order>
{
    public Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken);
    public Task<Order?> GetCart(CancellationToken cancellationToken);
}


