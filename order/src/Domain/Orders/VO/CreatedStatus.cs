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
        Order._status = new CanceledStatus(Order);
    }

    public override void Checkout()
    {
        Order._status = new WaitingPaymentStatus(Order);
    }

    public override void Approve()
    {
        throw new NotImplementedException();
    }


    public override void Reject()
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

    public override void PrepareForShipping()
    {
        throw new Exception("Invalid Status");
    }
}

