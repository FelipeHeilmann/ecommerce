using Domain.Customer;
namespace Infra.Repositories
{
    public class CustomerRepositoryMemory : ICustomerRepository
    {
        private readonly List<Customer> _context = new();

        public Task<ICollection<Customer>> GetAllAsync()
        {
            return Task.FromResult<ICollection<Customer>>(_context);
        }
        public Task<Customer?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_context.ToList().FirstOrDefault(o => o.Id == id));
        }
        public void Add(Customer entity)
        {
            _context.Add(entity);
        }

        public void Update(Customer entity)
        {
            var index = _context.FindIndex(o => o.Id == entity.Id);

            if (index == -1)
            {
                return;
            }

            _context[index] = entity;

        }
        public void Delete(Customer entity)
        {
            _context.Remove(entity);
        }

        public Task<Customer?> GetCategoryById(Guid id)
        {
            return Task.FromResult(_context.FirstOrDefault(c => c.Id == id));
        }
    }
}
