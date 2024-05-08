using Domain.Shared;
namespace Domain.Customers;

public class Customer
{
    public Guid Id { get; private set; }
    private Name _name;
    private Email _email;
    private CPF _cpf;
    private Phone _phone;
    public string Name { get => _name.Value; private set => _name = Customers.Name.Create(value).Value; }
    public string Email { get => _email.Value; private set => _email = Customers.Email.Create(value).Value; }
    public string CPF { get => _cpf.Value; private set => _cpf = Customers.CPF.Create(value).Value; }
    public string Phone { get => _phone.Value; private set => _phone = Customers.Phone.Create(value).Value; }
    public string Password { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Customer(Guid id, Name name, Email email, CPF cpf, Phone phone ,string password, DateOnly birthDate, DateTime createdAt)
    {
        Id = id;
        _name = name;
        _email = email;
        _cpf = cpf;
        _phone = phone;
        BirthDate = birthDate;
        CreatedAt = createdAt;
        Password = password;
    }

    public Customer() { }

    public static Result<Customer> Create(string nameString, string emailString, string password, DateOnly birthDate, string cpfString, string phoneString)
    {
        var name = Customers.Name.Create(nameString);
        if (name.IsFailure) return Result.Failure<Customer>(name.Error);

        var email = Customers.Email.Create(emailString);
        if (email.IsFailure) return Result.Failure<Customer>(email.Error);
        
        var cpf = Customers.CPF.Create(cpfString);
        if (cpf.IsFailure) return Result.Failure<Customer>(cpf.Error);

        if (!IsOldEnough(birthDate))
            return Result.Failure<Customer>(CustomerErrors.InvalidAge);

        var phone = Customers.Phone.Create(phoneString);
        if (cpf.IsFailure) return Result.Failure<Customer>(phone.Error);


        return new Customer(Guid.NewGuid(), name.Value, email.Value, cpf.Value, phone.Value, password, birthDate, DateTime.UtcNow);
    }

    private static bool IsOldEnough(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
        {
            age--;
        }
        return age >= 18;
    }
}
