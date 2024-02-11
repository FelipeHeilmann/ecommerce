using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Data;
using Domain.Customer;
using Domain.Shared;

namespace Application.Customers.Create;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Guid>
{
    private readonly ICustomerRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(ICustomerRepository repository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var email = Email.Create(request.Email);

        if (email.IsFailure) return Result.Failure<Guid>(email.Error);

        var emailAreadyUsed = await _repository.IsEmailUsedAsync(email.Value, cancellationToken);

        if(emailAreadyUsed) return Result.Failure<Guid>(CustomerErrors.EmailAlredyInUse);

        var hashedPassword = _passwordHasher.Generate(request.password);

        var birthDate = new DateOnly(request.birthDate.Year, request.birthDate.Month, request.birthDate.Day);

        var result = Customer.Create(request.Name, request.Email, hashedPassword, birthDate);

        if(result.IsFailure) return Result.Failure<Guid>(result.Error);

        var customer = result.Value;

        _repository.Add(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(customer.Id);
    }
}
