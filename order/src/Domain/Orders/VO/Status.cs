using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public abstract class OrderStatus
{
    public abstract string Value { get; set; }

    public abstract void Checkout(Order order);
    public abstract void Approve(Order order);
    public abstract void Refuse(Order order);
    public abstract void Ship(Order order);
    public abstract void Delivery(Order order);
    public abstract void Cancel(Order order);
}

public class OrderStatusFactory
{
    public static OrderStatus Create(string status)
    {
        if (status == "created") return new CreatedStatus();
        if (status == "waiting_payment") return new WaitingPaymentStatus();
        if (status == "payment_approved") return new ApprovedStatus();
        if (status == "payment_refused") return new RefusedStatus();
        if (status == "shipped") return new ShippedStatus();
        if (status == "delivered") return new DeliveredStatus();
        if (status == "canceled") return new CanceledStatus();
        throw new Exception();
    }
}

