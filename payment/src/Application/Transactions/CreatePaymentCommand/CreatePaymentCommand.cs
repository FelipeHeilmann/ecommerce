using Application.Abstractions.Messaging;
using Domain.Events;

namespace Application.Transactions.CreatePaymentCommand;

public record CreatePaymentCommand (OrderCheckedout request) : ICommand<Guid>;
