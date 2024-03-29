using Application.Abstractions.Gateway;
using Application.Abstractions.Queue;
using Application.Orders.Model;
using Domain.Orders.DomainEvents;
using System.Text.Json;
using System.Text;
using System.Threading;

namespace Infra.Gateway;

public class PaymentGateway : IPaymentGateway
{
    private readonly IQueue _queue;

    public PaymentGateway(IQueue queue)
    {
        _queue = queue;
    }

    public async Task<object> LongDurationTransaction(OrderPurchasedEvent events)
    {
        await _queue.PublishAsync(events, "order-purchased");

        return string.Empty;
    }

    public async Task<object> ShortDurationTransaction(OrderPurchasedEvent events)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonSerializer.Serialize(events), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://localhost:5129/api/transactions", content);

            string paymentResponseJson = await response.Content.ReadAsStringAsync();

            PaymentSystemTransactionResponse payment = JsonSerializer.Deserialize<PaymentSystemTransactionResponse>(paymentResponseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            return payment;
        }
    }
}
