using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class RejectedStatus : OrderStatus
{
    public override string Value { get; set; }
    public RejectedStatus(Order order) : base(order)
    {
        Value = "payment_rejected";
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

    public override void Reject()
    {
        throw new NotImplementedException();
    }

    public override void Ship()
    {
        throw new NotImplementedException();
    }
}
