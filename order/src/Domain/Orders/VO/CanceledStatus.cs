using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class CanceledStatus : OrderStatus
{
    public override string Value { get; set; }

    public CanceledStatus(Order order) : base(order)
    {
        Value = "canceled";
    }

    public override void Cancel()
    {
        throw new Exception("Invalid Status");
    }

    public override void Checkout()
    {
        throw new Exception("Invalid Status");
    }

    public override void Approve(   )
    {
        throw new NotImplementedException();
    }

    public override void Reject()
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery()
    {
        throw new Exception("Invalid Status");
    }

    public override void Prepare()
    {
        throw new Exception("Invalid Status");
    }

    public override void Ship()
    {
        throw new Exception("Invalid Status");
    }
}

