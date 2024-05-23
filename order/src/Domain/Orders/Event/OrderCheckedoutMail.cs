namespace Domain.Orders.Events;

public sealed record OrderCheckedoutMail(Guid OrderId, DateTime Date, string Name, string Email, List<OrderCheckedoutIMailItem> Items);
public record OrderCheckedoutIMailItem(string Name, double Price, int Quantity);

