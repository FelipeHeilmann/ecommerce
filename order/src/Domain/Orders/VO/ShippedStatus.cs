using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class ShippedStatus : OrderStatus
{
    public override string Value { get; set; }

    public ShippedStatus() : base()
    {
        Value = "shipped";
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
        throw new Exception("Invalid Status");
    }

    public override void Refuse(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Ship(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery(Order order)
    {
        order.Status = new DeliveredStatus();
    }

}

