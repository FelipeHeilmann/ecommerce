﻿using MediatR;

namespace Domain.Customers.Event;

public class CustomerCreatedEvent : INotification
{
    public string Name { get; set; }
    public string Email { get; set; }

    public CustomerCreatedEvent(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
