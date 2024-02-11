using Application.Abstractions;
using Application.Customers.Model;

namespace Application.Customers.Login;

public record LoginCommand(LoginModel request) : ICommand<string>;
