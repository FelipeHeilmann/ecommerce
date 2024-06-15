using Domain.Customers.Error;
using Domain.Customers.VO;
namespace Domain.Customers.Entity;

public class Customer
{
    private Name _name;
    private Email _email;
    private CPF _cpf;
    private Phone _phone;
    private Password _password;
    public Guid Id { get; private set; }
    public string Name => _name.Value; 
    public string Email => _email.Value; 
    public string CPF => _cpf.Value; 
    public string Phone => _phone.Value;
    public string Password => _password.Value;
    public DateTime BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Customer(Guid id, Name name, Email email, CPF cpf, Phone phone, Password password, DateTime birthDate, DateTime createdAt)
    {
        Id = id;
        _name = name;
        _email = email;
        _cpf = cpf;
        _phone = phone;
        BirthDate = birthDate;
        CreatedAt = createdAt;
        _password = password;
    }

    public static Customer Create(string name, string email, string password, DateTime birthDate, string cpf, string phone)
    {
        if (!IsOldEnough(birthDate)) throw new UnderAge();

        return new Customer(Guid.NewGuid(), new Name(name), new Email(email), new CPF(cpf), new Phone(phone), BcryptPassword.Create(password), birthDate, DateTime.UtcNow);
    }

    public static Customer Restore(Guid id, string name, string email, string cpf, string phone, string password, DateTime birthDate, DateTime createdAt)
    {
        return new Customer(id, new Name(name), new Email(email), new CPF(cpf), new Phone(phone), BcryptPassword.Restore(password), birthDate, createdAt);
    }

    public bool PasswordMatches(string password)
    {
        return _password.PasswordMatches(password);
    }

    private static bool IsOldEnough(DateTime birthDate)
    {
        var today = DateTime.Now;
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month || today.Month == birthDate.Month && today.Day < birthDate.Day)
        {
            age--;
        }
        return age >= 18;
    }
}