using Application.Abstractions.Messaging;
using Application.Abstractions.Queue;
using Application.Data;
using Domain.Shared;
using Domain.Transactions.Repository;

namespace Application.Transactions.ProccessTransaction;

public class ProccessTransactionCommandHandler : ICommandHandler<ProccessTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQueue _queue;

    public ProccessTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IQueue queue)
    {
        _transactionRepository = transactionRepository;
        _queue = queue;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ProccessTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByGatewayServiceId(command.TransactionGatewayId, cancellationToken);

        transaction.Register("TransactionApproved", async domainEvent => {

            await _queue.PublishAsync(domainEvent.Data, "transaction.approved");
        });
        
        if(command.Status == "approved")
        {
            transaction.Approve();
        }
        if(command.Status == "rejected")
        {
            transaction.Reject();
        }

        _transactionRepository.Update(transaction);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
