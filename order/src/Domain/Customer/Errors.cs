using Domain.Shared;

namespace Domain.Customer;

public static class CustomerErrors
{
    public static Error EmailNull => Error.Validation("Email.Null.Empty", "The email value should not be empty or null");
    public static Error EmailFormat => Error.Validation("Email.Invalid", "Invalid email format");
    public static Error EmailIsAreadyUsed => Error.Failure("Email.Used", "The email is already used");
    public static Error NameNull => Error.Validation("Name.Null.Empty", "The name value should not be empty or null");
    public static Error NameFormat => Error.Validation("Name.Invalid", "Invalid name format");
    public static Error InvalidAge => Error.Validation("Invalid.Age", "Age should be greatter than 18 years");
    public static Error CustomerInvalidCredencials => Error.NotFound("Customer.Invalid.Credentials", "Invalid or/and email");
}

