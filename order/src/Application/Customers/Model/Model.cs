namespace Application.Customers.Model;

public record CreateAccountRequest(string Name, string Email, string password, DateTime birthDate, string CPF, string Phone);

public record LoginRequest(string Email, string Password);
