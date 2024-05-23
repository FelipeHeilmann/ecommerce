using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class WaitingPaymentStatus : OrderStatus
{
    public override string Value { get; set; }

    public WaitingPaymentStatus(Order order): base(order)
    {
        Value = "waiting_payment";
    }

    public override void Cancel()
    {
        Order._status = new CanceledStatus(Order);
    }

    public override void Checkout()
    {
        throw new Exception("Invalid Status");
    }

    public override void Approve()
    {
        Order._status = new ApprovedStatus(Order);
    }

    public override void Refuse()
    {
        Order._status = new RefusedStatus(Order);
    }

    public override void Ship()
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery()
    {
        throw new Exception("Invalid Status");
    }
}

