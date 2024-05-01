using Application.Gateway;
using System.Text.Json;
using System.Text;

namespace Infra.Gateway.Nofify
{
    public class NotifyGatewayHttp : INotifyGateway
    {
        public async Task SendPaymentRecivedMail(PaymenRecivedRequest payment)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");

                await client.PostAsync("http://localhost:5130/api/notify/payment-recived", content);
            }
        }
    }
}
