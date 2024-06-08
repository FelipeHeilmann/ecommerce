namespace Domain.Coupons.Entity;

public class Coupon
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public double Value { get; private set; }
    public DateTime ExpiressAt { get; private set; }

    public Coupon(Guid id, string name, double value, DateTime expiressAt)
    {
        Id = id;
        Name = name;
        Value = value;
        ExpiressAt = expiressAt;
    }

    public static Coupon Create(string name, double value, DateTime expiressAt)
    {
        if (expiressAt < DateTime.Now) throw new Exception("Invalid date");

        return new Coupon(Guid.NewGuid(), name, value, expiressAt); 
    }

    public double GetDiscountAmount(double total)
    {
        return (Value/100)*total;
    }
}

