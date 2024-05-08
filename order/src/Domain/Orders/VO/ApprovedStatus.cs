using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class ApprovedStatus : OrderStatus
{
    public override string Value { get ; set ; }

    public ApprovedStatus() : base()
    {
        Value = "payment_approved";
    }

    public override void Checkout(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Approve(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Refuse(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Ship(Order order)
    {
        order.Status = new ShippedStatus();
    }

    public override void Delivery(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Cancel(Order order)
    {
        order.Status = new CanceledStatus();
    }
}
