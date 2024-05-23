using Domain.Orders.Entity;
using Domain.Orders.Repository;

namespace Infra.Repositories.Memory
{
    public class OrderRepositoryMemory : IOrderRepository
    {
        private readonly List<Order> _context = new();

        public Task<ICollection<Order>> GetAllAsync(CancellationToken cancellationToken)
        { 
            return Task.FromResult<ICollection<Order>>(_context);
        }

        public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.FirstOrDefault(o => o.Id == id));
        }

        public Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            return Task.FromResult<ICollection<Order>>(_context.Where(o => o.CustomerId == customerId).ToList());
        }

        public void Add(Order entity)
        {
            _context.Add(entity);
        }

        public void Update(Order entity)
        {
            var index = _context.FindIndex(o => o.Id == entity.Id);

            if (index == -1)
            {
                return;
            }

            _context[index] = entity;
        }

        public void Delete(Order entity)
        {
            _context.Remove(entity);
        }

        public Task<Order?> GetCategoryById(Guid id)
        {
            return Task.FromResult(_context.FirstOrDefault(c => c.Id == id));
        }

        public IQueryable<Order> GetQueryable(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetCart(CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.FirstOrDefault(o => o.Status == "cart"));
        }
    }
}
