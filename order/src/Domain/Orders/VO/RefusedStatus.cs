using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class RefusedStatus : OrderStatus
{
    public override string Value { get; set; }
    public RefusedStatus(Order order) : base(order)
    {
        Value = "payment_refused";
    }

    public override void Approve()
    {
        throw new NotImplementedException();
    }

    public override void Cancel()
    {
        throw new NotImplementedException();
    }

    public override void Checkout()
    {
        throw new NotImplementedException();
    }

    public override void Delivery()
    {
        throw new NotImplementedException();
    }

    public override void Refuse()
    {
        throw new NotImplementedException();
    }

    public override void Ship()
    {
        throw new NotImplementedException();
    }
}
