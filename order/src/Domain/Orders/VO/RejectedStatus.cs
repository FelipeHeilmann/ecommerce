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
        throw new Exception("Invalid Status");
    }

    public override void Cancel()
    {
        throw new Exception("Invalid Status");
    }

    public override void Checkout()
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery()
    {
        throw new Exception("Invalid Status");
    }

    public override void Reject()
    {
        throw new Exception("Invalid Status");
    }

    public override void Ship()
    {
        throw new Exception("Invalid Status");
    }

    public override void PrepareForShipping()
    {
        throw new Exception("Invalid Status");
    }
}
