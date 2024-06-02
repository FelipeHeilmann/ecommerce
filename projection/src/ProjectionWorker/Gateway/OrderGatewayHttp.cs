using System.Text.Json;

namespace ProjectionWorker.Gateway;

public class OrderGatewayHttp : IOrderGeteway
{
    public async Task<AddressGatewayResponse> GetAddressById(Guid id)
    {
        using (HttpClient client = new HttpClient()) 
        {
            var response = await client.GetAsync($"https://localhost:7078/api/addresses/service/{id}");
            string addressResponseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AddressGatewayResponse>(addressResponseJson, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true
            })!;
        }
    }

    public async Task<ProductGatewayResponse> GetProductById(Guid id)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"https://localhost:7078/api/products/{id}");

            string productResponseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ProductGatewayResponse>(productResponseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
