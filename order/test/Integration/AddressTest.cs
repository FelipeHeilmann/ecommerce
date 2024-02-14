using Application.Addresses.Create;
using Application.Addresses.GetByCustomerId;
using Application.Addresses.GetById;
using Application.Data;
using Domain.Addresses;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class AddressTest
{
    private readonly IAddressRepository _addressRepository = new AddressRepositoryInMemory();
    private readonly IUnitOfWork _unitOfWork = new UnitOfWorkMemory();
    public AddressTest() 
    {
        RepositorySetup.PopulateAddressRepository(_addressRepository);
    }

    [Fact]
    public async Task Sould_Create_Address()
    {
        var customerId = Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0");
        var zipcode = "04182-123";
        var street = "Rua C";
        var neighborhood = "Jardim Sacoma";
        var number = "112";
        var apartment = "43";
        var city = "São Paulo";
        var state = "São Paulo";
        var country = "Brasil";

        var request = new CreateAddressRequest(customerId, zipcode, street, neighborhood, number, apartment, city, state, country);

        var command = new CreateAddressCommand(request);

        var commandHandler = new CreateAddressCommandHandler(_addressRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value is Guid);
    }

    [Fact]
    public async Task Sould_Get_Addresses_By_Customer_Id()
    {
        var customerId = Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0");

        var query = new GetAddressesByCustomerIdQuery(customerId);

        var queryHandler = new GetAddressesByCustomerIdQueryHandler(_addressRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value is ICollection<Address>);
    }

    [Fact]
    public async Task Sould_Get_Address_By_Id()
    {
        var addressId = Guid.Parse("2b169c76-acee-4ddf-86c4-37af9fbb07ea");

        var query = new GetAddressByIdQuery(addressId);

        var queryHandler = new GetAddressByIdQueryHandler(_addressRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value is Address);
    }
    [Fact]
    public async Task Sould_Not_Get_Addresses_By_Id()
    {
        var addressId = Guid.NewGuid();

        var query = new GetAddressByIdQuery(addressId);

        var queryHandler = new GetAddressByIdQueryHandler(_addressRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(AddressErrors.NotFound, result.Error);
    }

}
