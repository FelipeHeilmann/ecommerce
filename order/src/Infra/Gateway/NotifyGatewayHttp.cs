using Application.Gateway;
using Domain.Orders;
using System.Text.Json;
using System.Text;

namespace Infra.Gateway;

public class NotifyGatewayHttp : INotifyGateway
{
    public async Task SendMail(string name, string email)
    {
       using(var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var body = new { name, email };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            await client.PostAsync("http://localhost:5130/api/notify/welcome", content);
        }
    }
}
