using System.Data;
using Domain.Customers.Entity;
using Domain.Customers.Repository;
using Infra.Database;

namespace Infra.Repositories.Database;

public class CustomerRepositoryDatabase : ICustomerRepository
{
    private readonly IDatabaseConnection _connection;

    public CustomerRepositoryDatabase(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public async Task<ICollection<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        var customers = await _connection.Query<Customer>("SELECT * FROM customers", null, MapCustomer);
        return customers.ToList();
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var customer = await _connection.Query<Customer>("SELECT * FROM customers where email = @Email", new { Email = email }, MapCustomer);
        return customer.FirstOrDefault();
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _connection.Query<Customer>("SELECT * FROM customers where id = @Id", new { Id = id }, MapCustomer);
        return customer.FirstOrDefault();
    }

    public IQueryable<Customer> GetQueryable(CancellationToken cancellationToken)
    {
        var customers = _connection.Query<Customer>("SELECT * FROM customers", null, MapCustomer).Result;
        return customers.AsQueryable();
    }

    public async Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken)
    {
        var isEmailInUse = await _connection.QuerySingle<int>("SELECT COUNT(*) FROM Customers WHERE Email = @Email", new { Email = email });
        return isEmailInUse > 0;
    }

    public async Task Add(Customer entity)
    {
        await _connection.Query<Task>("INSERT INTO customers (id, name, email, password, phone, cpf, birth_date, created_at) VALUES (@Id, @Name, @Email, @Password, @Phone, @CPF, @BirthDate, @CreatedAt)", entity);
    }

    public async Task Update(Customer entity)
    {
        await _connection.Query<Task>("UPDATE customers set id = @Id, name = @Name, email = @Email, password = @Password, phone = @Phone, cpf = @CPF, birth_date = @BirthDate, created_at = @CreatedAt where id = @Id", entity);
    }

    public async Task Delete(Customer entity)
    {
        await _connection.Query<Task>("DELETE from customers where id = @Id" ,new { Id = entity.Id });
    }

    private Customer MapCustomer(IDataReader reader)
    {
        return Customer.Restore(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("name")),
            reader.GetString(reader.GetOrdinal("email")), 
            reader.GetString(reader.GetOrdinal("cpf")),        
            reader.GetString(reader.GetOrdinal("phone")),
            reader.GetString(reader.GetOrdinal("password")),
            reader.GetDateTime(reader.GetOrdinal("birth_date")),
            reader.GetDateTime(reader.GetOrdinal("created_at"))
        );
    } 
}
