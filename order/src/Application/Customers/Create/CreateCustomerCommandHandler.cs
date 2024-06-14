using Application.Abstractions.Messaging;
using Application.Abstractions.Queue;
using Application.Abstractions.Services;
using Domain.Customers.Entity;
using Domain.Customers.Error;
using Domain.Customers.Repository;
using Domain.Shared;

namespace Application.Customers.Create;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IQueue _queue;

    public CreateCustomerCommandHandler(ICustomerRepository repository, IPasswordHasher passwordHasher, IQueue queue)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _queue = queue;
    }

    public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var emailAreadyUsed = await _repository.IsEmailUsedAsync(command.Email, cancellationToken);

        if(emailAreadyUsed) return Result.Failure<Guid>(CustomerErrors.EmailAlredyInUse);

        var hashedPassword = _passwordHasher.Generate(command.password);

        var customer = Customer.Create(command.Name, command.Email, hashedPassword, command.birthDate, command.CPF, command.Phone);
 
        await _repository.Add(customer);

        await _queue.PublishAsync(new { customer.Name, customer.Email }, "customer.created");

        return Result.Success(customer.Id);
    }
}
