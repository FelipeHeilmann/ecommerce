using Application.Abstractions;
using Application.Customers.Model;
using Domain.Customer;
using Domain.Shared;

namespace Application.Customers.Create;

public record CreateAccountCommand(CreateAccountModel request) : ICommand<Result<Customer>>;

