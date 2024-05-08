using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class RefusedStatus : OrderStatus
{
    public override string Value { get; set; }
    public RefusedStatus() : base()
    {
        Value = "payment_refused";
    }

    public override void Approve(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Cancel(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Checkout(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Delivery(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Refuse(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Ship(Order order)
    {
        throw new NotImplementedException();
    }
}
