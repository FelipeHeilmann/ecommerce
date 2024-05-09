namespace Domain.Customers.Error;

public static class CustomerErrors
{
    public static Shared.Error CustomerNotFound => Shared.Error.NotFound("Customer.Not.Found", "The customer was not found");
    public static Shared.Error EmailIsAreadyUsed => Shared.Error.Failure("Email.Used", "The email is already used");
    public static Shared.Error InvalidAge => Shared.Error.Validation("Invalid.Age", "Age should be greatter than 18 years");
    public static Shared.Error CustomerInvalidCredencials => Shared.Error.Validation("Customer.Invalid.Credentials", "Invalid email or/and password");
    public static Shared.Error EmailAlredyInUse => Shared.Error.Conflict("Email.In.Use", "This is email is already in use");
}

