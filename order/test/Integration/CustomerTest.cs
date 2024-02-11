using Application.Customers.Create;
using Application.Customers.Login;
using Application.Customers.Model;
using Domain.Customer;
using Infra.Authenication;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class CustomerTest
{
    private readonly CustomerRepositoryMemory _customerRepository = new();
    private readonly UnitOfWorkMemory _unitOfWork = new UnitOfWorkMemory();
    private readonly JwtProvider _jwtProvider = new(new JwtOptions("issuer", "audience", "my-test-key"));
    [Fact]
    public async Task Should_Create_Customer()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var request = new CreateAccountModel(name, email, password, birthDate);

        var command = new CreateAccountCommand(request);

        var commandHandler = new CreateAccountCommandHandler(_customerRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public async Task Should_Not_Create_Customer_Due_Email_In_Use()
    {
        await new CreateAccountCommandHandler(_customerRepository, _unitOfWork).Handle(new CreateAccountCommand(new CreateAccountModel("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2))), CancellationToken.None);

        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var request = new CreateAccountModel(name, email, password, birthDate);

        var command = new CreateAccountCommand(request);

        var commandHandler = new CreateAccountCommandHandler(_customerRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(CustomerErrors.EmailAlredyInUse, result.Error);
    }
}
