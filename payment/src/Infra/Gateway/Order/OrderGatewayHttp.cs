using Application.Abstractions.Gateway;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Infra.Gateway.Order;

public class OrderGatewayHttp : IOrderGateway
{
    private const string APIKEYNAME = "ApiKey";
    private readonly string _apiKey;
    public OrderGatewayHttp(IConfiguration configuration)
    {
        _apiKey = configuration.GetValue<string>(APIKEYNAME) ?? throw new ArgumentException();
    }
    public async Task<AddressGatewayResponse> GetAddressById(Guid id)
    {
        using(HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add(APIKEYNAME, _apiKey);

            var response = await client.GetAsync($"http://localhost:5256/api/addresses/service/{id}");

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
            var response = await client.GetAsync($"http://localhost:5256/api/customers/{id}");

            string customerResponseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CustomerGatewayResponse>(customerResponseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
