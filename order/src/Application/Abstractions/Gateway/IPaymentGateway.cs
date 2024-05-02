using Domain.Orders;

namespace Application.Abstractions.Gateway;

public interface IPaymentGateway
{
    Task<object> ProccessPayment(OrderPurchasedEvent events);
}
