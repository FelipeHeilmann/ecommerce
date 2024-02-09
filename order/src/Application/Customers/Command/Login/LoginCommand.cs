using Application.Abstractions;
using Application.Customers.Model;
using Domain.Shared;

namespace Application.Customers.Command.Login;

public record LoginCommand(LoginModel request) : ICommand<Result<string>>;
