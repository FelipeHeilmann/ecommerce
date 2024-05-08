using Application.Transactions.Model;
using Domain.Events;

namespace Application.Abstractions.Gateway;

public interface IPaymentGateway
{
    Task <PaymentGatewayResponse> ProccessPayment(ProccessPaymentRequest request);
}
