using System.Data;
using Domain.Addresses.Entity;
using Domain.Addresses.Repository;
using Infra.Database;

namespace Infra.Repositories.Database;

public class AddressRepositoryDatabase : IAddressRepository
{
    private readonly IDatabaseConnection _connection;

    public AddressRepositoryDatabase(IDatabaseConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<ICollection<Address>> GetAllAsync(CancellationToken cancellationToken)
    {
        var addresses = await _connection.Query<Address>("select * from addresses", null, MapAddress);
        return addresses.ToList();
    }

    public async Task<ICollection<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var addresses = await _connection.Query<Address>("select * from addresses where customer_id = @CustomerId", new { CustomerId = customerId }, MapAddress);
        return addresses.ToList();
    }

    public async Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var addresses = await _connection.Query<Address>("select * from addresses where id = @Id", new { Id = id }, MapAddress);
        return addresses.FirstOrDefault();
    }

    public IQueryable<Address> GetQueryable(CancellationToken cancellationToken)
    {
         var addresses = _connection.Query<Address>("select * from addresses", null, MapAddress).Result;
        return addresses.AsQueryable();
    } 

    public async Task Add(Address entity)
    {
       await _connection.Query<Task>("insert into addresses (id, customer_id ,zip_code, street, neighborhood, number, complement, city, state, country) values (@Id, @CustomerId, @ZipCode, @Street, @Neighborhood, @Number, @Complement, @City, @State, @Country)", entity);
    }

    public async Task Update(Address entity)
    {
        await _connection.Query<Task>("update addresses set zip_code = @ZipCode, street = @Street, neighborhood = @Neighborhood, number = @Number, complement = @Complement, city = @City, state = @State, country = @Country) where id = @Id", entity);

    }

    public async Task Delete(Address entity)
    {
        await _connection.Query<Task>("delete from addresses where id = @Id", new { entity.Id });
    }

    private Address MapAddress(IDataReader reader)
    {
        return Address.Restore(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetGuid(reader.GetOrdinal("customer_id")),
            reader.GetString(reader.GetOrdinal("zip_code")),
            reader.GetString(reader.GetOrdinal("street")),
            reader.GetString(reader.GetOrdinal("neighborhood")),
            reader.GetString(reader.GetOrdinal("number")),
            reader.IsDBNull(reader.GetOrdinal("complement")) ? null : reader.GetString(reader.GetOrdinal("complement")),
            reader.GetString(reader.GetOrdinal("city")),
            reader.GetString(reader.GetOrdinal("state")),
            reader.GetString(reader.GetOrdinal("country"))
        );
    }
}
