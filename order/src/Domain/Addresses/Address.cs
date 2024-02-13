using Domain.Shared;

namespace Domain.Addresses;

public class Address
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public ZipCode ZipCode { get; private set; }
    public string Street { get; private set; } 
    public string Neighborhood { get; private set; }
    public string Number { get; private set; }
    public string? Apartment {  get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }

    public Address(
        Guid id, 
        Guid customerId, 
        ZipCode zipCode, 
        string street, 
        string neighborhood,
        string number, 
        string? apartment,
        string city, 
        string state, 
        string country
    )
    {
        Id = id;
        CustomerId = customerId;
        ZipCode = zipCode;
        Street = street;
        Neighborhood = neighborhood;
        Number = number;
        Apartment = apartment;
        City = city;
        State = state;
        Country = country;
    }

    public static Result<Address> Create(
        Guid customerId, 
        string zipCodeString,
        string street, 
        string neighborhood,
        string number, 
        string? apartment, 
        string city, 
        string state, 
        string country
    )
    {
        var zipCode = ZipCode.Create(zipCodeString);

        if (zipCode.IsFailure) return Result.Failure<Address>(zipCode.Error);

        return new Address(
            Guid.NewGuid(),
            customerId,
            zipCode.Value,
            street,
            neighborhood,
            number,
            apartment,
            city,
            state,
            country
        );
    }

}
