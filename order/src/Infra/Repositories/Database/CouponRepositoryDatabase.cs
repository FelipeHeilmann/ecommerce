using System.Data;
using Domain.Coupons.Entity;
using Domain.Coupons.Repository;
using Infra.Database;

namespace Infra.Repositories.Database;

public class CouponRepositoryDatabase : ICouponRepository
{
    private readonly IDatabaseConnection _connection;

    public CouponRepositoryDatabase(IDatabaseConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<ICollection<Coupon>> GetAllAsync(CancellationToken cancellationToken)
    {
       var coupons = await _connection.Query("select * from coupons", null, MapCoupon);
       return coupons.ToList();
    }

    public async Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var coupon = await _connection.Query("select * from coupons where id = @Id", new { Id = id }, MapCoupon);
        return coupon.FirstOrDefault();
    }

    public async Task<Coupon?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var coupon = await _connection.Query("select * from coupons where name = @Name", new { Name = name }, MapCoupon);
        return coupon.FirstOrDefault();
    }

    public IQueryable<Coupon> GetQueryable(CancellationToken cancellationToken)
    {
        var coupons = _connection.Query("select * from coupons", null, MapCoupon).Result;
        return coupons.AsQueryable();
    }

    public async Task Add(Coupon entity)
    {
        await _connection.Query<Task>("insert into coupons (id, name, value, expires_at) values (@Id, @Name, @Value, @ExpiresAt)", entity);
    }

    public async Task Update(Coupon entity)
    {
        await _connection.Query<Task>("update coupons set name = @Name, value  @Value, expires_at = @ExpiresAt where id = @Id", entity);
    }

    public async Task Delete(Coupon entity)
    {
        await _connection.Query<Task>("delete from coupons where id = @Id", new { entity.Id });
    }

    private Coupon MapCoupon(IDataReader reader)
    {
        return Coupon.Restore(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("name")),
            reader.GetDouble(reader.GetOrdinal("value")),
            reader.GetDateTime(reader.GetOrdinal("expires_at"))
        );
    }
}
