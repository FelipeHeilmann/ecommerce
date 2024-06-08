using Domain.Coupons.Entity;
using Domain.Shared;

namespace Domain.Coupons.Repository;

public interface ICouponRepository : IRepositoryBase<Coupon>
{
    public Task<Coupon?> GetByNameAsync(string name, CancellationToken cancellationToken);
}
