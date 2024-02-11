namespace Application.Customers.Model;

public record CreateAccountModel(string Name, string Email, string password, DateTime birthDate);

public record LoginModel(string Email, string Password);
