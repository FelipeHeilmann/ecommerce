using Application.Abstractions;
using Application.Customers.Services;
using Application.Data;
using Domain.Customer;
using Domain.Shared;

namespace Application.Customers.Create;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Guid>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(ICustomerRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var emailAreadyUsed = await _repository.IsEmailUsedAsync(request.Email, cancellationToken);

        if(emailAreadyUsed) return Result.Failure<Guid>(CustomerErrors.EmailAlredyInUse);

        var hashedPassword = HashPasswordService.Generate(request.password);

        var result = Customer.Create(request.Name, request.Email, hashedPassword, request.birthDate);

        if(result.IsFailure) return Result.Failure<Guid>(result.Error);

        var customer = result.Data;

        _repository.Add(customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(customer.Id);
    }
}
