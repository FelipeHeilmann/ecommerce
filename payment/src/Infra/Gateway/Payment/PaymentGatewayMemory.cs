using Application.Abstractions.Gateway;
using Application.Transactions.Model;
using Domain.Events;
using System.Text;
using System.Text.Json;

namespace Infra.Gateway.Payment;

public class PaymentGatewayMemory : IPaymentGateway
{
    public async Task<PagarmeCreateOrderResponse> CreateOrder(OrderPurchasedEvent request)
    {
        HttpClient client = new HttpClient();

        HttpResponseMessage zipCodeResponse = await client.GetAsync($"https://viacep.com.br/ws/{request.AddressZipCode}/json/");

        string jsonResponse = await zipCodeResponse.Content.ReadAsStringAsync();

        CepAPIResponse cepInfo = JsonSerializer.Deserialize<CepAPIResponse>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        var body = JsonSerializer.Serialize(new CreateOrderModel()
        {
            Code = request.OrderId.ToString(),
            Customer = new CreateCustomerModel()
            {
                Name = request.CustomerName,
                Email = request.CustomerEmail,
                Document = request.CustomerDocument,
                Type = "individual",
                Address = new CreateAddressModel()
                {
                    City = cepInfo.Localidade,
                    Complement = request.AddressLine ?? string.Empty,
                    Country = "Brasil",
                    Line1 = "",
                    Line2 = "",
                    Neighborhood = cepInfo.Bairro,
                    Number = request.AddressNumber,
                    State = cepInfo.Uf,
                    Street = cepInfo.Logradouro,
                    ZipCode = request.AddressZipCode
                },
                Phone = new CreateCustomerPhoneModel()
                {
                    CountryCode = "55",
                    AreaCode = request.CustomerPhone.Substring(0, 2),
                    Number = request.CustomerPhone.Substring(2)
                }
            },
            Payments = new List<OrderPaymentType>() { GetPaymentType(request.PaymentType, request.Installment, request.CardToken) },
            Items = request.Items.Select(i => new CreateOrderItemModel() { Amount = i.Amount, Quantity = i.Quantity, Code = i.Id.ToString(), Description = "Produto" }).ToList(),
            Closed = true
        });


        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(body, Encoding.UTF8, "application/json");
      
        var response = await client.PostAsync("http://localhost:3333/payment-gateway/transactions", content);

        return new PagarmeCreateOrderResponse(Guid.NewGuid().ToString());
    }

    private OrderPaymentType GetPaymentType(string type, int installments, string? cardToken = null)
    {
        if (type == "credit") return new OrderPaymentType(new CreditCard("ecommerce", "auth_capture", installments, cardToken!));
        if (type == "pix") return new OrderPaymentType(new Pix(1000 * 1 * 60 * 30));
        if (type == "billet") return new OrderPaymentType(new Boleto("104", new DateTime().AddDays(7).ToLongDateString(), "pagar até a data"));

        throw new ArgumentException("Invalid payment type");
    }
}
