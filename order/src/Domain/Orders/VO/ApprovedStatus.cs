using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class ApprovedStatus : OrderStatus
{
    public override string Value { get ; set ; }

    public ApprovedStatus(Order order) : base(order)
    {
        Value = "payment_approved";
    }

    public override void Checkout()
    {
        throw new NotImplementedException();
    }

    public override void Approve()
    {
        throw new NotImplementedException();
    }

    public override void Reject()
    {
        throw new NotImplementedException();
    }

    public override void Ship()
    {
        Order._status = new ShippedStatus(Order);
    }

    public override void Delivery()
    {
        throw new NotImplementedException();
    }

    public override void Cancel()
    {
        Order._status = new CanceledStatus(Order);
    }
}
