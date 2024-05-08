using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class DeliveredStatus : OrderStatus
{
    public override string Value { get; set; }

    public DeliveredStatus() : base()
    {
        Value = "delivered";
    }

    public override void Cancel(Order order)
    {
        order.Status = new CanceledStatus();
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

    public override void Delivery(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Ship(Order order)
    {
        throw new NotImplementedException();
    }
}

