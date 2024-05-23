namespace API.Events;


public sealed record OrderCheckedoutEvent(Guid OrderId, DateTime Date, string Name, string Email, List<OrderCreatedItem> Items);
public record OrderCreatedItem(string Name, double Price, int Quantity);
