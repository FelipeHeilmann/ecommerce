namespace API.Requests;

public record CreateCustomerRequest(string Name, string Email, string password, DateTime birthDate, string CPF, string Phone);

public record LoginRequest(string Email, string Password);
