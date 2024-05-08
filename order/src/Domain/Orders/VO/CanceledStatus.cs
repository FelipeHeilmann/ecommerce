using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class CanceledStatus : OrderStatus
{
    public override string Value { get; set; }

    public CanceledStatus() : base()
    {
        Value = "canceled";
    }

    public override void Cancel(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Checkout(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Approve(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Refuse(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Ship(Order order)
    {
        throw new Exception("Invalid Status");
    }
}

