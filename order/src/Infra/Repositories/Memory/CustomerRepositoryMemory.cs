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
            return Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
        }

        public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(_customers.FirstOrDefault(c => c.Email == email));
        }

        public Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(_customers.Any(c => c.Email == email));
        }

        public Task Add(Customer entity)
        {
            _customers.Add(entity);
             return Task.CompletedTask;
        }

        public Task Update(Customer entity)
        {
            var index = _customers.FindIndex(c => c.Id == entity.Id);

            if (index == -1)
            {
                 return Task.CompletedTask;;
            }

            _customers[index] = entity;
            return Task.CompletedTask;
        }

        public Task Delete(Customer entity)
        {
            _customers.Remove(entity);
             return Task.CompletedTask;
        }

        public IQueryable<Customer> GetQueryable(CancellationToken cancellation)
        {
            return _customers.AsQueryable();
        }
    }
}
