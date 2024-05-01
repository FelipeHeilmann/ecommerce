using Application.Abstractions.Gateway;
using Application.Abstractions.Queue;
using Application.Orders.Model;
using Domain.Orders.DomainEvents;
using System.Text.Json;
using System.Text;

namespace Infra.Gateway;

public class PaymentGatewayHttp : IPaymentGateway
{
    private readonly IQueue _queue;

    public PaymentGatewayHttp(IQueue queue)
    {
        _queue = queue;
    }

    public async Task<object> ProccessPayment(OrderPurchasedEvent order)
    {
        if(order.PaymentType == "credit")
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5129/api/transactions", content);

                string paymentResponseJson = await response.Content.ReadAsStringAsync();

                PaymentSystemTransactionResponse payment = JsonSerializer.Deserialize<PaymentSystemTransactionResponse>(paymentResponseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

                return payment;
            }
        }

        await _queue.PublishAsync(order, "order-purchased");

        return string.Empty;
    }

}
