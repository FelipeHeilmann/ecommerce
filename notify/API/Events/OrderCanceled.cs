namespace API.Events;

public record OrderCanceled(Guid OrderId, Guid CustomerId);
