using Domain.Addresses.VO;
using Domain.Shared;

namespace Domain.Addresses.Entity;

public class Address
{
    private ZipCode _zipCode;
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string ZipCode { get => _zipCode.Value; }
    public string Street { get; private set; }
    public string Neighborhood { get; private set; }
    public string Number { get; private set; }
    public string? Complement { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }

    public Address() { }
    public Address(
        Guid id,
        Guid customerId,
        ZipCode zipCode,
        string street,
        string neighborhood,
        string number,
        string? complement,
        string city,
        string state,
        string country
    )
    {
        Id = id;
        CustomerId = customerId;
        _zipCode = zipCode;
        Street = street;
        Neighborhood = neighborhood;
        Number = number;
        Complement = complement;
        City = city;
        State = state;
        Country = country;
    }

    public static Address Create(
        Guid customerId,
        string zipCode,
        string street,
        string neighborhood,
        string number,
        string? complement,
        string city,
        string state,
        string country
    )
    {
 
        return new Address(
            Guid.NewGuid(),
            customerId,
            new ZipCode(zipCode),
            street,
            neighborhood,
            number,
            complement,
            city,
            state,
            country
        );
    }

    public void Update(
        string zipCode,
        string street,
        string neighborhood,
        string number,
        string? complement,
        string city,
        string state,
        string country
    )
    {
      
        _zipCode = new ZipCode(zipCode);
        Street = street;
        Neighborhood = neighborhood;
        Number = number;
        Complement = complement;
        City = city;
        State = state;
        Country = country;
    }

}
