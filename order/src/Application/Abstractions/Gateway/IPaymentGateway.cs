using Domain.Orders.DomainEvents;

namespace Application.Abstractions.Gateway;

public interface IPaymentGateway
{
    Task<object> LongDurationTransaction(OrderPurchasedEvent events);
    Task<object> ShortDurationTransaction(OrderPurchasedEvent events);
}
