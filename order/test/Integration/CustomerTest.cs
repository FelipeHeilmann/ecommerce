using Application.Abstractions.Queue;
using Application.Customers.Create;
using Application.Customers.GetById;
using Domain.Customers.Error;
using Domain.Customers.Repository;
using Infra.Queue;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class CustomerTest
{
    private readonly ICustomerRepository customerRepository;
    private readonly IQueue queue;

    public CustomerTest()
    {
        customerRepository = new CustomerRepositoryMemory();
        queue = new MemoryQueueAdapter();
    }

    [Fact]
    public async Task Should_Create_Customer()
    {
        var inputCreateCustomer = new CreateCustomerCommand("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateCustomerCommandHandler(customerRepository, queue);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var getCustomerById = new GetCustomerByIdQueryHandler(customerRepository);

        var outputGetCustomer = await getCustomerById.Handle(new GetCustomerByIdQuery(outputCreateCustomer.Value), CancellationToken.None);

        Assert.Equal("Felipe Heilmann", outputGetCustomer.Value.Name);
        Assert.Equal("felipeheilmannm@gmail.com", outputGetCustomer.Value.Email);
    }

    [Fact]
    public async Task Should_Not_Create_Customer_Due_Email_In_Use()
    {
        await new CreateCustomerCommandHandler(customerRepository,queue).Handle(new CreateCustomerCommand("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507"), CancellationToken.None);

        var inputCreateAccount = new CreateCustomerCommand("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 11, 6), "97067401046", "11 97414-6507");

        var commandHandler = new CreateCustomerCommandHandler(customerRepository, queue);

        var outputCreateAccount = await commandHandler.Handle(inputCreateAccount, CancellationToken.None);

        Assert.True(outputCreateAccount.IsFailure);
        Assert.False(outputCreateAccount.IsSuccess);
        Assert.Equal(CustomerErrors.EmailAlredyInUse, outputCreateAccount.Error);
    }
}
