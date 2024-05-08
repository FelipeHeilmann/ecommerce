using Domain.Orders.Entity;
using Domain.Products;
using Domain.Shared;

namespace Domain.Orders.Repository;

public interface IOrderRepository : IRepositoryBase<Order>
{
    public Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken, string? include = null);
}


