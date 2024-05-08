using Application.Abstractions.Gateway;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.Transactions;
using Application.Data;
using MediatR;
using Application.Abstractions.Queue;
using System.Text.Json;
using System.Text;

namespace Application.Transactions.MakePaymentRequest;

public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOrderGateway _customerGateway;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueue _queue;

    public CreatePaymentCommandHandler(IPaymentGateway paymentGateway, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IQueue queue, IOrderGateway customerGateway)
    {
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _queue = queue;
        _customerGateway = customerGateway;
    }

    public async Task<Result> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var customer = await _customerGateway.GetCustomerById(request.CustomerId);

        var address = await _customerGateway.GetAddressById(request.AddressId);

        var paymentRequest = new ProccessPaymentRequest(
            request.OrderId,
            request.Total,
            request.Items,
            request.CustomerId,
            customer.Name,
            customer.Email,
            customer.CPF,
            customer.Phone,
            request.PaymentType,
            request.CardToken,
            request.Installment,
            address.ZipCode,
            address.Number,
            address.Complement
        );

        var responsePaymentGateway = await _paymentGateway.ProccessPayment(paymentRequest);

        var total = request.Items.Sum(i => i.Amount * i.Quantity);

        var transaction = Transaction.Create(request.OrderId, request.CustomerId, Guid.Parse(responsePaymentGateway.Id), total, request.PaymentType);

        _transactionRepository.Add(transaction);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (request.PaymentType != "credit") await _queue.PublishAsync(new { request.OrderId, Url = responsePaymentGateway.PaymentUrl }, "payment.url");

        return Result.Success();
    }
}
