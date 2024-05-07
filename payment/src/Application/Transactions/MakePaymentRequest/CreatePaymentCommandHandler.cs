using Application.Abstractions.Gateway;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.Transactions;
using Application.Data;
using MediatR;
using Application.Abstractions.Queue;

namespace Application.Transactions.MakePaymentRequest;

public class CreatePaymentCommandHandler : ICommandHandler<CreatePaymentCommand>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IMediator _mediator;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueue _queue;

    public CreatePaymentCommandHandler(IPaymentGateway paymentGateway, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IMediator mediator, IQueue queue)
    {
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _queue = queue;
    }

    public async Task<Result> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var responsePaymentGateway = await _paymentGateway.ProccessPayment(request);

        var total = request.Items.Sum(i => i.Amount * i.Quantity);

        var transaction = Transaction.Create(request.OrderId, request.CustomerId, Guid.Parse(responsePaymentGateway.Id), total, request.PaymentType);

        _transactionRepository.Add(transaction);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(request);

        if (request.PaymentType != "credit") await _queue.PublishAsync(new { request.OrderId, Url = responsePaymentGateway.PaymentUrl }, "payment.url");

        return Result.Success();
    }
}
