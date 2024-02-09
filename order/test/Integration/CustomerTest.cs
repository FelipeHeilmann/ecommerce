using Application.Customers.Command;
using Application.Customers.Model;
using Application.Data;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class CustomerTest
{
    private readonly CustomerRepositoryMemory _customerRepository = new();
    private readonly UnitOfWorkMemory _unitOfWork = new UnitOfWorkMemory();
    [Fact]
    public async Task Should_Create_A_Customer()
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
}
