using Domain.Products;
using Domain.Shared;

namespace Domain.Orders;

public interface IOrderRepository : IRepositoryBase<Order>
{
    public Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken);
}


