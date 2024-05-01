using Application.Gateway;
using System.Text.Json;
using System.Text;

namespace Infra.Gateway;

public class NotifyGatewayHttp : INotifyGateway
{

    public async Task SendWelcomeMail(string name, string email)
    {
       using(var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var body = new { name, email };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            await client.PostAsync("http://localhost:5130/api/notify/welcome", content);
        }
    }

    public async Task SendOrderCreatedMail(OrderCreatedMail order)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

            await client.PostAsync("http://localhost:5130/api/notify/order-created", content);
        }
    }
}
