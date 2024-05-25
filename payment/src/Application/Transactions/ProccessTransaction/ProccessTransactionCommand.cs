using Application.Abstractions.Messaging;

namespace Application.Transactions.ProccessTransaction;

public record ProccessTransactionCommand(Guid TransactionId, string Status): ICommand;
