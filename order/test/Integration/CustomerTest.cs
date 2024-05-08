using Application.Abstractions.Services;
using Application.Customers.Create;
using Application.Customers.Model;
using Application.Data;
using Domain.Customers.Error;
using Domain.Customers.Event;
using Domain.Customers.Repository;
using Infra.Data;
using Infra.Implementations;
using Infra.Repositories.Memory;
using MediatR;
using Moq;
using Xunit;

namespace Integration;

public class CustomerTest
{
    private readonly ICustomerRepository _customerRepository = new CustomerRepositoryMemory();
    private readonly IPasswordHasher _passwordHasher = new PasswordHasher();
    private readonly IUnitOfWork _unitOfWork = new UnitOfWorkMemory();
    [Fact]
    public async Task Should_Create_Customer()
    {
        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";

        var request = new CreateAccountRequest(name, email, password, birthDate, "97067401046", "11 97414-6507");

        var command = new CreateAccountCommand(request);

        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var commandHandler = new CreateAccountCommandHandler(_customerRepository, _unitOfWork, _passwordHasher, mediatorMock.Object);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public async Task Should_Not_Create_Customer_Due_Email_In_Use()
    {
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await new CreateAccountCommandHandler(_customerRepository, _unitOfWork, _passwordHasher, mediatorMock.Object).Handle(new CreateAccountCommand(new CreateAccountRequest("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507")), CancellationToken.None);

        var name = "Felipe Heilmann";
        var email = "felipeheilmannm@gmail.com";
        var birthDate = new DateTime(2004, 6, 2);
        var password = "senha";
        var phone = "11 97414-6507";
        var cpf = "97067401046";

        var request = new CreateAccountRequest(name, email, password, birthDate,cpf, phone);

        var command = new CreateAccountCommand(request);

        var commandHandler = new CreateAccountCommandHandler(_customerRepository, _unitOfWork, _passwordHasher, mediatorMock.Object);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(CustomerErrors.EmailAlredyInUse, result.Error);
    }
}
