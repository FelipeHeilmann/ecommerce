using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public class CartStatus : OrderStatus
{
    public override string Value { get; set ; }

    public CartStatus(Order order) : base(order) 
    {
        Value = "cart";
    }

    public override void Approve()
    {
        throw new NotImplementedException();
    }

    public override void Cancel()
    {
        throw new NotImplementedException();
    }

    public override void Checkout()
    {
        Order._status = new WaitingPaymentStatus(Order);
    }

    public override void Delivery()
    {
        throw new NotImplementedException();
    }

    public override void Reject()
    {
        throw new NotImplementedException();
    }

    public override void Ship()
    {
        throw new NotImplementedException();
    }
}
