namespace Domain.Coupons.Entity;

public class Coupon
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public double Value { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    private Coupon(Guid id, string name, double value, DateTime expiresAt)
    {
        Id = id;
        Name = name;
        Value = value;
        ExpiresAt = expiresAt;
    }

    public static Coupon Create(string name, double value, DateTime expiresAt)
    {
        if (expiresAt < DateTime.Now) throw new Exception("Invalid date");

        return new Coupon(Guid.NewGuid(), name, value, expiresAt); 
    }

    public static Coupon Restore(Guid id, string name, double value, DateTime expiresAt)
    {
        return new Coupon(id, name, value, expiresAt);
    }

    public double GetDiscountAmount(double total)
    {
        return Value/100*total;
    }

    public bool IsValid()
    {
         return ExpiresAt < DateTime.Now;
    }
}

