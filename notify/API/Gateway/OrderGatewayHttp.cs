using System.Text.Json;

namespace API.Gateway
{
    public class OrderGatewayHttp : IOrderGateway
    {
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

}
