using Application.Abstractions.Queue;
using Application.Abstractions.Services;
using Application.Addresses.Create;
using Application.Addresses.GetByCustomerId;
using Application.Addresses.GetById;
using Application.Addresses.Update;
using Application.Customers.Create;
using Domain.Addresses.Repository;
using Domain.Customers.Repository;
using Infra.Implementations;
using Infra.Queue;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class AddressTest
{
    private readonly IAddressRepository addressRepository;
    private readonly ICustomerRepository customerRepository;
    private readonly IPasswordHasher passwordHasher;
    private readonly IQueue queue;

    public AddressTest() 
    {
        addressRepository = new AddressRepositoryMemory();
        customerRepository = new CustomerRepositoryMemory();
        passwordHasher = new PasswordHasher();
        queue = new MemoryMQAdapter();
    }

    [Fact]
    public async Task Sould_Create_Address()
    {
        var inputCreateCustomer = new CreateCustomerCommand("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateAddress = new CreateAddressCommand(outputCreateCustomer.Value, "04182-123", "Rua C", "Jardim Sacoma", "112", "apt 43", "São Paulo", "São Paulo", "Brasil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        var outputCreateAddress = await createAddressCommandHandler.Handle(inputCreateAddress, CancellationToken.None);

        var getAddressQueryHandler = new GetAddressByIdQueryHandler(addressRepository);

        var outputGetAddress = await getAddressQueryHandler.Handle(new GetAddressByIdQuery(outputCreateAddress.Value), CancellationToken.None);

        Assert.Equal("São Paulo", outputGetAddress.Value.State);
        Assert.Equal("apt 43", outputGetAddress.Value.Complement);
        Assert.Equal("São Paulo", outputGetAddress.Value.State);
    }

    [Fact]
    public async Task Sould_Get_Addresses_By_Customer_Id()
    {
        var inputCreateCustomer = new CreateCustomerCommand("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateAddress1 = new CreateAddressCommand(outputCreateCustomer.Value, "04182-123", "Rua C", "Jardim Sacoma", "112", "apt 43", "São Paulo", "São Paulo", "Brasil");
        var inputCreateAddress2 = new CreateAddressCommand(outputCreateCustomer.Value, "03246-435", "Rua A", "Jardim Sacoma", "115", null, "São Paulo", "São Paulo", "Brasil");
        var inputCreateAddress3= new CreateAddressCommand(outputCreateCustomer.Value, "04082-168", "Rua B", "Jardim Sacoma2", "116", "apt 49", "São Paulo", "São Paulo", "Brasil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        await createAddressCommandHandler.Handle(inputCreateAddress1, CancellationToken.None);
        await createAddressCommandHandler.Handle(inputCreateAddress2, CancellationToken.None);
        await createAddressCommandHandler.Handle(inputCreateAddress3, CancellationToken.None);

        var getAddressesByCustomerIdQueryHandler = new GetAddressesByCustomerIdQueryHandler(addressRepository);

        var outputGetAddresses = await getAddressesByCustomerIdQueryHandler.Handle(new GetAddressesByCustomerIdQuery(outputCreateCustomer.Value), CancellationToken.None);

        Assert.Equal(3, outputGetAddresses.Value.Count);

        Assert.Equal("Rua C", outputGetAddresses.Value.ToList()[0].Street);
        Assert.Equal("apt 43", outputGetAddresses.Value.ToList()[0].Complement);
        Assert.Equal("04182-123", outputGetAddresses.Value.ToList()[0].ZipCode);


        Assert.Equal("Rua A", outputGetAddresses.Value.ToList()[1].Street);
        Assert.Null(outputGetAddresses.Value.ToList()[1].Complement);
        Assert.Equal("03246-435", outputGetAddresses.Value.ToList()[1].ZipCode);


        Assert.Equal("Rua B", outputGetAddresses.Value.ToList()[2].Street);
        Assert.Equal("apt 49", outputGetAddresses.Value.ToList()[2].Complement);
        Assert.Equal("04082-168", outputGetAddresses.Value.ToList()[2].ZipCode);
    }


    [Fact]
    public async Task Should_Update_Address()
    {
        var inputCreateCustomer = new CreateCustomerCommand("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateTime(2004, 6, 2), "97067401046", "11 97414-6507");

        var createAccountCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createAccountCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateAddress = new CreateAddressCommand(outputCreateCustomer.Value, "03246-435", "Rua A", "Jardim Sacoma", "115", null, "São Paulo", "São Paulo", "Brasil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        var outputCreateAddress = await createAddressCommandHandler.Handle(inputCreateAddress, CancellationToken.None);

        var inputUpdateAddress = new UpdateAddressCommand(outputCreateAddress.Value, outputCreateCustomer.Value, "04182-123", "Rua C", "Jardim Sacoma", "112", "apt 43", "São Paulo", "São Paulo", "Brasil");

        var updateAddressCommandHandler = new UpdateAddressCommandHandler(addressRepository);

        await updateAddressCommandHandler.Handle(inputUpdateAddress, CancellationToken.None);

        var getAddressQueryHandler = new GetAddressByIdQueryHandler(addressRepository);

        var outputGetAddress = await getAddressQueryHandler.Handle(new GetAddressByIdQuery(outputCreateAddress.Value), CancellationToken.None);

        Assert.Equal("Rua C", outputGetAddress.Value.Street);
        Assert.Equal("apt 43", outputGetAddress.Value.Complement);
        Assert.Equal("04182-123", outputGetAddress.Value.ZipCode);
    }
}
