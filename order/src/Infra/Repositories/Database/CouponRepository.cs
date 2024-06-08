using Domain.Coupons.Entity;
using Domain.Coupons.Repository;
using Infra.Context;
using Infra.Models.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class CouponRepository : ICouponRepository
{
    private readonly ApplicationContext _context;
    public CouponRepository(ApplicationContext context)
    {
        _context = context;
    }

    public IQueryable<Coupon> GetQueryable(CancellationToken cancellationToken)
    {
        var coupons = new List<Coupon>();
        foreach (var couponModel in _context.Set<CouponModel>().ToList())
        {
            coupons.Add(couponModel.ToAggregate());
        }
        return coupons.AsQueryable();
    }


    public async Task<ICollection<Coupon>> GetAllAsync(CancellationToken cancellationToken)
    {
        var coupons = new List<Coupon>();
        foreach (var couponModel in await _context.Set<CouponModel>().ToListAsync())
        {
            coupons.Add(couponModel.ToAggregate());
        }
        return coupons.ToList();
    }

    public async Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var couponModel = await _context.Set<CouponModel>().FirstOrDefaultAsync(model => model.Id == id, cancellationToken);
        return couponModel?.ToAggregate();
    }

    public async Task<Coupon?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var couponModel = await _context.Set<CouponModel>().FirstOrDefaultAsync(model => model.Name == name, cancellationToken);
        return couponModel?.ToAggregate();
    }

   
    public void Add(Coupon entity)
    {
        _context.Set<CouponModel>().Add(CouponModel.FromAggregate(entity));
    }

    public void Update(Coupon entity)
    {
        _context.Set<CouponModel>().Update(CouponModel.FromAggregate(entity));
    }

    public void Delete(Coupon entity)
    {
        _context.Set<CouponModel>().Remove(CouponModel.FromAggregate(entity));
    }
}
