using Domain.Coupons.Entity;
using Domain.Coupons.Repository;

namespace Infra.Repositories.Memory;

public class CouponRepositoryMemory : ICouponRepository
{
    private List<Coupon> coupons;

    public CouponRepositoryMemory()
    {
        coupons = [];
    }

    public IQueryable<Coupon> GetQueryable(CancellationToken cancellationToken)
    {
        return coupons.AsQueryable();
    }

    public Task<ICollection<Coupon>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollection<Coupon>>(coupons);
    }

    public Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(coupons.First(coupon => coupon.Id == id) ?? null);
    }

    public Task<Coupon?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return Task.FromResult(coupons.First(coupon => coupon.Name == name) ?? null);
    }

    public void Add(Coupon entity)
    {
        coupons.Add(entity);
    }

    public void Update(Coupon entity)
    {
        var index = coupons.FindIndex(coupon => coupon.Id == entity.Id);
        if (index == -1)
        {
            return;
        }
        coupons[index] = entity;
    }

    public void Delete(Coupon entity)
    {
        coupons.Remove(entity);
    }
}
