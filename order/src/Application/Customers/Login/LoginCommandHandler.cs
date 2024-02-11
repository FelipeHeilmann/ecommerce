using Application.Abstractions;
using Application.Customers.Services;
using Domain.Customer;
using Domain.Shared;

namespace Application.Customers.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<string>>
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
        var email = Email.Create(command.request.Email).Data;

        var customer = await _customerRepository.GetByEmailAsync(email!.Value, cancellationToken);

        if (customer == null) return Result.Failure<string>(CustomerErrors.CustomerInvalidCredencials);

        var verifiedPassword = HashPasswordService.Verify(command.request.Password, customer!.Password);

        if(!verifiedPassword) return Result.Failure<string>(CustomerErrors.CustomerInvalidCredencials);

        var token = _jwtProvider.Generate(customer);

        return token;
    }
}
