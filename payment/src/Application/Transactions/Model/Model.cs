using RabbitMQ.Client;

namespace Application.Transactions.Model;
public record CreateOrderModel
{
    public string Code { get; init; }
    public bool Closed { get;  init;}
    public List<CreateOrderItemModel> Items { get; init; }
    public CreateCustomerModel Customer { get;  init;}
    public OrderPaymentType Payments {  get;  init;}

}

public record CreateOrderItemModel
{
    public double Amount { get;  init;}
    public string Description { get;  init;}
    public int Quantity { get;  init;}
    public string Code { get;  init;}
}

public record CreateCustomerModel
{
    public string Name { get;  init;}
    public string Email { get;  init;}
    public string  Document { get;  init;}
    public string Type { get; init; }
    public CreateAddressModel Address { get;  init;}
    public CreateCustomerPhoneModel Phone { get; init;}
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
    public string Line1 { get;  init; }
    public string Line2 { get; init; }
}

public record CreateCustomerPhoneModel
{
    public string CountryCode { get; init; }
    public string AreaCode { get; init; }
    public string Number { get; init; }
}

public abstract record OrderPaymentType(string PaymentMethod) { }

public record BilletPaymentType : OrderPaymentType
{
    public BilletPaymentType(string bank, string dueDate, string instructions, int installments)
        : base("boleto")
    {
        Bank = bank;
        DueDate = dueDate;
        Instructions = instructions;
        Installments = installments;
    }

    public string Bank { get; }
    public string DueDate { get; }
    public string Instructions { get; }
    public int Installments { get; } = 1;
}

public record CardPaymentType : OrderPaymentType
{
    public CardPaymentType(string cardType, string operationType, int installments, string cardToken) : base(cardType)
    {
        CardType = cardType;
        OperationType = operationType;
        Installments = installments;
        CardType = cardToken;
    }
    public string CardType { get; }
    public string OperationType { get; }
    public int Installments { get; }
    public string CardToken { get; }
}

public record PixPaymentType : OrderPaymentType
{
    public PixPaymentType(int expiresIn) : base("pix")
    {
        ExpiresIn = expiresIn;
    }
    public int ExpiresIn { get; init; }
}

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

public record PagarmeCreateOrderResponse(string Id);
