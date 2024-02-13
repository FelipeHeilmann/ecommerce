namespace Domain.Orders.DomainEvents;

public record OrderCheckoutedEvent(Guid OrderId, Guid CustomerId ,DateTime CreateAt, Double Total);
