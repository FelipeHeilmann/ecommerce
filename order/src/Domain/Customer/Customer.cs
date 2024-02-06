using Domain.Shared;

namespace Domain.Customer;

public class Customer
{
    public Guid Guid { get; set; }
    public Name Name { get; set; }
    public Email Email { get; set; }
    public DateTime DateBirth { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer(Guid guid, Name name, Email email, DateTime dateBirth, DateTime createdAt)
    {
        Guid = guid;
        Name = name;
        Email = email;
        DateBirth = dateBirth;
        CreatedAt = createdAt;
    }

    public static Result<Customer> Create(string nameString, string emailString, DateTime dateBirth) 
    {
        var name = Name.Create(nameString);
        if (name.IsFailure) return Result.Failure<Customer>(name.Error);

        var email = Email.Create(emailString);
        if (email.IsFailure) return Result.Failure<Customer>(email.Error);

        if ((DateTime.Now - dateBirth).TotalDays < 18 * 365.25) return Result.Failure<Customer>(CustomerErrors.InvalidAge);
        return new Customer(Guid.NewGuid(), name, email, dateBirth, DateTime.Now);

    }
}
