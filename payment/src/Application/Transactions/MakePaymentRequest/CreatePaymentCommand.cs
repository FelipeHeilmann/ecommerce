using Application.Abstractions.Messaging;
using Domain.Events;

namespace Application.Transactions.MakePaymentRequest;

public record CreatePaymentCommand (OrderCheckedout request) : ICommand<Guid>;
