using Domain.Shared;

namespace Domain.Customers;

public class Customer
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Customer(Guid id, Name name, Email email, string password, DateOnly birthDate, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Email = email;
        BirthDate = birthDate;
        CreatedAt = createdAt;
        Password = password;
    }

    public static Result<Customer> Create(string nameString, string emailString, string password, DateOnly birthDate)
    {
        var name = Name.Create(nameString);
        if (name.IsFailure) return Result.Failure<Customer>(name.Error);

        var email = Email.Create(emailString);
        if (email.IsFailure) return Result.Failure<Customer>(email.Error);

        if (!IsOldEnough(birthDate))
            return Result.Failure<Customer>(CustomerErrors.InvalidAge);

        return new Customer(Guid.NewGuid(), name.Value, email.Value, password, birthDate, DateTime.UtcNow);
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
