namespace Domain.Events;

public record TransactionCreated(Guid TransactionId, Guid OrderId, string? PaymentUrl);

