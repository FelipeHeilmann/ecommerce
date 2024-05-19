using Application.Abstractions.Services;
using Application.Customers.Create;
using Application.Customers.GetById;
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
    private readonly ICustomerRepository customerRepository = new CustomerRepositoryMemory();
    private readonly IPasswordHasher passwordHasher = new PasswordHasher();
    private readonly IUnitOfWork unitOfWork = new UnitOfWorkMemory();
    [Fact]
    public async Task Should_Create_Customer()
    {
     
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediatorMock.Object);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var getCustomerById = new GetCustomerByIdQueryHandler(customerRepository);

        var outputGetCustomer = await getCustomerById.Handle(new GetCustomerByIdQuery(outputCreateCustomer.Value), CancellationToken.None);

        Assert.Equal("Felipe Heilmann", outputGetCustomer.Value.Name);
        Assert.Equal("felipeheilmannm@gmail.com", outputGetCustomer.Value.Email);
    }

    [Fact]
    public async Task Should_Not_Create_Customer_Due_Email_In_Use()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object).Handle(new CreateAccountCommand(new CreateCustomerRequest("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507")), CancellationToken.None);

        var inputCreateAccount = new CreateCustomerRequest("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 11, 6), "97067401046", "11 97414-6507");

        var commandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object);

        var outputCreateAccount = await commandHandler.Handle(new CreateAccountCommand(inputCreateAccount), CancellationToken.None);

        Assert.True(outputCreateAccount.IsFailure);
        Assert.False(outputCreateAccount.IsSuccess);
        Assert.Equal(CustomerErrors.EmailAlredyInUse, outputCreateAccount.Error);
    }
}
