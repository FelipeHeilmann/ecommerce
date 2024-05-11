namespace Domain.Orders.Event;

public record OrderPaymentUrlEvent(Guid OrderId, string Url, string PaymentType);
