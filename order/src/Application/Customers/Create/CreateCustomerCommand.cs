using Application.Abstractions.Messaging;

namespace Application.Customers.Create;

public record CreateCustomerCommand(string Name, string Email, string password, DateTime birthDate, string CPF, string Phone) : ICommand<Guid>;

