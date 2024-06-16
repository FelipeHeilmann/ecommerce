using System.Text.Json;

namespace ProjectionWorker.Gateway;

public class OrderGatewayHttp : IOrderGeteway
{
    private const string APIKEYNAME = "ApiKey";
    private readonly string _apiKey;

    public OrderGatewayHttp(IConfiguration configuration)
    {
        _apiKey = configuration.GetValue<string>(APIKEYNAME) ?? throw new ArgumentException();
    }

    public async Task<AddressGatewayResponse> GetAddressById(Guid id)
    {
        using (HttpClient client = new HttpClient()) 
        {
            client.DefaultRequestHeaders.Add(APIKEYNAME, _apiKey);

            var response = await client.GetAsync($"http://localhost:5256/api/addresses/service/{id}");

            if (!response.IsSuccessStatusCode) throw new Exception(response.Content.ToString());

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
            var response = await client.GetAsync($"http://localhost:5256/api/products/{id}");

            string productResponseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ProductGatewayResponse>(productResponseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
