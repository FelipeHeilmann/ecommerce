using Domain.Orders;

namespace Application.Gateway;

public interface INotifyGateway
{
    public Task SendWelcomeMail(string name, string email);
    public Task SendOrderCreatedMail(OrderCreatedMail order);
}

public record OrderCreatedMail(Guid OrderId, DateTime Date, string Name, string Email, List<ItemsMail> Items);
public record ItemsMail(string Name, double Price, int Quantity);



