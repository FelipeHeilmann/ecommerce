using Domain.Customers.Error;
using Domain.Customers.VO;
namespace Domain.Customers.Entity;

public class Customer
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public CPF CPF { get; private set; }
    public Phone Phone { get; private set; }
    public string Password { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Customer() { }
    public Customer(Guid id, Name name, Email email, CPF cpf, Phone phone, string password, DateOnly birthDate, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Email = email;
        CPF = cpf;
        Phone = phone;
        BirthDate = birthDate;
        CreatedAt = createdAt;
        Password = password;
    }

    public string GetName()
    {
        return Name.Value;
    }

 
    public string GetPhone()
    {
        return Phone.Value;
    }

    public string GetCPF()
    {
        return CPF.Value;
    }

    public static Customer Create(string name, string email, string password, DateOnly birthDate, string cpf, string phone)
    {
        if (!IsOldEnough(birthDate)) throw new UnderAge();

        return new Customer(Guid.NewGuid(), new Name(name), new Email(email), new CPF(cpf), new Phone(phone), password, birthDate, DateTime.UtcNow);
    }

    private static bool IsOldEnough(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month || today.Month == birthDate.Month && today.Day < birthDate.Day)
        {
            age--;
        }
        return age >= 18;
    }
}
