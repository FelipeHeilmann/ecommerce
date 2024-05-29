using Application.Abstractions.Messaging;

namespace Application.Transactions.ProccessTransaction;

public record ProccessTransactionCommand(Guid TransactionGatewayId, string Status): ICommand;
