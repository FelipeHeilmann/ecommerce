using Application.Abstractions.Messaging;
using Application.Customers.Model;

namespace Application.Customers.Create;

public record CreateAccountCommand(CreateAccountRequest request) : ICommand<Guid>;

