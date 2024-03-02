using Application.Abstractions.Gateway;
using Application.Transactions.Model;

namespace Infra.Queue;

public class MemoryGateway : IPaymentGateway
{
    public Task<PagarmeCreateOrderResponse> CreateOrder(CreateOrderModel request)
    {
        return Task.FromResult(new PagarmeCreateOrderResponse(Guid.NewGuid().ToString()));
    }
}
