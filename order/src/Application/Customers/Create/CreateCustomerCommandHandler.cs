using Application.Abstractions.Messaging;
using Application.Abstractions.Queue;
using Application.Abstractions.Services;
using Application.Data;
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
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(ICustomerRepository repository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IQueue queue)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _queue = queue;
    }

    public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var emailAreadyUsed = await _repository.IsEmailUsedAsync(command.Email, cancellationToken);

        if(emailAreadyUsed) return Result.Failure<Guid>(CustomerErrors.EmailAlredyInUse);

        var hashedPassword = _passwordHasher.Generate(command.password);

        var birthDate = new DateOnly(command.birthDate.Year, command.birthDate.Month, command.birthDate.Day);

        var customer = Customer.Create(command.Name, command.Email, hashedPassword, birthDate, command.CPF, command.Phone);
 
        _repository.Add(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _queue.PublishAsync(new { customer.Name, customer.Email }, "customer.created");

        return Result.Success(customer.Id);
    }
}
