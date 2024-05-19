using Application.Abstractions.Services;
using Application.Addresses.Create;
using Application.Addresses.GetByCustomerId;
using Application.Addresses.GetById;
using Application.Addresses.Model;
using Application.Addresses.Update;
using Application.Customers.Create;
using Application.Customers.Model;
using Application.Data;
using Domain.Addresses.Repository;
using Domain.Customers.Event;
using Domain.Customers.Repository;
using Infra.Data;
using Infra.Implementations;
using Infra.Repositories.Memory;
using MediatR;
using Moq;
using Xunit;

namespace Integration;

public class AddressTest
{
    private readonly IAddressRepository addressRepository = new AddressRepositoryInMemory();
    private readonly ICustomerRepository customerRepository = new CustomerRepositoryMemory();
    private readonly IPasswordHasher passwordHasher = new PasswordHasher();
    private readonly IUnitOfWork unitOfWork = new UnitOfWorkMemory();

    [Fact]
    public async Task Sould_Create_Address()
    {
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediatorMock.Object);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateAddress = new CreateAddressRequest(outputCreateCustomer.Value, "04182-123", "Rua C", "Jardim Sacoma", "112", "apt 43", "São Paulo", "São Paulo", "Brasil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository, unitOfWork);

        var outputCreateAddress = await createAddressCommandHandler.Handle(new CreateAddressCommand(inputCreateAddress), CancellationToken.None);

        var getAddressQueryHandler = new GetAddressByIdQueryHandler(addressRepository);

        var outputGetAddress = await getAddressQueryHandler.Handle(new GetAddressByIdQuery(outputCreateAddress.Value), CancellationToken.None);

        Assert.Equal("São Paulo", outputGetAddress.Value.State);
        Assert.Equal("apt 43", outputGetAddress.Value.Complement);
        Assert.Equal("São Paulo", outputGetAddress.Value.State);
    }

    [Fact]
    public async Task Sould_Get_Addresses_By_Customer_Id()
    {
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediatorMock.Object);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateAddress1 = new CreateAddressRequest(outputCreateCustomer.Value, "04182-123", "Rua C", "Jardim Sacoma", "112", "apt 43", "São Paulo", "São Paulo", "Brasil");
        var inputCreateAddress2 = new CreateAddressRequest(outputCreateCustomer.Value, "03246-435", "Rua A", "Jardim Sacoma", "115", null, "São Paulo", "São Paulo", "Brasil");
        var inputCreateAddress3= new CreateAddressRequest(outputCreateCustomer.Value, "04082-168", "Rua B", "Jardim Sacoma2", "116", "apt 49", "São Paulo", "São Paulo", "Brasil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository, unitOfWork);

        await createAddressCommandHandler.Handle(new CreateAddressCommand(inputCreateAddress1), CancellationToken.None);
        await createAddressCommandHandler.Handle(new CreateAddressCommand(inputCreateAddress2), CancellationToken.None);
        await createAddressCommandHandler.Handle(new CreateAddressCommand(inputCreateAddress3), CancellationToken.None);

        var getAddressesByCustomerIdQueryHandler = new GetAddressesByCustomerIdQueryHandler(addressRepository);

        var outputGetAddresses = await getAddressesByCustomerIdQueryHandler.Handle(new GetAddressesByCustomerIdQuery(outputCreateCustomer.Value), CancellationToken.None);

        Assert.Equal(3, outputGetAddresses.Value.Count);

        Assert.Equal("Rua C", outputGetAddresses.Value.ToList()[0].Street);
        Assert.Equal("apt 43", outputGetAddresses.Value.ToList()[0].Complement);
        Assert.Equal("04182-123", outputGetAddresses.Value.ToList()[0].ZipCode);


        Assert.Equal("Rua A", outputGetAddresses.Value.ToList()[1].Street);
        Assert.Equal(null, outputGetAddresses.Value.ToList()[1].Complement);
        Assert.Equal("03246-435", outputGetAddresses.Value.ToList()[1].ZipCode);


        Assert.Equal("Rua B", outputGetAddresses.Value.ToList()[2].Street);
        Assert.Equal("apt 49", outputGetAddresses.Value.ToList()[2].Complement);
        Assert.Equal("04082-168", outputGetAddresses.Value.ToList()[2].ZipCode);
    }


    [Fact]
    public async Task Should_Update_Address()
    {
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediatorMock.Object);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateAddress = new CreateAddressRequest(outputCreateCustomer.Value, "03246-435", "Rua A", "Jardim Sacoma", "115", null, "São Paulo", "São Paulo", "Brasil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository, unitOfWork);

        var outputCreateAddress = await createAddressCommandHandler.Handle(new CreateAddressCommand(inputCreateAddress), CancellationToken.None);

        var inputUpdateAddress = new UpdateAddressRequest(outputCreateAddress.Value, outputCreateCustomer.Value, "04182-123", "Rua C", "Jardim Sacoma", "112", "apt 43", "São Paulo", "São Paulo", "Brasil");

        var updateAddressCommandHandler = new UpdateAddressCommandHandler(addressRepository, unitOfWork);

        await updateAddressCommandHandler.Handle(new UpdateAddressCommand(inputUpdateAddress), CancellationToken.None);

        var getAddressQueryHandler = new GetAddressByIdQueryHandler(addressRepository);

        var outputGetAddress = await getAddressQueryHandler.Handle(new GetAddressByIdQuery(outputCreateAddress.Value), CancellationToken.None);

        Assert.Equal("Rua C", outputGetAddress.Value.Street);
        Assert.Equal("apt 43", outputGetAddress.Value.Complement);
        Assert.Equal("04182-123", outputGetAddress.Value.ZipCode);
    }

}
