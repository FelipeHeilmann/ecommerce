using Microsoft.Extensions.Primitives;

namespace Infra.Gateway.Payment;

public record CreateOrderModel
{
    public string Code { get; init; }
    public bool Closed { get; init; }
    public List<CreateOrderItemModel> Items { get; init; }
    public CreateCustomerModel Customer { get; init; }
    public List<OrderPaymentType> Payments { get; init; }
}

public record CreateOrderItemModel
{
    public double Amount { get; init; }
    public string Description { get; init; }
    public int Quantity { get; init; }
    public string Code { get; init; }
}

public record CreateCustomerModel
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Document { get; init; }
    public string Type { get; init; }
    public CreateAddressModel Address { get; init; }
    public CreateCustomerPhoneModel Phone { get; init; }
}
public record CreateAddressModel
{
    public string Street { get; init; }
    public string Number { get; init; }
    public string ZipCode { get; init; }
    public string Neighborhood { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
    public string Complement { get; init; }
    public string Line1 { get; init; }
    public string Line2 { get; init; }
}

public record CreateCustomerPhoneModel
{
    public string CountryCode { get; init; }
    public string AreaCode { get; init; }
    public string Number { get; init; }
}

public class OrderPaymentType
{
    public CreditCard CreditCard { get; set; }
    public Boleto Boleto { get; set; }
    public Pix Pix { get; set; }

    public string PaymentType { get; set; }

    public OrderPaymentType(CreditCard creditCard)
    {
        PaymentType = "credit_card";
        CreditCard = creditCard ?? throw new ArgumentNullException(nameof(creditCard));
    }

    public OrderPaymentType(Boleto boleto)
    {
        PaymentType = "boleto";
        Boleto = boleto ?? throw new ArgumentNullException(nameof(boleto));
    }

    public OrderPaymentType(Pix pix)
    {
        PaymentType = "Pix";
        Pix = pix ?? throw new ArgumentNullException(nameof(pix));
    }
}

public record CreditCard(string StatementDescriptor, string OperationType, int Intallments,string CardToken);

public record Boleto(string Bank, string DueAt ,string Instructions);

public record Pix(int expiresIn);

public record CepAPIResponse
{
    public string Cep { get; set; }
    public string Logradouro { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Localidade { get; set; }
    public string Uf { get; set; }
    public string Ibge { get; set; }
    public string Gia { get; set; }
    public string Ddd { get; set; }
    public string Siafi { get; set; }
}

public record PaymentAPIResponse 
{
    public Guid Id { get; init; }
    public Guid Code { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public Payment Payment { get; init; }
    public bool Closed { get; init; }
    public List<CreateOrderItemModel> Items { get; init; }
    public CreateCustomerModel Customer { get; init; }
    public string Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime ClosedAt { get; init; }
}

public record Payment(string PaymentType, string? PaymentUrl);

