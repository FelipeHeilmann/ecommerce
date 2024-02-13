using Domain.Addresses;
using Xunit;

namespace Unit;

public class AddressTest
{
    [Fact]
    public void Should_Create_Valid_Address()
    {
        var customerId = Guid.NewGuid();
        var zicode = "04182-123";
        var street = "Rua C";
        var neighborhood = "Jardim Sacoma";
        var number = "112";
        var apartment = "43";
        var city = "São Paulo";
        var state = "São Paulo";
        var country = "Brasil";

        var address = Address.Create(customerId, zicode, street, neighborhood, number, apartment, city, state, country);

        Assert.True(address.IsSuccess);
        Assert.False(address.IsFailure);
    }

    [Fact]
    public void Should_Not_Create_Address()
    {
        var customerId = Guid.NewGuid();
        var zicode = "04182123";
        var street = "Rua C";
        var neighborhood = "Jardim Sacoma";
        var number = "112";
        var apartment = "43";
        var city = "São Paulo";
        var state = "São Paulo";
        var country = "Brasil";

        var address = Address.Create(customerId, zicode, street, neighborhood, number, apartment, city, state, country);

        Assert.False(address.IsSuccess);
        Assert.True(address.IsFailure);
    }

}
