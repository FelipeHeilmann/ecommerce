using Domain.Orders.Entity;

namespace Domain.Orders.VO;

public abstract class OrderStatus
{
    public abstract string Value { get; set; }
    public Order Order { get; set; }
    public OrderStatus(Order order ) { this.Order = order; }
    public abstract void Checkout();
    public abstract void Approve();
    public abstract void Refuse();
    public abstract void Ship();
    public abstract void Delivery();
    public abstract void Cancel();
}

public class OrderStatusFactory
{
    public static OrderStatus Create(Order order,string status)
    {
        if (status == "cart") return new CartStatus(order);
        if (status == "created") return new CreatedStatus(order);
        if (status == "waiting_payment") return new WaitingPaymentStatus(order);
        if (status == "payment_approved") return new ApprovedStatus(order);
        if (status == "payment_refused") return new RefusedStatus(order);
        if (status == "shipped") return new ShippedStatus(order);
        if (status == "delivered") return new DeliveredStatus(order);
        if (status == "canceled") return new CanceledStatus(order);
        throw new Exception();
    }
}

