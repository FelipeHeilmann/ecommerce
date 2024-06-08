using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class ApprovedStatus : OrderStatus
{
    public override string Value { get ; set ; }

    public ApprovedStatus(Order order) : base(order)
    {
        Value = "payment_approved";
    }

    public override void Checkout()
    {
        throw new Exception("Invalid Status");
    }

    public override void Approve()
    {
        throw new Exception("Invalid Status");
    }

    public override void Reject()
    {
        throw new Exception("Invalid Status");
    }

    public override void Prepare()
    {
        Order._status = new InPreparationStatus(Order);
    }

    public override void Ship()
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery()
    {
        throw new Exception("Invalid Status");
    }

    public override void Cancel()
    {
        Order._status = new CanceledStatus(Order);
    }   
}
