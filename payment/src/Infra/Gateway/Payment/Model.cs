using Microsoft.Extensions.Primitives;

namespace Infra.Gateway.Payment;

public record CreateOrderModel
{
    public string Code { get; init; } = string.Empty;
    public bool Closed { get; init; }
    public List<CreateOrderItemModel> Items { get; init; } = new List<CreateOrderItemModel>();
    public CreateCustomerModel Customer { get; init; } = new CreateCustomerModel();
    public List<OrderPaymentType> Payments { get; init; } = new List<OrderPaymentType>();
}

public record CreateOrderItemModel
{
    public double Amount { get; init; }
    public string Description { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public string Code { get; init; }  = string.Empty;
}

public record CreateCustomerModel
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Document { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public CreateAddressModel Address { get; init; } = new CreateAddressModel();
    public CreateCustomerPhoneModel Phone { get; init; } = new CreateCustomerPhoneModel();
}
public record CreateAddressModel
{
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Neighborhood { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string Complement { get; init; } = string.Empty;
    public string Line1 { get; init; } = string.Empty;
    public string Line2 { get; init; } = string.Empty;
}

public record CreateCustomerPhoneModel
{
    public string CountryCode { get; init; } = string.Empty;
    public string AreaCode { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}

public class OrderPaymentType
{
    public CreditCard? CreditCard { get; set; }
    public Boleto? Boleto { get; set; }
    public Pix? Pix { get; set; }
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
        PaymentType = "pix";
        Pix = pix ?? throw new ArgumentNullException(nameof(pix));
    }
}

public record CreditCard(string StatementDescriptor, string OperationType, int Intallments,string CardToken);

public record Boleto(string Bank, string DueAt ,string Instructions);

public record Pix(int expiresIn);

public record CepAPIResponse
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public string Gia { get; set; } = string.Empty;
    public string Ddd { get; set; } = string.Empty;
    public string Siafi { get; set; } = string.Empty;
}

public record PaymentAPIResponse 
{
    public Guid Id { get; init; }
    public Guid Code { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public required Payment Payment { get; init; }
    public bool Closed { get; init; }
    public List<CreateOrderItemModel> Items { get; init; } = new List<CreateOrderItemModel>();
    public CreateCustomerModel Customer { get; init; } = new CreateCustomerModel();
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime ClosedAt { get; init; }
}

public record Payment(string PaymentType, string PaymentUrl);

