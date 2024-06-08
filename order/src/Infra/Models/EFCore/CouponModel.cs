using Domain.Coupons.Entity;

namespace Infra.Models.EFCore;

public class CouponModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Value { get; set; }
    public DateTime ExpiressAt { get; set; }

    public CouponModel(Guid id, string name, double value, DateTime expiressAt)
    {
        Id = id;
        Name = name;
        Value = value;
        ExpiressAt = expiressAt;
    }

    public static CouponModel FromAggregate(Coupon coupon)
    {
        return new CouponModel(coupon.Id, coupon.Name, coupon.Value, coupon.ExpiressAt);
    }

    public Coupon ToAggregate()
    {
        return new Coupon(Id, Name, Value, ExpiressAt);
    }
}
