using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class CreatedStatus : OrderStatus
{
    public override string Value { get; set; }

    public CreatedStatus(Order order): base(order)
    {
        Value = "created";
    }

    public override void Cancel()
    {
        Order.Status = new CanceledStatus(Order);
    }

    public override void Checkout()
    {
        Order.Status = new WaitingPaymentStatus(Order);
    }

    public override void Approve()
    {
        throw new NotImplementedException();
    }


    public override void Refuse()
    {
        throw new Exception("Invalid Status");
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

