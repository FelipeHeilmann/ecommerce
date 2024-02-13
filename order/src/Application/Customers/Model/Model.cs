namespace Application.Customers.Model;

public record CreateAccountRequest(string Name, string Email, string password, DateTime birthDate);

public record LoginRequest(string Email, string Password);
