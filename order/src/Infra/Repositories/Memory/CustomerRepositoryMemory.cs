using Domain.Customers.Entity;
using Domain.Customers.Repository;

namespace Infra.Repositories.Memory
{
    public class CustomerRepositoryMemory : ICustomerRepository
    {
        private List<Customer> _customers;

        public CustomerRepositoryMemory()
        {
            _customers = [];
        }

        public Task<ICollection<Customer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<ICollection<Customer>>(_customers);
        }

        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_customers.ToList().FirstOrDefault(c => c.Id == id));
        }

        public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(_customers.ToList().FirstOrDefault(c => c.Email == email));
        }

        public Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(_customers.Any(c => c.Email == email));
        }

        public void Add(Customer entity)
        {
            _customers.Add(entity);
        }

        public void Update(Customer entity)
        {
            var index = _customers.FindIndex(c => c.Id == entity.Id);

            if (index == -1)
            {
                return;
            }

            _customers[index] = entity;

        }

        public void Delete(Customer entity)
        {
            _customers.Remove(entity);
        }

        public IQueryable<Customer> GetQueryable(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
