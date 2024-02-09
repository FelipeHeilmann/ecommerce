using Domain.Shared;

namespace Domain.Customer;

public class Customer
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public DateTime BirhDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Customer(Guid guid, Name name, Email email, string Password ,DateTime birthDate, DateTime createdAt)
    {
        Id = guid;
        Name = name;
        Email = email;
        BirhDate = birthDate;
        CreatedAt = createdAt;
    }

    public static Result<Customer> Create(string nameString, string emailString, string password ,DateTime birthDate) 
    {
        var name = Name.Create(nameString);
        if (name.IsFailure) return Result.Failure<Customer>(name.Error);

        var email = Email.Create(emailString);
        if (email.IsFailure) return Result.Failure<Customer>(email.Error);

        if ((DateTime.Now - birthDate).TotalDays < 18 * 365.25) return Result.Failure<Customer>(CustomerErrors.InvalidAge);
        return new Customer(Guid.NewGuid(), name, email, password, birthDate, DateTime.Now);

    }
}
