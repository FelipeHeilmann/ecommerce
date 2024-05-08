using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class WaitingPaymentStatus : OrderStatus
{
    public override string Value { get; set; }

    public WaitingPaymentStatus()
    {
        Value = "waiting_payment";
    }

    public override void Cancel(Order order)
    {
        order.Status = new CanceledStatus();
    }

    public override void Checkout(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Approve(Order order)
    {
        order.Status = new ApprovedStatus();
    }

    public override void Refuse(Order order)
    {
        order.Status = new RefusedStatus();
    }

    public override void Ship(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery(Order order)
    {
        throw new Exception("Invalid Status");
    }
}

