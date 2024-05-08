using Domain.Shared;

namespace Domain.Customers.Error;

public static class CustomerErrors
{
    public static Error CustomerNotFound => Error.NotFound("Customer.Not.Found", "The customer was not found");
    public static Error EmailFormat => Error.Validation("Email.Invalid", "Invalid email format");
    public static Error NameFormat => Error.Validation("Name.Invalid", "Invalid name format");
    public static Error EmailIsAreadyUsed => Error.Failure("Email.Used", "The email is already used");
    public static Error InvalidAge => Error.Validation("Invalid.Age", "Age should be greatter than 18 years");
    public static Error CustomerInvalidCredencials => Error.Validation("Customer.Invalid.Credentials", "Invalid email or/and password");
    public static Error EmailAlredyInUse => Error.Conflict("Email.In.Use", "This is email is already in use");
    public static Error CPFFormat => Error.Validation("CPF.Invalid", "Cpf invalid");
    public static Error PhoneFormat => Error.Validation("Phone.Invalid", "Phone invalid");
}

