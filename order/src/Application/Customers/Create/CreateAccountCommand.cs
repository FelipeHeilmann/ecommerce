using Application.Abstractions;
using Application.Customers.Model;

namespace Application.Customers.Create;

public record CreateAccountCommand(CreateAccountModel request) : ICommand<Guid>;

