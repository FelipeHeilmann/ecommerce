﻿using Application.Abstractions.Gateway;
using Application.Abstractions.Messaging;
using Domain.Shared;
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

        var response = await _paymentGateway.CreateOrder(request);

        var total = request.Items.Sum(i => i.Amount * i.Quantity);

        var transaction = Transaction.Create(request.OrderId, request.CustomerId, Guid.Parse(response.Id), total, request.PaymentType);

        _transactionRepository.Add(transaction);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
