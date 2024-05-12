using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class CartStatus : OrderStatus
{
    public override string Value { get; set ; }

    public CartStatus()
    {
        Value = "cart";
    }

    public override void Approve(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Cancel(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Checkout(Order order)
    {
        order.Status = new WaitingPaymentStatus();
    }

    public override void Delivery(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Refuse(Order order)
    {
        throw new NotImplementedException();
    }

    public override void Ship(Order order)
    {
        throw new NotImplementedException();
    }
}
