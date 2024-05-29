using Application.Abstractions.Messaging;

namespace Application.Orders.OrderPaymentStatusChanged;

public record OrderPaymentStatusChangedCommand(Guid OrderId, string Status): ICommand;
