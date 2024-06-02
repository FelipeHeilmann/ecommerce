namespace NotifyWorker.Events;

public record OrderCanceled(Guid OrderId, Guid CustomerId);
