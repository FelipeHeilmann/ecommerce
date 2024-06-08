using Domain.Orders.Entity;
using Domain.Orders.Repository;

namespace Infra.Repositories.Memory
{
    public class OrderRepositoryMemory : IOrderRepository
    {
        private List<Order> _orders;

        public OrderRepositoryMemory()
        {
            _orders = [];
        }

        public Task<ICollection<Order>> GetAllAsync(CancellationToken cancellationToken)
        { 
            return Task.FromResult<ICollection<Order>>(_orders);
        }

        public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        }

        public Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            return Task.FromResult<ICollection<Order>>(_orders.Where(o => o.CustomerId == customerId).ToList());
        }

        public void Add(Order entity)
        {
            _orders.Add(entity);
        }

        public void Update(Order entity)
        {
            var index = _orders.FindIndex(o => o.Id == entity.Id);

            if (index == -1)
            {
                return;
            }

            _orders[index] = entity;
        }

        public void Delete(Order entity)
        {
            _orders.Remove(entity);
        }

        public Task<Order?> GetCategoryById(Guid id)
        {
            return Task.FromResult(_orders.FirstOrDefault(c => c.Id == id));
        }

        public IQueryable<Order> GetQueryable(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetCart(CancellationToken cancellationToken)
        {
            return Task.FromResult(_orders.FirstOrDefault(o => o.Status == "cart"));
        }
    }
}
