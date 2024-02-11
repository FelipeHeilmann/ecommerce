using Application.Abstractions.Messaging;
using Application.Customers.Model;

namespace Application.Customers.Login;

public record LoginCommand(LoginModel request) : ICommand<string>;
