using Domain.Customers.Entity;
using Domain.Customers.Repository;

namespace Infra.Repositories.Memory
{
    public class CustomerRepositoryMemory : ICustomerRepository
    {
        private readonly List<Customer> _context = new();

        public Task<ICollection<Customer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<ICollection<Customer>>(_context);
        }

        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.ToList().FirstOrDefault(c => c.Id == id));
        }

        public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.ToList().FirstOrDefault(c => c.Email == email));
        }

        public Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Any(c => c.Email == email));
        }

        public void Add(Customer entity)
        {
            _context.Add(entity);
        }

        public void Update(Customer entity)
        {
            var index = _context.FindIndex(c => c.Id == entity.Id);

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

        public IQueryable<Customer> GetQueryable(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
