using Domain.Shared;
namespace Domain.Customers;

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

    public Customer(Guid id, Name name, Email email, CPF cpf, Phone phone ,string password, DateOnly birthDate, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Email = email;
        BirthDate = birthDate;
        CreatedAt = createdAt;
        Password = password;
        CPF = cpf;  
        Phone = phone;
    }

    public Customer() { }

    public static Result<Customer> Create(string nameString, string emailString, string password, DateOnly birthDate, string cpfString, string phoneString)
    {
        var name = Name.Create(nameString);
        if (name.IsFailure) return Result.Failure<Customer>(name.Error);

        var email = Email.Create(emailString);
        if (email.IsFailure) return Result.Failure<Customer>(email.Error);
        
        var cpf = CPF.Create(cpfString);
        if (cpf.IsFailure) return Result.Failure<Customer>(cpf.Error);

        if (!IsOldEnough(birthDate))
            return Result.Failure<Customer>(CustomerErrors.InvalidAge);

        var phone = Phone.Create(phoneString);
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
