using Domain.Orders.Entity;

namespace Domain.Orders.VO;

internal class InPreparationStatus : OrderStatus
{
    public override string Value { get; set ; }

    public InPreparationStatus(Order order): base(order) 
    {
        Value = "in_preparation";
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
        Order._status = new ShippedStatus(Order);
    }

    public override void Prepare()
    {
        throw new Exception("Invalid Status");
    }
}
