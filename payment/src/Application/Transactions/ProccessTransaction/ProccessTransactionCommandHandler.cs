using Application.Abstractions.Messaging;
using Application.Abstractions.Queue;
using Domain.Shared;
using Domain.Transactions.Repository;

namespace Application.Transactions.ProccessTransaction;

public class ProccessTransactionCommandHandler : ICommandHandler<ProccessTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IQueue _queue;

    public ProccessTransactionCommandHandler(ITransactionRepository transactionRepository, IQueue queue)
    {
        _transactionRepository = transactionRepository;
        _queue = queue;
    }

    public async Task<Result> Handle(ProccessTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByGatewayServiceId(command.TransactionGatewayId, cancellationToken);

        if(transaction is null) throw new Exception();

        transaction.Register("TransactionStatusChanged", async domainEvent => {

            await _queue.PublishAsync(domainEvent.Data, "transaction.status.changed");
        });
        
        if(command.Status == "approved")
        {
            transaction.Approve();
        }
        if(command.Status == "rejected")
        {
            transaction.Reject();
        }

        await _transactionRepository.Update(transaction);

        return Result.Success();
    }
}
