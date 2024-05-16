using Domain.Customers.Entity;
using Domain.Customers.VO;

namespace Infra.Models;

public class CustomerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string CPF { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public DateOnly BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public CustomerModel(Guid id, string name, string email, string cPF, string phone, string password, DateOnly birthDate, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Email = email;
        CPF = cPF;
        Phone = phone;
        Password = password;
        BirthDate = birthDate;
        CreatedAt = createdAt;
    }

    public CustomerModel() { }

    public static CustomerModel FromAggregate(Customer customer)
    {
        return new CustomerModel(customer.Id, customer.Name, customer.Email, customer.CPF, customer.Phone, customer.Password, customer.BirthDate, customer.CreatedAt);
    }

    public Customer ToAggregate()
    {
        return new Customer(Id, new Name(Name), new Email(Email), new CPF(CPF), new Phone(Phone), Password, BirthDate, CreatedAt);
    }
}
