using MediatR;

namespace Domain.Customers;

public class CustomerCreatedEvent : INotification
{
    public CustomerCreatedEvent(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public string Name { get; set; }
    public string Email { get; set; }
}
