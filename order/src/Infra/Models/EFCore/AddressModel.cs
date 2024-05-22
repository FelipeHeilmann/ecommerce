using Domain.Addresses.Entity;
using Domain.Addresses.VO;
using System.Net;

namespace Infra.Models.EFCore;

public class AddressModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string ZipCode { get; set; }
    public string Street { get; set; }
    public string Neighborhood { get; set; }
    public string Number { get; set; }
    public string? Complement { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public AddressModel(Guid id, Guid customerId, string zipCode, string street, string neighborhood, string number, string? complement, string city, string state, string country)
    {
        Id = id;
        CustomerId = customerId;
        ZipCode = zipCode;
        Street = street;
        Neighborhood = neighborhood;
        Number = number;
        Complement = complement;
        City = city;
        State = state;
        Country = country;
    }

    public AddressModel() { }

    public static AddressModel FromAggregate(Address address)
    {
        return new AddressModel(address.Id, address.CustomerId, address.ZipCode, address.Street, address.Neighborhood, address.Number, address.Complement, address.City, address.State, address.Country);
    }

    public Address ToAggregate()
    {
        return new Address(Id, CustomerId, new ZipCode(ZipCode), Street, Neighborhood, Number, Complement, City, State, Country);
    }

}
