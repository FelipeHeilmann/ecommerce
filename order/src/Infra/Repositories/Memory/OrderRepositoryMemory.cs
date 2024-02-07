using Domain.Orders;
namespace Infra.Repositories.Memory
{
    public class OrderRepositoryMemory : IOrderRepository
    {
        private readonly List<Order> _context = new();

        public Task<ICollection<Order>> GetAllAsync()
        {
            return Task.FromResult<ICollection<Order>>(_context);
        }
        public Task<Order?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_context.ToList().FirstOrDefault(o => o.Id == id));
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
    }
}
