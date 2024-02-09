using Application.Abstractions;
using Application.Customers.Services;
using Domain.Customer;
using Domain.Shared;

namespace Application.Customers.Command;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Result<Customer>>
{
    private readonly ICustomerRepository _repository;

    public CreateAccountCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public Task<Result<Customer>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var hashedPassword = HashPasswordService.Generate(request.password);

        var result = Customer.Create(request.Name, request.Email, hashedPassword, request.birthDate);

        if(result.IsFailure) return Task.FromResult(Result.Failure<Customer>(result));

        var customer = result.Data;

        _repository.Add(customer);

        return Task.FromResult(Result.Success<Customer>(result));
    }
}
