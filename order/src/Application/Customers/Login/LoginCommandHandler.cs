using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Customers.Error;
using Domain.Customers.Repository;
using Domain.Shared;

namespace Application.Customers.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(ICustomerRepository customerRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _customerRepository = customerRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {

        var customer = await _customerRepository.GetByEmailAsync(command.request.Email, cancellationToken);

        if (customer == null) return Result.Failure<string>(CustomerErrors.CustomerInvalidCredencials);

        var verifiedPassword = _passwordHasher.Verify(command.request.Password, customer!.Password);

        if(!verifiedPassword) return Result.Failure<string>(CustomerErrors.CustomerInvalidCredencials);

        var token = _jwtProvider.Generate(customer);

        return token;
    }
}
