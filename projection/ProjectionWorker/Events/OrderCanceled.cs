namespace ProjectionWorker.Events;

public record OrderCanceled(Guid OrderId, Guid CustomerId);
