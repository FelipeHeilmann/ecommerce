namespace Domain.Orders.Event;

public record TransactionStatusChanged(Guid Id, Guid OrderId, DateTime ApprovedOrRejectedAt, string Status);
