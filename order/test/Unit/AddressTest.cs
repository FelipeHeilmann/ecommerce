﻿using Domain.Addresses.Entity;
using Domain.Addresses.Error;
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

        Assert.Equal(street,address.Street);
        Assert.Equal(neighborhood, address.Neighborhood);
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

        Assert.Throws<InvalidZipCode>(() => Address.Create(customerId, zicode, street, neighborhood, number, apartment, city, state, country));
    }

}
