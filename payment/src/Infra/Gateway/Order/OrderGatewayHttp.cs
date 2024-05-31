using Application.Abstractions.Gateway;
using System.Text.Json;
using System.Text;

namespace Infra.Gateway.Order;

public class OrderGatewayHttp : IOrderGateway
{
    public async Task<AddressGatewayResponse> GetAddressById(Guid id)
    {
        using(HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"https://localhost:7078/api/addresses/service/{id}");

            string addressResponseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<AddressGatewayResponse>(addressResponseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;   
        }
    }

    public async Task<CustomerGatewayResponse> GetCustomerById(Guid id)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"https://localhost:7078/api/customers/{id}");

            string customerResponseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CustomerGatewayResponse>(customerResponseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
