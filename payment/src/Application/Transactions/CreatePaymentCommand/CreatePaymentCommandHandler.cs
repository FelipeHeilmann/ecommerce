﻿using Application.Abstractions.Gateway;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Application.Abstractions.Queue;
using Domain.Transactions.Repository;
using Domain.Transactions.Entity;

namespace Application.Transactions.CreatePaymentCommand;

public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand, Guid>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOrderGateway _orderGateway;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IQueue _queue;

    public CreatePaymentCommandHandler(IPaymentGateway paymentGateway, ITransactionRepository transactionRepository, IQueue queue, IOrderGateway orderGateway)
    {
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _queue = queue;
        _orderGateway = orderGateway;
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var customer = await _orderGateway.GetCustomerById(request.CustomerId);

        var address = await _orderGateway.GetAddressById(request.AddressId);

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
            request.CardToken ?? "",
            request.Installment,
            address.ZipCode,
            address.Number,
            address.Street,
            address.State,
            address.Neighborhood,
            address.Complement,
            address.City,
            address.Country
        );

        var responsePaymentGateway = await _paymentGateway.ProccessPayment(paymentRequest);

        var total = request.Items.Sum(i => i.Price * i.Quantity);

        var transaction = Transaction.Create(request.OrderId, request.CustomerId, Guid.Parse(responsePaymentGateway.Id), total, request.PaymentType);

        await _transactionRepository.Add(transaction);

        await _queue.PublishAsync(new { request.OrderId, Url = responsePaymentGateway.PaymentUrl, request.PaymentType }, "payment.url");
        
        return transaction.Id;
    }
}
