using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Customers.Error;
using Domain.Customers.Repository;
using Domain.Shared;

namespace Application.Customers.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(ICustomerRepository customerRepository, IJwtProvider jwtProvider)
    {
        _customerRepository = customerRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (customer == null) return Result.Failure<string>(CustomerErrors.CustomerInvalidCredencials);

        if(!customer.PasswordMatches(command.Password)) return Result.Failure<string>(CustomerErrors.CustomerInvalidCredencials);

        var token = _jwtProvider.Generate(customer);

        return token;
    }
}
