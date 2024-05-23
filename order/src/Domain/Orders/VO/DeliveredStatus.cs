using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class DeliveredStatus : OrderStatus
{
    public override string Value { get; set; }

    public DeliveredStatus(Order order) : base(order)
    {
        Value = "delivered";
    }

    public override void Cancel()
    {
        Order._status = new CanceledStatus(Order);
    }

    public override void Checkout()
    {
        throw new NotImplementedException();
    }

    public override void Approve()
    {
        throw new NotImplementedException();
    }

    public override void Refuse()
    {
        throw new NotImplementedException();
    }

    public override void Delivery()
    {
        throw new NotImplementedException();
    }

    public override void Ship()
    {
        throw new NotImplementedException();
    }
}

