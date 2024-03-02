using Application.Transactions.Model;
using Domain.Events;

namespace Application.Abstractions.Gateway;

public interface IPaymentGateway
{
    Task <PagarmeCreateOrderResponse> CreateOrder(OrderPurchasedEvent request);
}
