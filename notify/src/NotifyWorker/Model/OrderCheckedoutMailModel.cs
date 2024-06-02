namespace NotifyWorker.Model;

public record OrderCheckedoutMailModel(string Name, Guid OrderId, DateTime Date, List<OrderChecedoutItem> Items);
public record OrderChecedoutItem(Guid Id, string Name, int Quantity, double Price);
