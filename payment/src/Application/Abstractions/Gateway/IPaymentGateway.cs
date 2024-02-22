using Application.Transactions.Model;

namespace Application.Abstractions.Gateway;

public interface IPaymentGateway
{
    Task <PagarmeCreateOrderResponse> CreateOrder(CreateOrderModel request);
}
