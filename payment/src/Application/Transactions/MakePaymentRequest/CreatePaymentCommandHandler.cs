using Application.Abstractions.Gateway;
using Application.Abstractions.Messaging;
using Application.Transactions.Model;
using Application.Transactions;
using Domain.Shared;
using System.Text.Json;
using Domain.Transactions;
using Application.Data;

namespace Application.Transactions.MakePaymentRequest;

public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ITransactionRepository _transactionRepository;
    private IUnitOfWork _unitOfWork;

    public CreatePaymentCommandHandler(IPaymentGateway paymentGateway, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        HttpClient client = new HttpClient();

        HttpResponseMessage zipCodeResponse = await client.GetAsync($"https://viacep.com.br/ws/{request.AddressZipCode}/json/");

        string jsonResponse = await zipCodeResponse.Content.ReadAsStringAsync();

        CepAPIResponse cepInfo = JsonSerializer.Deserialize<CepAPIResponse>(jsonResponse)!;


        var body = new CreateOrderModel()
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
                    Number = request.CustomerPhone.Substring(3)
                }
            },
            Payments = GetPaymentType(request.PaymentType, request.Installment, request.CardToken),
            Items = request.Items.Select(i => new CreateOrderItemModel() { Amount = i.Amount, Quantity = i.Quantity, Code = i.Id.ToString(), Description = "Produto" }).ToList(),
            Closed = true
        };

        var response = await _paymentGateway.CreateOrder(body);

        var total = request.Items.Sum(i => i.Amount * i.Quantity);

        var transaction = Transaction.Create(request.OrderId, request.CustomerId, Guid.Parse(response.Id), total, request.PaymentType);

        _transactionRepository.Add(transaction);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private OrderPaymentType GetPaymentType(string type, int installments, string? cardToken = null)
    {
        if (type == "credit") return new CardPaymentType("credit", "auth_capture", installments, cardToken!);
        if (type == "debit") return new CardPaymentType("credit", "auth_capture", 1, cardToken!);
        if (type == "pix") return new PixPaymentType(1000 * 1 * 60 * 30);
        if (type == "billet") return new BilletPaymentType("104", new DateTime().AddDays(7).ToLongDateString(), "pagar até a data", 1);

        throw new ArgumentException("Invalid payment type");
    }
}
