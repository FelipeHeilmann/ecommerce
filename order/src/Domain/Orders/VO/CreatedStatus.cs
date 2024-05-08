using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class CreatedStatus : OrderStatus
{
    public override string Value { get; set; }

    public CreatedStatus()
    {
        Value = "created";
    }

    public override void Cancel(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Checkout(Order order)
    {
        order.Status = new WaitingPaymentStatus();
    }

    public override void Approve(Order order)
    {
        throw new NotImplementedException();
    }


    public override void Refuse(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Ship(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override void Delivery(Order order)
    {
        throw new Exception("Invalid Status");
    }

    public override string ToString()
    {
        return "created";
    }
}

