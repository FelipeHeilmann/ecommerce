using Application.Abstractions.Gateway;
using Application.Transactions.Model;
using Domain.Events;
using System.Text;
using System.Text.Json;

namespace Infra.Gateway.Payment;

public class PaymentGatewayFake : IPaymentGateway
{
    public async Task<PaymentGatewayResponse> ProccessPayment(ProccessPaymentRequest request)
    {
        HttpClient client = new HttpClient();

 

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
                    City = request.AddressCity,
                    Complement = request.AddressComplement ?? "",
                    Country = request.AddressCountry,
                    Line1 = "",
                    Line2 = "",
                    Neighborhood = request.AddressNeighborhood,
                    Number = request.AddressNumber,
                    State = request.AddressState,
                    Street = request.AddressStreet,
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
            Items = request.Items.Select(i => new CreateOrderItemModel() { Amount = i.Price, Quantity = i.Quantity, Code = i.Id.ToString(), Description = "Produto" }).ToList(),
            Closed = true
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });


        var paymentId = Guid.NewGuid();
        var paymentUrl = "https://www.instagram.com/p/C38mWMeMSn7/?utm_source=ig_embed&ig_rid=e61afe49-0878-41c0-9b1e-b17f00f5cf76";

        return new PaymentGatewayResponse((paymentId.ToString()), paymentUrl);
    }


    private OrderPaymentType GetPaymentType(string type, int installments, string? cardToken = null)
    {
        if (type == "credit") return new OrderPaymentType(new CreditCard("ecommerce", "auth_capture", installments, cardToken!));
        if (type == "pix") return new OrderPaymentType(new Pix(1000 * 1 * 60 * 30));
        if (type == "billet") return new OrderPaymentType(new Boleto("104", new DateTime().AddDays(7).ToLongDateString(), "pagar até a data"));

        throw new ArgumentException("Invalid payment type");
    }
}
