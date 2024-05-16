using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class ShippedStatus : OrderStatus
{
    public override string Value { get; set; }

    public ShippedStatus(Order order) : base(order)
    {
        Value = "shipped";
    }

    public override void Cancel()
    {
        Order.Status = new CanceledStatus(Order);
    }

    public override void Checkout()
    {
        throw new Exception("Invalid Status");
    }

    public override void Approve()
    {
        throw new Exception("Invalid Status");
    }

    public override void Refuse()
    {
        throw new Exception("Invalid Status");
    }

    public override void Ship(  )
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery()
    {
        Order.Status = new DeliveredStatus(Order);
    }

}

