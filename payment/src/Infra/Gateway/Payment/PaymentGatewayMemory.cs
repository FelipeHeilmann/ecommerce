using Application.Abstractions.Gateway;
using Application.Transactions.Model;

namespace Infra.Gateway.Payment;

public class PaymentGatewayMemory : IPaymentGateway
{
    public Task<PagarmeCreateOrderResponse> CreateOrder(CreateOrderModel request)
    {
        return Task.FromResult(new PagarmeCreateOrderResponse(Guid.NewGuid().ToString()));
    }
}
