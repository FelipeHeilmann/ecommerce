namespace API.Events;

public record TransactionStatusChanged(Guid Id, Guid OrderId, DateTime ApprovedOrRejectedAt, string Status);